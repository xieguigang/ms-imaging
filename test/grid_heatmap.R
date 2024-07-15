require(MSImaging);
require(mzkit);
require(ggplot);

imports "MsImaging" from "mzplot";

options(memory.load = "max");

let msi_rawdata = system.file("data/HR2MSI mouse urinary bladder S096 - Figure1.cdf", package = "MSImaging");
let rgb_rawdata = open.mzpack(msi_rawdata) |> MsImaging::viewer();
let rgb_ions = list(
    # 1. type   precursor type information string
    # 2. title  the ion metabolite name
    # 3. layer  the MSI ion layer data
    # 4. mz     the target ion m/z value
    list(type = "[M+H]+", title = "metabolite 1", mz =  743.5468, layer = MSIlayer(rgb_rawdata, 743.5468)),
    list(type = "[M+H]+", title = "metabolite 2", mz =  798.5414, layer = MSIlayer(rgb_rawdata, 798.5414)),
    list(type = "[M+H]+", title = "metabolite 3", mz =  741.5303, layer = MSIlayer(rgb_rawdata, 741.5303))
);

bitmap(file = file.path(@dir, "rgb_3x1.png"));
    PlotMSIMatrixHeatmap(rgb_ions, 
        layout        = [3,1],
        colorSet      = "Jet",
        MSI_TrIQ      = 0.8,
        gaussian      = 3,
        size          = [6700, 2000], 
        canvasPadding = [50, 300, 50, 50], 
        cellPadding   = [200, 100, 0, 100], 
        font_size     = 27,
        filters       = denoise_scale() + TrIQ_scale(0.85) + knn_scale(1,1.2, random = TRUE) + soften_scale(),
        msi_dimension = [300,300],
        strict        = TRUE);
dev.off();