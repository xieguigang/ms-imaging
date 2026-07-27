const ms1_search_ion = function(mz, id, name, formula, kegg_id, pathway, class,
                                ms1ppm = 15, 
                                adducts = c("[M+H]+","[M]+")) {

    as.numeric(mz) |> __ms1_search(data.frame(
        row.names = id,
        name = name,
        formula = formula,
        kegg_id = kegg_id,
        pathway = pathway,
        class = class
    ), ms1ppm = ms1ppm, adducts = adducts)
    ;
}