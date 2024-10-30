require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");

setwd(@dir);

let ions = read.csv( "./ions.xls", tsv = TRUE, row.names = NULL, check.names=FALSE);
let msi_data = open.mzpack("F:\\raw.mzPack");

print(ions, max.print = 6);

for(let mz in ions$mz) {
    pdf(file = `F:/All/${mz}.pdf`, size = [3000, 2700]) {
        
        # load mzpack/imzML raw data file
        # and config ggplot data source driver 
        # as MSImaging data reader
        ggplot(msi_data, padding = "padding: 200px 600px 200px 250px;") 
        # rendering of a single ion m/z
        # default color palette is Jet color set
        + geom_msimaging(mz = mz, tolerance = "da:0.3")
        + geom_MSIbackground("TIC")
        + geom_MSIfilters([
            "denoise(0.01)"
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
}


