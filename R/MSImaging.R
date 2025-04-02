#' Create red channel MS imaging layer
#' 
#' Generates a ggplot2 layer for mass spectrometry imaging (MSI) data visualization 
#' in red color channel corresponding to specified m/z value.
#' 
#' @param mz numeric. The target mass-to-charge ratio (m/z) value for ion detection.
#' @param tolerance character or numeric. Mass accuracy tolerance in Dalton (Da) format.
#'        Default uses option "mzdiff" if set, otherwise "da:0.3". Format should follow
#'        "da:[value]" for Dalton-based tolerance or "ppm:[value]" for parts-per-million.
#'        
#' @return A ggplot2 layer object that can be added to existing MSI plots for 
#'         multi-channel visualization. Displays ion distribution in red color scale.
#' 
#' @examples
#' \dontrun{
#' plot_base() + geom_red(843.521)
#' }
const geom_red = function(mz, tolerance = getOption("mzdiff", default = "da:0.3")) {
    geom_color(mz, "red", tolerance);
}

#' Create green channel MS imaging layer
#' 
#' Generates a ggplot2 layer for mass spectrometry imaging (MSI) data visualization
#' in green color channel corresponding to specified m/z value.
#' 
#' @param mz numeric. The target mass-to-charge ratio (m/z) value for ion detection.
#' @param tolerance character or numeric. Mass accuracy tolerance. See geom_red() for format details.
#' 
#' @return A ggplot2 layer object displaying ion distribution in green color scale.
#' 
#' @examples
#' \dontrun{
#' plot_base() + geom_green(556.276)
#' }
const geom_green = function(mz, tolerance = getOption("mzdiff", default = "da:0.3")) {
    geom_color(mz, "green", tolerance);
}

#' Create blue channel MS imaging layer
#' 
#' Generates a ggplot2 layer for mass spectrometry imaging (MSI) data visualization
#' in blue color channel corresponding to specified m/z value.
#' 
#' @param mz numeric. The target mass-to-charge ratio (m/z) value for ion detection.
#' @param tolerance character or numeric. Mass accuracy tolerance. See geom_red() for format details.
#' 
#' @return A ggplot2 layer object displaying ion distribution in blue color scale.
#' 
#' @examples
#' \dontrun{
#' plot_base() + geom_blue(1024.673)
#' }
const geom_blue = function(mz, tolerance = getOption("mzdiff", default = "da:0.3")) {
    geom_color(mz, "blue", tolerance);
}

#' Create monochromatic overlay for MS imaging
#' 
#' Generates a grayscale ggplot2 layer for mass spectrometry imaging (MSI) data,
#' typically used as a background layer or for intensity normalization.
#' 
#' @param mz numeric. The target mass-to-charge ratio (m/z) value for ion detection.
#' @param tolerance character or numeric. Mass accuracy tolerance. See geom_red() for format details.
#' 
#' @return A ggplot2 layer object using "Greys" color palette with alpha channel (c8 = 200/255 transparency)
#'         suitable for creating intensity-normalized backgrounds or composite overlays.
#' 
#' @examples
#' \dontrun{
#' plot_base() + geom_blanket(1200.834)
#' }
const geom_blanket = function(mz, tolerance = getOption("mzdiff", default = "da:0.3")) {
    geom_msimaging(mz, tolerance, color = "Greys:c8");
}