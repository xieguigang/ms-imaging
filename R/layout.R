require(graphics2D);

#' evaluate of the canvas size and padding
#' 
#' @param dims the dimension data of the MSImaging raw data. 
#'     it should be a numeric vector with at least two element 
#'     standards for ``[w, h]``; or a list object with at 
#'     least two slot elements which are named ``w`` and ``h``.
#' @param scale a numeric factor for describ the pixel scale 
#'     size in MSImaging rendering process. default value is 1 
#'     means no scale, just rendering of the origina size!
#' @param padding  the padding element is a numeric vector with 
#'     four elements standards for css padding value: 
#'     ``[top, right, bottom, left]``. 
#' 
#' @return this function returns a list object that contains 
#'   the recommended size value for the MSImaging plot. the 
#'   result value contains two slot elements: size element is 
#'   a dimension vector with two elements standards for 
#'   ``[w, h]``.
#' 
const autoSize = function(dims, padding, scale = 1) {
    dims    = graphics2D::sizeVector(dims);
    padding = graphics2D::paddingVector(padding); 
    vpad    = padding[1] + padding[3];
    hpad    = padding[2] + padding[4]; 
    dims    = dims * scale;
    dims    = [
        dims[1] + hpad, # width
        dims[2] + vpad  # height
    ];

    dims;
}