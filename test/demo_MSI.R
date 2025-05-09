# imports "package_utils" from "devkit";
# imports "ggplot2" from "../assembly/net6.0/ggplot.dll";

setwd(@dir);

# package_utils::attach("../../MSI_app"); 
# package_utils::attach("../../mzkit_app");

require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");

let make_ggplot = function() {
    # load mzpack/imzML raw data file
    # and config ggplot data source driver 
    # as MSImaging data reader
    ggplot(open.mzpack(system.file("data/HR2MSI mouse urinary bladder S096 - Figure1.cdf", package = "MSImaging")), 
            mapping = aes(), 
            padding = "padding: 200px 600px 200px 250px;"
    ) 

    + geom_MSIbackground("black")
    # rendering of a single ion m/z
    # default color palette is Jet color set
    + geom_msimaging(mz = 741.5303, tolerance = "da:0.3", color = "fleximaging")
    + default_MSIfilter() 	   
    + geom_MSIruler()
    # add ggplot charting elements
    + ggtitle("MSImaging of m/z 741.5303")
    + labs(x = "Dimension(X)", y = "Dimension(Y)")
    + scale_x_continuous(labels = "F0")
    + scale_y_continuous(labels = "F0")
    + theme(     
        panel.grid.major = element_blank(),
        panel.grid.minor = element_blank(),
        panel.grid = element_blank(),
        plot.background = "white"
    )
    ;
}

# svg(file = "./HR2MSI_mouse_urinary_bladder_S096.svg", size = [3300, 2000]) {
#     make_ggplot();
# }

pdf(file = "./HR2MSI_mouse_urinary_bladder_S096.pdf", size = [3300, 2000]) {
    make_ggplot();
}