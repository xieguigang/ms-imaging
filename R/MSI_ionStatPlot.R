imports "ggspatial" from "MSImaging";

#' Stat plot combine with the MS-imaging
#' 
#' @param mzpack a mzpack data object used for do MS-imaging 
#'    or other data object that contains the ms-imaging layer 
#'    data and could be recognized by the ggplot ms-imaging
#'    function.
#' @param ionName the display title string. the ion m/z value 
#'    will be used if this parameter leaves default NULL.
#' @param mz the target ion m/z value.
#' @param met should be a list data object that contains the
#'    sample data, which this list the key names is the sample 
#'    id set and the corresponding element value is the sample 
#'    intensity value.
#' @param sampleinfo a data list set which could be used for 
#'    defined the sample group information and the plot color 
#'    value. this list data should be contains at least 3 
#'    required data fields:
#'
#'       1. group: the sample group name, used for display as 
#'          the title label of each group data
#'       2. id: a character vector that contains the necessary
#'          sample id reference to get the required sample data
#'          for each data group
#'       3. color: a single character string value in html color
#'          format for specific the color of the bar/box/violin
#'          stat plot.
#' 
#' @param colorMap a list object that contains the color set value
#'    that used for rendering the stat chartting, example as box/bar/violin.
#'    this list data object the key names should be the sample group 
#'    tag and the corresponding value must be the sample intensity 
#'    data value.
#' @param MSI_colorset the color map name for do MS-imaging 
#'    rendering, default color scale name is ``viridis:turbo``.
#' @param ggStatPlot default is bar plot.
#' @param combine_layout the layout of the stat charting 
#'   and the MS-imaging plot. a numeric vector that contains
#'   two value element, first is the width percentage of the 
#'   stats chart and the second one is the width percentage
#'   of the single ion ms-imaging.
#' @param savePng the filepath to save the image plot output.
#' @param backcolor the background color of the MS-imaging plot
#' 
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
                                 jitter_size    = 8, 
                                 TrIQ           = 0.65,
                                 backcolor      = "black", 
                                 regions        = NULL) {

    bitmap(file = savePng, size = size, fill = "white");

    print("open the graphics device at location:");
    print(savePng);

    # mzkit::ANOVAGroup
    let data = ANOVAGroup(met, sampleinfo);
    let width = size[1] - (padding_left + padding_right);

    ionName = ifelse(is.null(ionName), `M/Z: ${mz |> toString(format = "F3")}`, ionName);
    combine_layout = combine_layout / sum(combine_layout);   

    let left  = width * combine_layout[1];
    let right = width * combine_layout[2];
    let layout_left = `padding: ${padding_top}px ${padding_right + right}px ${padding_bottom}px ${padding_left}px;`;
    let layout_right = `padding: ${padding_top}px ${padding_right}px ${padding_bottom}px ${padding_right + left + interval}px;`;

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

    print("previews of the ANOVA group data:");
    print(data, max.print = 13);

    # chartting at left
    const bar = ggplot(data, aes(x = "region_group", y = "intensity"), padding = layout_left)
    # Add horizontal line at base mean 
    + geom_hline(yintercept = mean(data$intensity), linetype="dash", line.width = 6, color = "red")
    + ggStatPlot(colorMap)
    + geom_jitter(width = 0.3, radius = jitter_size, color = colorMap)	
    # + ggtitle(ionName)
    + ylab("intensity")
    + xlab("")
    + scale_y_continuous(labels = "G2")
    # Add global annova p-value 
    + stat_compare_means(method = "anova", label.y = 1600) 
    # Pairwise comparison against all
    + stat_compare_means(label = "p.signif", method = "t.test", ref.group = ".all.", hide.ns = TRUE)
    # + geom_signif(list(vs = tag))	
    # + stat_pvalue_manual(pvalue)
    + theme(
        axis.text.x = element_text(angle = 45), 
        plot.title  = element_text(family = "Cambria Math", size = 16)
    )
    ;

    # ms-imaging at right
    const ion = ggplot(mzpack, padding = layout_right)
    # rendering of a single ion m/z
    # default color palette is Jet color set
    + geom_msimaging(
        mz        = mz,
        tolerance = mzkit::tolerance("da", 0.1),
        TrIQ      = TrIQ,
        knnFill   = TRUE,
        color     = MSI_colorset
    )
    + geom_MSIbackground(backcolor)
    + default_MSIfilter()
    # add ggplot charting elements
    + ggtitle("")
    + labs(x = "Dimension(X)", y = "Dimension(Y)")
    + scale_x_continuous(labels = "F0")
    + scale_y_continuous(labels = "F0")
    + theme(panel.grid = element_blank())
    ;

    plot(ggplot(padding = `padding: ${padding_top}px ${padding_right}px ${padding_bottom}px ${padding_left}px;`) 
        + ggtitle(ionName) 
        + theme(plot.title = element_text(family = "Cambria Math", size = 32))
    );
    plot(bar);

    if (is.null(regions)) {
        plot(ion);         
    } else {
        plot(ion + geom_spatialScatter(regions$x, regions$y, regions$colors));
    }       

    dev.off();
}