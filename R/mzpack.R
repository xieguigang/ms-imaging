
#' count the scan ms1 number as the number of pixels
#'
#' @param raw the mzpack object
#' 
#' @return the pixel counts(each pixel in the 
#'   MS-imaging 2D plain is a ms1 scan in mzpack
#'   raw data).
#'
const npixels = function(raw) {
	length([raw]::MS);
}