#' Plot MS-Imaging Heatmap Matrix in Grid Layout
#' 
#' Generates a matrix of mass spectrometry imaging (MSI) heatmaps in grid layout 
#' with intensity filtering, spatial smoothing, and formatted metabolite labels.
#' Supports custom color schemes, layout configurations, and intensity filters.
#'
#' @param ions_data A list of MSI layer data. Each element must be a list containing:
#'    \itemize{
#'      \item{type: Character string specifying precursor type (e.g., "[M+H]+")}
#'      \item{title: Character string of metabolite/ion name for labeling}
#'      \item{layer: Matrix or raster object containing spatial intensity data}
#'      \item{mz: Numeric m/z value of target ion (used in label generation)}
#'    }
#' @param filters Optional custom intensity filter(s) for raster data preprocessing. 
#'    If NULL (default), applies Top x% Intensity Quantile (TrIQ) filtering via `MSI_TrIQ`.
#' @param layout Integer vector of length 2 specifying grid dimensions [rows, columns]. 
#'    Default: [3,3].
#' @param colorSet Character specifying color palette name (e.g., "Jet", "viridis"). 
#'    Default: "Jet".
#' @param MSI_TrIQ Numeric [0-1] specifying Top x% Intensity Quantile threshold for 
#'    default intensity filtering. Values > threshold are clipped. Default: 0.8.
#' @param gaussian Numeric specifying Gaussian blur radius (in pixels) for spatial 
#'    smoothing. Set to 0 to disable. Default: 3.
#' @param size Integer vector of length 2 specifying canvas dimensions [width, height] 
#'    in pixels. Default: [2700, 2000].
#' @param canvasPadding Integer vector of length 4 specifying canvas margins 
#'    [top, right, bottom, left] in pixels. Default: [50, 300, 50, 50].
#' @param cellPadding Integer vector of length 4 specifying grid cell margins 
#'    [top, right, bottom, left] in pixels. Default: [200, 100, 0, 100].
#' @param font_size Numeric specifying base font size for labels. Default: 27.
#' @param strict Logical. If TRUE, enforces dimension consistency checks. Default: TRUE.
#' @param msi_dimension Optional integer vector of length 2 specifying output dimensions 
#'    [width, height] for rasterization. If NULL, uses native data dimensions.
#'
#' @return Invisibly returns NULL. Generates a composite plot in the active graphics device
#'    containing:
#'    \itemize{
#'      \item{Grid of MSI heatmaps with Gaussian smoothing}
#'      \item{Color legend using specified palette}
#'      \item{Metabolite labels with split formatting (name, precursor type, m/z)}
#'    }
#'
#' @details 
#' Key processing steps:
#' \enumerate{
#'   \item Applies intensity filtering (default or custom) to all ion layers
#'   \item Generates grid layout with specified cell/canvas padding
#'   \item Renders color legend using selected palette
#'   \item Draws each ion heatmap with optional Gaussian smoothing
#'   \item Adds formatted multi-line labels below each heatmap:
#'      \itemize{
#'        \item{Line 1: Metabolite name (auto-wrapped at 32 characters)}
#'        \item{Line 2: Precursor type (from `type` slot)}
#'        \item{Line 3: m/z value formatted to 3 decimal places}
#'      }
#' }
#' 
#' @seealso \code{\link{rasterHeatmap}}, \code{\link{layout.grid}}, \code{\link{colorMap.legend}}
#' @export
const PlotMSIMatrixHeatmap = function(ions_data, 
                                      layout        = [3,3],
                                      colorSet      = "Jet",
                                      MSI_TrIQ      = 0.8,
                                      gaussian      = 3,
                                      size          = [2700, 2000], 
                                      canvasPadding = [50, 300, 50, 50], 
                                      cellPadding   = [200, 100, 0, 100], 
                                      font_size     = 27,
                                      filters       = NULL,
                                      strict        = TRUE,
                                      msi_dimension = NULL) {
    let NxN as integer = layout;
    let cambria   = rasterFont("Cambria", 27, "Bold");
    let cellWidth = (size[1] - canvasPadding[2] - canvasPadding[4]) / NxN[1]; 
    let apply_msi_filter = {
        if (length(filters) == 0) {
            # apply of the default filter for the single ions data
            x -> default_intensity_filter(x, MSI_TrIQ);
        } else {
            x -> custom_intensity_filter(x, filters, MSI_TrIQ);
        }
    }

    ions_data = lapply(ions_data, ion -> apply_msi_filter(ion));
    layout    = layout |> layout.grid(
        margin = `padding: ${cellPadding[1]}px ${cellPadding[2]}px ${cellPadding[3]}px ${cellPadding[4]}px;`
    );

    print("grid layouts:");
    print(layout);
    print("width of a cell:");
    print(cellWidth);

    colorSet 
    |> colorMap.legend(
        [0, 100],
        titleFont = "font-style: strong; font-size: 16; font-family: Cambria;", 
        tickFont  = "font-style: normal; font-size: 16; font-family: Cambria;", 
        title     = "", 
        format    = "F0", 
        foreColor = "white"
    )
    |> plot()
    ;     

    # draw each cell drawing
    for(i in 1:length(layout)) {
        const region   = layout[i];
        const ion_data = ions_data[[i]];
        const ion      = ion_data$layer;

        i = as.vector(region);

        [ion]::MSILayer |> rasterHeatmap(
            region       = region, 
            gauss        = gaussian, 
            colorName    = colorSet, 
            rasterBitmap = TRUE,
            strict       = strict,
            dimSize      = msi_dimension
        );

        print("raw label text of current ion image:");
        print(ion_data$title);

        let labels as string = ion_data$title;
        let max = NULL;
        let h = NULL;
        let x = NULL;
        let y = NULL;

        labels = labels 
        |> Html::plainText() 
        |> splitParagraph(32) 
        |> append(as.character([ion_data$type, toString(ion_data$mz, format = "F3")]))
        ;
        max    = labels[which.max(sapply(labels, s -> nchar(s)))];
        max    = measureString(max, font = cambria);
        h      = max[2] * (length(labels) + 0.125);

        x = i[1];
        y = i[2] - h;

        print("split as multiple line labels:");
        print(labels);
        print("max size of the label parts:");
        print(max);

        # draw multiple line text
        for(line in labels) {
            const lb_size = measureString(line, font = cambria);
            const yx = y;

            x = (cellWidth - lb_size[1]) / 2 + i[1] - cellPadding[4];
            # move to new line
            y = y + lb_size[2];

            text(x, yx, line, col = "white", font = cambria);
        }    

        cat("\n\n");
    }
}