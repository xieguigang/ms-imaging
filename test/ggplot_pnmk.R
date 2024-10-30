require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");

setwd(@dir);

# let ions = read.csv( "./ions.xls", tsv = TRUE, row.names = NULL, check.names=FALSE);
let msi_data = open.mzpack("F:\\PANOMIX\\RAW.mzPack");
let mz = 541.2068;
let make_plot = function() {
    # load mzpack/imzML raw data file
    # and config ggplot data source driver 
    # as MSImaging data reader
    ggplot(msi_data, padding = "padding: 200px 600px 200px 250px;") 
    + geom_MSIbackground("TIC")
    # rendering of a single ion m/z
    # default color palette is Jet color set
    + geom_msimaging(mz = mz, tolerance = "da:0.01")        
    + geom_MSIfilters([            
        "knn_fill(3,0.65,random=false)"
        "soften()"
        "power(2)"
    ])
    + MSI_hqx(1)
    # add ggplot charting elements
    + ggtitle(`MSImaging of m/z ${mz}`)
    + labs(x = "Dimension(X)", y = "Dimension(Y)")
    + scale_x_continuous(labels = "F0")
    + scale_y_continuous(labels = "F0")
    ;
}

pdf(file = file.path(@dir, "PNMK.pdf"), size = [4200, 2100], dpi = 300) {
    make_plot() 
}

bitmap(file = file.path(@dir, "PNMK.jpeg"), size = [4200, 2100], dpi = 300) {
    make_plot() 
}

svg(file = file.path(@dir, "PNMK.svg"), size = [4200, 2100], dpi = 300) {
    make_plot() 
}
