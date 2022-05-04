require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");

bitmap(file = `${@dir}/TrIQ_MSImaging.png`, size = [3300, 2000]) {
    
    # load mzpack/imzML raw data file
    # and config ggplot data source driver 
    # as MSImaging data reader
    ggplot(open.mzpack("E:\\FULL MS_centriod_CHCA_20210819.mzPack"), 
           mapping = aes(driver = MSImaging()), 
           padding = "padding: 200px 600px 200px 250px;"
    ) 
       # rendering of a single ion m/z
       # default color palette is Jet color set
       + geom_msimaging(mz = 772.5206, tolerance = "da:0.1", TrIQ = 0.5)
       + MSI_gaussblur(levels = 5)
       + geom_MSIbackground("black")
       # add ggplot charting elements
       + ggtitle("MSImaging of m/z 772.5206")
       + labs(x = "Dimension(X)", y = "Dimension(Y)")
       + scale_x_continuous(labels = "F0")
       + scale_y_continuous(labels = "F0")
    ;
}