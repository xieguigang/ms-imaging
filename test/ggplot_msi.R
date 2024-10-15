require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");

svg(file = `${@dir}/demo.svg`, size = [3000, 2400]) {
    
    # load mzpack/imzML raw data file
    # and config ggplot data source driver 
    # as MSImaging data reader
    ggplot(open.mzpack(system.file("data/S043_Processed_imzML1.1.1.mzPack", package = "MSImaging")), 
           mapping = aes(driver = MSImaging()), 
           padding = "padding: 200px 600px 200px 250px;"
    ) 
       # rendering of a single ion m/z
       # default color palette is Jet color set
       + geom_msimaging(mz = 153.2, tolerance = "da:0.3")
       # add ggplot charting elements
       + ggtitle("MSImaging of m/z 153.2")
       + labs(x = "Dimension(X)", y = "Dimension(Y)")
       + scale_x_continuous(labels = "F0")
       + scale_y_continuous(labels = "F0")
    ;
}