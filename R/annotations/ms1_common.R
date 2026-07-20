imports "metadb" from "mzkit";

const __ms1_search = function(mz, data) {
    let hmdb = read.csv(relative_work("../hmdb.csv"), row.names = 1);
    let i = !has.metal_ion( hmdb$formula);
    let j = is.organic(hmdb$formula,strict =TRUE);

    hmdb = hmdb[i && j,];
    hmdb = hmdb[nchar(hmdb$kegg_id) > 2, ];
    hmdb[,"id"] = rownames(hmdb);

    let index = annotationStream(rownames(hmdb), hmdb$name, hmdb$formula);
    let hmdb_db = ms1_handler(index, precursors = c("[M+H]+"),tolerance= "ppm:15");
    let data = read.csv("expression_matrix.csv", row.names = 1, check.names = FALSE);
    let mz = as.numeric(rownames(data));

    print(hmdb, max.print = 6);
    print(mz);

    let result = hmdb_db |> ms1_search(mz = mz,unique = TRUE);

    result = as.data.frame(result);
    result = result |> left_join( hmdb, by.x = "unique_id", by.y = "id"  );
    result[,"intensity"] = NULL;
    result[,"mz_ref"] = NULL;
    result[,"index"] = NULL;
    result[,"name.1"] = NULL;
    result[,"formula.1"] = NULL;
    result[,"unique_id"] = NULL;

    result[,"ion_id"] = `m/z ${ sprintf("%.4f", round(result$mz,4))    }`;

    print(result);

    write.csv(result, file = "annotations.csv", row.names = FALSE);
}