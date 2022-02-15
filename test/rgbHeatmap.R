require(MSImaging);
require(mzkit);
require(ggplot);

options(memory.load = "max");

datadir = "D:\biodeep\biodeepdb_v3\spatial\MSI_analysis\test\HE_mapping";

data = `${datadir}/UMAP2D_phenograph.csv`;
data = read.csv(data, row.names = 1, check.names = FALSE);

print(data, max.print = 13);

bitmap(file = `${datadir}/UMAP_RGB.png`, size = [3300, 2000]) {
    
    # load mzpack/imzML raw data file
    # and config ggplot data source driver 
    # as MSImaging data reader
    ggplot(MSIheatmap(data, R = "x", G = "y", B = "z"),           
           padding = "padding: 200px 600px 200px 250px;"
    )        

       # plot heatmap
       + geom_msiheatmap()
	   + theme(panel.background = "black")
	   + MSI_knnfill()
       
       # add ggplot charting elements
       + ggtitle("UMAP RGB")
       + labs(x = "Dimension(X)", y = "Dimension(Y)")
       + scale_x_continuous(labels = "F0")
       + scale_y_continuous(labels = "F0")
    ;
}