#' Create raster image based on the total ions thumbnail of the sample rawdata file.
#' 
#' @param rawdata should be the mzkit mzpack data object
#' @param filters the raster filters for the ms-imaging spatial pixels rawdata
#' 
#' @return a gdi+ raster image data object for make single ion ms-imaging 
#'    rendering as background.
#' 
const MSI_sampleTIC = function(rawdata, dims = NULL, filters = NULL) {
    let tic = MSI::MSI_summary(rawdata, as_vector = TRUE);
    let dim_size = {
        if (is.null(dims)) {
            dimension_size(rawdata);
        } else {
            dims;
        }
    };
    let raster = tic |> raster_blending(dims = dim_size,
                                       scale = "gray",
                                      levels = 60,
                                     filters = filters
    ); 
    # returns the raster image data 
    return(raster);
}