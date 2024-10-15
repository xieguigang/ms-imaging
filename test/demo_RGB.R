# imports "package_utils" from "devkit";

setwd(@dir);

# package_utils::attach("../../mzkit_app");
# package_utils::attach("../../MSI_app"); 

require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");

let make_rgb_ggplot = function() {
    # load mzpack/imzML raw data file
    # and config ggplot data source driver 
    # as MSImaging data reader
    ggplot(open.mzpack(system.file("data/HR2MSI mouse urinary bladder S096 - Figure1.cdf", package = "MSImaging")),           
           padding = "padding: 200px 600px 200px 250px;"
    ) 
       # rendering of rgb channels ion m/z
       + geom_red(mz = 743.5468, tolerance = "da:0.3")
       + geom_green(mz = 798.5414, tolerance = "da:0.3")
       + geom_blue(mz = 741.5303, tolerance = "da:0.3")
	   + theme(panel.background = "black", panel.grid = element_blank())
	   + geom_MSIfilters(
            TrIQ_scale(0.999)
       )
       # add ggplot charting elements
       + ggtitle("HR2MSI mouse urinary bladder S096 - Figure1")
       + labs(x = "Dimension(X)", y = "Dimension(Y)")
       + scale_x_continuous(labels = "F0")
       + scale_y_continuous(labels = "F0")
    ;
}


png(filename = `./HR2MSI_mouse_urinary_bladder_S096_RGB.png`, size = [3300, 2000], dpi = 300) {
    make_rgb_ggplot();
}

svg(file = `./HR2MSI_mouse_urinary_bladder_S096_RGB.svg`, size = [3300, 2000], dpi = 300) {
    make_rgb_ggplot();
}

pdf(file = `./HR2MSI_mouse_urinary_bladder_S096_RGB.pdf`, size = [3300, 2000], dpi = 300) {
    make_rgb_ggplot();
}