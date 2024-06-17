# imports "package_utils" from "devkit";

setwd(@dir);

# package_utils::attach("../../mzkit_app");
# package_utils::attach("../../MSI_app"); 

require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");

let region_spots = "../data/HR2MSI mouse urinary bladder_outline.csv";
let msi_rawdata = system.file("data/HR2MSI mouse urinary bladder S096 - Figure1.cdf", package = "MSImaging");

region_spots <- read.csv(region_spots, row.names = NULL);

print(region_spots, max.print = 6);

bitmap(file = `./HR2MSI_mouse_urinary_bladder_S096_RGB.png`, size = [3300, 2000]) {
    
    # load mzpack/imzML raw data file
    # and config ggplot data source driver 
    # as MSImaging data reader
    ggplot(open.mzpack(msi_rawdata),           
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
       + geom_sample_outline(spots = region_spots,
            threshold = 0,
            scale = 5,
            degree = 20,
            resolution = 1000,
            q = 0.1,
            line_stroke = "stroke: white; stroke-width: 9px; stroke-dash: dash;")
       + geom_sample_outline(
            threshold = 0,
            scale = 5,
            degree = 20,
            resolution = 1000,
            q = 0.1,
            line_stroke = "stroke: yellow; stroke-width: 9px; stroke-dash: solid;")
       # add ggplot charting elements
       + ggtitle("HR2MSI mouse urinary bladder S096 - Figure1")
       + labs(x = "Dimension(X)", y = "Dimension(Y)")
       + scale_x_continuous(labels = "F0")
       + scale_y_continuous(labels = "F0")
    ;
}