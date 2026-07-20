const NPBS_Atlas = function(mz) {
    let data = read.csv(`${@datadir}/NPBS_Atlas_Data.csv`, row.names = 1, check.names = FALSE);
    let result = __ms1_search(mz, data.frame(
        row.names = rownames(data),
        name = data[,"Compound name"],
        formula = data[,"Molecular formula"],
        pathway = data[,"Pathway"],
        class = data[,"Class"]
    ));

    return(result);
}