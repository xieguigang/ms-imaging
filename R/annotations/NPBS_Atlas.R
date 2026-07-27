const NPBS_Atlas = function(mz, ms1ppm = 15, adducts = c("[M+H]+","[M]+")) {
    let data = read.csv(`${@datadir}/NPBS_Atlas_Data.csv`, row.names = 1, check.names = FALSE);
    let result = __ms1_search(mz, data.frame(
        row.names = rownames(data),
        name = data[,"Compound name"],
        formula = data[,"Molecular formula"],
        kegg_id = "n/a",
        pathway = data[,"Pathway"],
        class = data[,"Class"]
    ), ms1ppm = ms1ppm, adducts = adducts);

    return(result);
}