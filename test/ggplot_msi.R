require(MSImaging);
require(mzkit);

options(memory.load = "max");

bitmap(file = `${@dir}/demo.png`, size = [3000, 2400]) {
    ggplot(open.mzpack(system.file("data/S043_Processed_imzML1.1.1.mzPack", package = "MSImaging")), 
           mapping = aes(driver = MSImaging()), 
           padding = "padding: 200px 600px 200px 250px;"
    ) 
       + geom_msimaging(mz = 153.2, tolerance = "da:0.3")
       + ggtitle("MSImaging of m/z 153.2")
       + labs(x = "Dimension(X)", y = "Dimension(Y)")
       + scale_x_continuous(labels = "F0")
       + scale_y_continuous(labels = "F0")
    ;
}