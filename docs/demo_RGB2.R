# imports "package_utils" from "devkit";

setwd(@dir);

# package_utils::attach("../../mzkit_app");
# package_utils::attach("../../MSI_app"); 

require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");

bitmap(file = `./mouse-embryos_RGB.png`, size = [3000, 3600]) {
    
    # load mzpack/imzML raw data file
    # and config ggplot data source driver 
    # as MSImaging data reader
    ggplot(open.mzpack(system.file("data/mouse-embryos.cdf", package = "MSImaging")),           
           padding = "padding: 200px 600px 200px 250px;"
    ) 
        + MSI_dimension(dims = [256,373])

       # rendering of rgb channels ion m/z
       + geom_red(mz = 508.0025, tolerance = "da:0.3")
       + geom_green(mz = 983.0731, tolerance = "da:0.3")
       + geom_blue(mz = 861.9469, tolerance = "da:0.3")
	   + theme(panel.background = "black", panel.grid = element_blank())
	   + geom_MSIfilters(
            TrIQ_scale(0.999)
       )
       # add ggplot charting elements
       + ggtitle("Mouse Embryos")
       + labs(x = "Dimension(X)", y = "Dimension(Y)")
       + scale_x_continuous(labels = "F0")
       + scale_y_continuous(labels = "F0")
    ;
}