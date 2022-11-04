require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");
setwd(@dir);

bitmap(file = `./HR2MSI_mouse_urinary_bladder_S096.png`, size = [3300, 2000]) {
    
    # load mzpack/imzML raw data file
    # and config ggplot data source driver 
    # as MSImaging data reader
    ggplot(open.mzpack(system.file("data/HR2MSI mouse urinary bladder S096 - Figure1.cdf", package = "MSImaging")), 
           mapping = aes(driver = MSImaging()), 
           padding = "padding: 200px 600px 200px 250px;"
    ) 
       # rendering of a single ion m/z
       # default color palette is Jet color set
       + geom_msimaging(mz = 741.5, tolerance = "da:0.3")
	   + default_MSIfilter() 
	   + geom_MSIbackground("black")
       # add ggplot charting elements
       + ggtitle("MSImaging of m/z 741.5")
       + labs(x = "Dimension(X)", y = "Dimension(Y)")
       + scale_x_continuous(labels = "F0")
       + scale_y_continuous(labels = "F0")
    ;
}