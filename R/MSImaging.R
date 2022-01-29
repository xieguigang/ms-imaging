#' Do MSI rendering for mz as red layer
#' 
#' @param mz the m/z value of the target ion
#' 
#' @return a ggplot layer object for do MS-imaging rendering. 
#' 
const geom_red = function(mz, tolerance = getOption("mzdiff", default = "da:0.3")) {
    geom_color(mz, "red", tolerance);
}

#' Do MSI rendering for mz as green layer
#' 
#' @param mz the m/z value of the target ion
#' 
#' @return a ggplot layer object for do MS-imaging rendering. 
#' 
const geom_green = function(mz, tolerance = getOption("mzdiff", default = "da:0.3")) {
    geom_color(mz, "green", tolerance);
}

#' Do MSI rendering for mz as blue layer
#' 
#' @param mz the m/z value of the target ion
#' 
#' @return a ggplot layer object for do MS-imaging rendering. 
#' 
const geom_blue = function(mz, tolerance = getOption("mzdiff", default = "da:0.3")) {
    geom_color(mz, "blue", tolerance);
}

const geom_blanket = function(mz, tolerance = getOption("mzdiff", default = "da:0.3")) {
    geom_msimaging(mz, tolerance, color = "Greys:c8");
}