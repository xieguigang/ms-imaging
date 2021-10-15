# require(MSImaging);

require(mzkit);

options(memory.load = "max");

const raw = open.mzpack(system.file("data/S043_Processed_imzML1.1.1.mzPack", package = "MSImaging"));

bitmap(file = `${@dir}/demo.png`) {

    ggplot(raw, aes(driver = MSImaging())) 
    + geom_msimaging(mz = 153.2, tolerance = "da:0.3")
    ;

}