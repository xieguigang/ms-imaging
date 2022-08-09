
#' Stat plot combine with the MS-imaging
#' 
#' @param ionName the display title string.
#' @param mz the target ion m/z value.
#' @param ggStatPlot default is bar plot.
#' @param combine_layout the layout of the stat charting 
#'   and the MS-imaging plot. a numeric vector that contains
#'   two value element, first is the width percentage of the 
#'   stats chart and the second one is the width percentage
#'   of the single ion ms-imaging.
#' 
const MSI_ionStatPlot = function(mzpack, mz, ionName, met, sampleinfo, 
                                 savePng        = "./Rplot.png", 
                                 size           = [2400, 1000], 
                                 colorMap       = NULL, 
                                 ggStatPlot     = NULL, 
                                 padding_top    = 150,
                                 padding_right  = 200,
                                 padding_bottom = 150,
                                 padding_left   = 150,
                                 interval       = 50,
                                 combine_layout = [4, 5], 
                                 jitter_size    = 8, 
                                 TrIQ           = 0.65) {

    bitmap(file = savePng, size = size, fill = "white");

    print("open the graphics device at location:");
    print(savePng);

    data = ANOVAGroup(met, sampleinfo);
    combine_layout = combine_layout / sum(combine_layout);
    width = size[1] - (padding_left + padding_right);
    left = width * combine_layout[1];
    right = width * combine_layout[2];

    layout_left = `padding: ${padding_top}px ${padding_right + right}px ${padding_bottom}px ${padding_left}px;`;
    layout_right = `padding: ${padding_top}px ${padding_right}px ${padding_bottom}px ${padding_right + left + interval}px;`;

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
    bar = ggplot(data, aes(x = "region_group", y = "intensity"), padding = layout_left)
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
    ion = ggplot(mzpack, padding = layout_right)
    # rendering of a single ion m/z
    # default color palette is Jet color set
    + geom_msimaging(
        mz        = mz,
        tolerance = mzkit::tolerance("da", 0.1),
        TrIQ      = TrIQ,
        knnFill   = TRUE,
        color     = "viridis:turbo"
    )
    + geom_MSIbackground("black")
    + MSI_knnfill(qcut = 0.5)
    # + MSI_gaussblur()
    # add ggplot charting elements
    + ggtitle("")
    + labs(x = "Dimension(X)", y = "Dimension(Y)")
    + scale_x_continuous(labels = "F0")
    + scale_y_continuous(labels = "F0")
    ;

    plot(ggplot(padding = `padding: ${padding_top}px ${padding_right}px ${padding_bottom}px ${padding_left}px;`) 
        + ggtitle(ionName) 
        + theme(plot.title = element_text(family = "Cambria Math", size = 32))
    );
    plot(bar);
    plot(ion);            

    dev.off();
}