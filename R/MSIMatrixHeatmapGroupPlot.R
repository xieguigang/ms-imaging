#' Plot MS-Imaging heatmap matrix
#' 
#' @param ions_data a list of MSI layers data, each 
#'    element in this list should also be a list 
#'    object that contains data slots:
#'     
#'       1. type  precursor type information string
#'       2. title   the ion metabolite name
#'       3. layer   the MSI ion layer data
#' 
const PlotMSIMatrixHeatmap = function(ions_data, 
                                      layout        = [3,3],
                                      colorSet      = "Jet",
                                      MSI_TrIQ      = 0.8,
                                      gaussian      = 3,
                                      size          = [2700, 2000], 
                                      canvasPadding = [50, 300, 50, 50], 
                                      cellPadding   = [200, 100, 0, 100]) {
    let NxN as integer = layout;
    let cambria   = rasterFont("Cambria", 27, "Bold");
    let cellWidth = (size[1] - canvasPadding[2] - canvasPadding[4]) / NxN[1]; 

    ions_data = ions_data
    |> lapply(function(ion) {
        ion$layer = ion$layer |> knnFill();
        ion$TrIQ  = TrIQ(ion$layer, q = MSI_TrIQ) * max(intensity(ion$layer));
        ion$layer = intensityLimits(ion$layer, max = ion$TrIQ[2]);
        ion;
    });
    
    layout = layout |> layout.grid(
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

        [ion]::MSILayer 
        |> rasterHeatmap(
            region       = region, 
            gauss        = gaussian, 
            colorName    = colorSet, 
            rasterBitmap = TRUE
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