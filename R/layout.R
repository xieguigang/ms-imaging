
#' evaluate of the canvas size and padding
#' 
#' @param dims the dimension data of the MSImaging raw data. 
#'     it should be a numeric vector with at least two element 
#'     standards for ``[w, h]``; or a list object with at 
#'     least two slot elements which are named ``w`` and ``h``.
#' @param scale a numeric factor for describ the pixel scale 
#'     size in MSImaging rendering process. default value is 1 
#'     means no scale, just rendering of the origina size!
#' 
#' @return this function returns a list object that contains 
#'   the recommended size value and canvas padding value for the 
#'   MSImaging plot. the result value contains two slot elements:
#'    
#'    1. size     size element is a dimension vector with two 
#'                elements standards for ``[w, h]``.
#'    2. padding  the padding element is a numeric vector with 
#'                four elements standards for css padding value: 
#'                ``[top, right, bottom, left]``. 
#' 
const autoSize as function(dims, scale = 1) {

}