#' Do MSI rendering for mz as red layer
#' 
#' @param mz the m/z value of the target ion
#' 
#' @return a ggplot layer object for do MS-imaging rendering. 
#' 
const geom_red as function(mz) {
    geom_color(mz, "red");
}

#' Do MSI rendering for mz as green layer
#' 
#' @param mz the m/z value of the target ion
#' 
#' @return a ggplot layer object for do MS-imaging rendering. 
#' 
const geom_green as function(mz) {
    geom_color(mz, "green");
}

#' Do MSI rendering for mz as blue layer
#' 
#' @param mz the m/z value of the target ion
#' 
#' @return a ggplot layer object for do MS-imaging rendering. 
#' 
const geom_blue as function(mz) {
    geom_color(mz, "blue");
}