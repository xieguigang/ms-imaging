# require(MSImaging);

require(mzkit);

const raw = read.mzpack(system.file("data/S043_Processed_imzML1.1.1.mzPack", package = "MSImaging"));

bitmap(file = `${@dir}/demo.png`) {

    ggplot(raw) 
    + geom_msimaging(mz = 153.2, tolerance = "da:0.3")
    ;

}