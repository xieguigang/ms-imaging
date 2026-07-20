imports "metadb" from "mzkit";

const __ms1_search = function(mz, data) {
    let i = !has.metal_ion(data$formula);
    let j = is.organic(data$formula,strict =TRUE);

    data = data[i && j,];
    data = data[nchar(data$kegg_id) > 2, ];
    data[,"id"] = rownames(data);

    let index = annotationStream(rownames(data), data$name, data$formula);
    let db = ms1_handler(index, precursors = c("[M+H]+"),tolerance= "ppm:15");
    
    print(data, max.print = 6);
    print(mz);

    let result = db |> ms1_search(mz = mz,unique = TRUE);

    result = as.data.frame(result);
    result = result |> left_join( data, by.x = "unique_id", by.y = "id"  );
    result[,"intensity"] = NULL;
    result[,"mz_ref"] = NULL;
    result[,"index"] = NULL;
    result[,"name.1"] = NULL;
    result[,"formula.1"] = NULL;
    result[,"unique_id"] = NULL;

    result[,"ion_id"] = `m/z ${ sprintf("%.4f", round(result$mz,4))    }`;
    result;
}