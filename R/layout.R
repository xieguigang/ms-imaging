require(graphics2D);

#' Calculate recommended canvas size for MSImaging visualization
#' 
#' Automatically evaluates appropriate canvas dimensions considering padding, scaling, 
#' and aspect ratio adjustments for multi-sample imaging displays.
#'
#' @param dims The dimensions of MSImaging raw data. Can be either:
#'     1. A numeric vector with at least two elements representing [width, height]
#'     2. A list object containing at least two named elements "w" and "h".
#' @param padding A numeric vector specifying CSS-style padding in the order: 
#'     [top, right, bottom, left]. Defines empty space around the imaging data.
#' @param scale Numeric scaling factor for pixel dimensions (default: 1). 
#'     Values >1 enlarge, <1 shrink while maintaining original proportions.
#' @param is_multiple_combine_wide Logical indicating if wide-format multi-sample 
#'     layout should be prioritized (default: FALSE). When FALSE, attempts to 
#'     maintain 1:1 aspect ratio by applying ratio scaling when needed.
#' @param ratio_threshold Numeric threshold for aspect ratio adjustment 
#'     (default: 1.25). Triggers dimension scaling when width/height ratio 
#'     exceeds this value.
#' @param ratio_scale Multiplicative factor (default: 1.5) applied to 
#'     under-dimensioned axis when aspect ratio exceeds threshold.
#'
#' @return A list containing:
#' \itemize{
#'   \item size - Numeric vector [width, height] of recommended canvas dimensions
#'   \item scale - Applied scaling factors (if any)
#' }
#' 
#' @examples
#' # Basic usage
#' autoSize(c(1024, 768), padding = c(20, 15, 20, 15))
const autoSize = function(dims, padding,
                          scale = 1, 
                          is_multiple_combine_wide = FALSE, 
                          ratio_threshold = 1.25,
                          ratio_scale = 1.5) {

    dims    = graphics2D::sizeVector(dims);
    padding = graphics2D::paddingVector(padding); 
    scale   = .auto_size_internal(dims, padding, scale);

    if (!is_multiple_combine_wide) {
        # try to make the w/h ratio 1:1
        let ratioW as double = .Internal::log(scale[1]/scale[2], 2);
        let ratioH as double = .Internal::log(scale[2]/scale[1], 2);
        let threshold as double = ratio_threshold;

        print("auto layout for non-multiple sample:");
        print("the original layout size:");
        str(scale);
        print("log ratio value:");
        str([ratioW, ratioH]);
        print(`test of ratio(${ratioW}) > threshold(${threshold}) OR ratio(${ratioH}) > [threshold](${threshold}):`);
        str([ratioW > threshold, ratioH > [threshold]]);        

        if (ratioW > threshold) {
            # is w >> h
            # keeps the width
            # and up scale height by factor
            scale = [
                scale[1],
                scale[2] * ratio_scale
            ];
        } else {
            if (ratioH > [threshold]) {
                # is h >> w
                # keeps the height
                # and up scale the width
                scale = [
                    scale[1] * ratio_scale,
                    scale[2] 
                ];
            } else {
                # the ratio is nearly 1:1
                # do nothing
            }
        }

        print("layout scale after the scaling:");
        str(scale);
    }

    scale;
}

