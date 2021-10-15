#' Do MSI rendering for mz as red layer
#' 
#' @param mz the m/z value of the target ion
#' 
#' @return a ggplot layer object for do MS-imaging rendering. 
#' 
const geom_red as function(mz, tolerance = "da:0.3") {
    geom_color(mz, "red", tolerance);
}

#' Do MSI rendering for mz as green layer
#' 
#' @param mz the m/z value of the target ion
#' 
#' @return a ggplot layer object for do MS-imaging rendering. 
#' 
const geom_green as function(mz, tolerance = "da:0.3") {
    geom_color(mz, "green", tolerance);
}

#' Do MSI rendering for mz as blue layer
#' 
#' @param mz the m/z value of the target ion
#' 
#' @return a ggplot layer object for do MS-imaging rendering. 
#' 
const geom_blue as function(mz, tolerance = "da:0.3") {
    geom_color(mz, "blue", tolerance);
}