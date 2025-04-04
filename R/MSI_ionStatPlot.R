imports "ggspatial" from "MSImaging";

#' Combined Statistical Plot and Mass Spectrometry Imaging Visualization
#'
#' Generates a composite visualization combining statistical charts (e.g., bar/box/violin plots) 
#' with mass spectrometry imaging (MSI) data. Outputs to specified PNG file with customizable layout.
#'
#' @param mzpack An mzpack object containing MS imaging data and layer information. Must be 
#'               compatible with ggplot MS imaging functions.
#' @param mz Target ion mass-to-charge ratio (m/z) value for imaging.
#' @param met List containing sample intensity data. List keys should be sample IDs with 
#'            corresponding intensity values.
#' @param sampleinfo Metadata list defining sample groups and visualization properties. Requires:
#'    \itemize{
#'      \item \code{group}: Character vector of group names (plot titles)
#'      \item \code{id}: Character vector of sample IDs corresponding to \code{met} keys
#'      \item \code{color}: HTML color codes for each group's statistical plot elements
#'    }
#' @param savePng Output file path for PNG image. Default: "./Rplot.png".
#' @param ionName Display title for the ion. Uses m/z value when \code{NULL} (default).
#' @param size Image dimensions in pixels [width, height]. Default: \code{c(2400, 1000)}.
#' @param colorMap Named list mapping group names to custom colors (overrides \code{sampleinfo$color}).
#' @param MSI_colorset Color palette for MS imaging. Default: "viridis:turbo".
#' @param ggStatPlot Statistical plot geometry function. Default: bar plot (\code{geom_barplot}).
#' @param padding_top Top padding in pixels. Default: 150.
#' @param padding_right Right padding in pixels. Default: 200.
#' @param padding_bottom Bottom padding in pixels. Default: 150.
#' @param padding_left Left padding in pixels. Default: 150.
#' @param interval Spacing between statistical plot and MS image in pixels. Default: 50.
#' @param combine_layout Layout ratio [stat_plot_width, msi_width]. Default: \code{c(4, 5)} 
#'        (converted to 44.4% vs 55.6% of available width).
#' @param jitter_size Point size for jittered data points. Default: 8.
#' @param TrIQ Intensity quantile threshold (0-1) for MS image enhancement. Default: 0.65.
#' @param backcolor Background color for MS imaging panel. Default: "black".
#' @param regions Optional list with spatial coordinates to overlay on MS image:
#'    \itemize{
#'      \item \code{x}: X-coordinates vector
#'      \item \code{y}: Y-coordinates vector
#'      \item \code{colors}: Corresponding colors vector
#'    }
#'
#' @details 
#' The function performs three main operations:
#' 1. Computes ANOVA group statistics using \code{ANOVAGroup()}
#' 2. Creates a statistical plot (bar/box/violin) with significance annotations
#' 3. Generates MS ion image with optional spatial region overlays
#' 
#' Plots are combined using a customizable split-layout system. Statistical tests include:
#' - ANOVA global test (displayed in plot)
#' - Pairwise t-tests against global mean (p-value symbols)
#'
#' @note Requires 'ggspatial' and 'MSImaging' packages. The \code{ANOVAGroup()} function 
#'       must be available in the environment.
#'
#' @examples
#' \dontrun{
#' MSI_ionStatPlot(
#'   mzpack = sample_data,
#'   mz = 885.454,
#'   met = intensity_list,
#'   sampleinfo = list(
#'     group = c("Control", "Treatment"),
#'     id = c("S1", "S2"),
#'     color = c("#FF0000", "#00FF00")
#'   ),
#'   savePng = "analysis_output.png",
#'   combine_layout = c(3, 5)
#' )
#' }
#' @export
const MSI_ionStatPlot = function(mzpack, mz, met, sampleinfo, 
                                 savePng        = "./Rplot.png", 
                                 ionName        = NULL,
                                 size           = [2400, 1000], 
                                 colorMap       = NULL, 
                                 MSI_colorset   = "viridis:turbo",
                                 ggStatPlot     = NULL, 
                                 padding_top    = 150,
                                 padding_right  = 200,
                                 padding_bottom = 150,
                                 padding_left   = 150,
                                 interval       = 50,
                                 combine_layout = [4, 5], 
                                 jitter_size    = 2, 
                                 TrIQ           = 0.65,
                                 backcolor      = "black", 
                                 regions        = NULL, 
                                 swap           = FALSE, 
                                 title_fontsize = 40, 
                                 font_family    = "Times New Roman",
                                 show_legend    = TRUE,
                                 show_grid      = TRUE,
                                 show_stats     = TRUE,
                                 show_axis.msi  = TRUE,
                                 tic_outline    = NULL) {

    bitmap(file = savePng, size = size, fill = "white");

    print("open the graphics device at location:");
    print(savePng);

    if (is.logical(tic_outline)) {
        if (tic_outline) {
            # generates the raster image if parameter is logical TRUE
            tic_outline <- MSI_sampleTIC(rawdata = mzpack, dims = NULL, filters = default_MSIfilter());
        } else {
            # set to nothing if the parameter is logcial FALSE
            tic_outline <- NULL;
        }
    } else {
        # is the raster image data object
        # just do nothing
    }

    if ((length(sampleinfo) == 3) && all(["group", "id", "color"] in sampleinfo)) {
        sampleinfo = sampleinfo |> groupBy("group");
        sampleinfo = lapply(sampleinfo, function(group) {
            if (is.data.frame(group)) {
                list(group = unique(group$group),
                    id = group$id,
                    color = first(group$color)
                );
            } else {
                list(group = unique(group@group),
                    id = group@id,
                    color = first(group@color)
                );
            }           
        });
    }

    let width = size[1] - (padding_left + padding_right);
    # mzkit::ANOVAGroup
    let data = ANOVAGroup(met, sampleinfo);

    ionName = ifelse(is.null(ionName), `M/Z: ${mz |> toString(format = "F3")}`, ionName);
    combine_layout = combine_layout / sum(combine_layout);   

    let left  = width * combine_layout[1];
    let right = width * combine_layout[2];
    let layout_left = `padding: ${padding_top}px ${padding_right + right}px ${padding_bottom}px ${padding_left}px;`;
    let layout_right = `padding: ${padding_top}px ${padding_right}px ${padding_bottom}px ${padding_right + left + interval}px;`;

    if (swap) {
        # swap of the plot layout
        [layout_left, layout_right] = list(
            layout_left  = layout_right, 
            layout_right = layout_left 
        );
    }

    print("previews of the ANOVA group data:");
    print(data, max.print = 13);

    if (is.null(colorMap)) {
        colorMap = lapply(sampleinfo, i -> i$color);
    }
    if (is.null(ggStatPlot)) {
        ggStatPlot = function(colorMap) {
            geom_barplot(
                width = 0.65, 
                color = colorMap
            );
        }
    }

    # chartting at left
    let bar = ggplot(data, aes(x = "region_group", y = "intensity"), padding = layout_left);

    if (show_stats) {
        # Add horizontal line at base mean 
        bar <- bar + geom_hline(yintercept = mean(data$intensity), linetype="dash", line.width = 6, color = "red");
    }
    
    bar <- bar 
    + ggStatPlot(colorMap)
    + geom_jitter(width = 0.2, radius = jitter_size, color = colorMap, adjust = "darker")	
    # + ggtitle(ionName)
    + ylab("intensity")
    + xlab("")
    + scale_y_continuous(labels = "G2", n = 5)
    ;

    if (show_stats) {
        # Add global annova p-value 
        bar <- bar + stat_compare_means(method = "anova", label.y = 1600) 
                   # Pairwise comparison against all
                   + stat_compare_means(label = "p.signif", method = "t.test", ref.group = ".all.", hide.ns = TRUE)
                   # + geom_signif(list(vs = tag))	
                   # + stat_pvalue_manual(pvalue)
    }
    
    bar <- bar + theme(
        axis.text.x      = element_text(angle = 45, family = font_family, size = 24), 
        axis.text        = element_text(family = font_family, size = title_fontsize * 0.8), 
        axis.title       = element_text(family = font_family, size = title_fontsize * 0.85, face = "bold"), 
        plot.title       = element_text(family = font_family, size = 16),
        panel.grid       = ifelse(show_grid, "stroke: lightgray; stroke-width: 2px; stroke-dash: dash;", element_blank()),
        panel.grid_major = ifelse(show_grid, "stroke: lightgray; stroke-width: 2px; stroke-dash: dash;", element_blank())
    )
    ;

    # ms-imaging at right
    let ion = ggplot(mzpack, padding = layout_right)
    # rendering of a single ion m/z
    # default color palette is Jet color set
    + geom_msimaging(
        mz        = mz,
        tolerance = mzkit::tolerance("da", 0.1),
        TrIQ      = TrIQ,
        knnFill   = TRUE,
        color     = MSI_colorset,
        raster    = tic_outline
    )
    + geom_MSIbackground(backcolor)
    + default_MSIfilter()
    # add ggplot charting elements
    + ggtitle("")
    + labs(x = "Dimension(X)", y = "Dimension(Y)")
    + scale_x_continuous(labels = "F0")
    + scale_y_continuous(labels = "F0")
    + theme(
        panel.grid = element_blank(), 
        legend.position = ifelse(show_legend, NULL, "none")
    )
    ;

    if (!show_axis.msi) {
        ion <- ion + theme_void();
    }

    plot(ggplot(padding = `padding: ${padding_top}px ${padding_right}px ${padding_bottom}px ${padding_left}px;`) 
        + ggtitle(ionName) 
        + theme(plot.title = element_text(family = font_family, size = title_fontsize, face = "bold"))
    );
    plot(bar);

    if (is.null(regions)) {
        plot(ion);         
    } else {
        plot(ion + geom_spatialScatter(regions$x, regions$y, regions$colors));
    }       

    dev.off();
}