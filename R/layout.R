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
#' @param is_multiple_combine_wide the function will trying to
#'     make the aspect ratio equals to 1:1 if this parameter value
#'     is set to false by default.
#' @param ratio_scale the scale of the ratio value should not be 1,
#'     due to the reason of ``1`` means no changes.
#' 
#' @return this function returns a list object that contains 
#'   the recommended size value for the MSImaging plot. the 
#'   result value contains two slot elements: size element is 
#'   a dimension vector with two elements standards for 
#'   ``[w, h]``.
#' 
const autoSize = function(dims, padding,
                          scale = 1, 
                          is_multiple_combine_wide = FALSE, 
                          ratio_threshold = 1.3,
                          ratio_scale = 1.5) {

    dims    = graphics2D::sizeVector(dims);
    padding = graphics2D::paddingVector(padding); 
    scale   = .auto_size_internal(dims, padding, scale);

    if (!is_multiple_combine_wide) {
        # try to make the w/h ratio 1:1
        let ratio as double = .Internal::log(scale[1]/scale[2], 2);
        let threshold as double = ratio_threshold;

        print("auto layout for non-multiple sample:");
        print("the original layout size:");
        str(scale);
        print("log ratio value:");
        str(ratio);
        print(`test of ratio(${ratio}) > threshold(${threshold}):`);
        str(ratio > threshold);        

        if (ratio > threshold) {
            # is w >> h
            # keeps the height
            # and scale width
            scale = [
                scale[2] * ratio_scale,
                scale[2]
            ];
        } else {
            if (ratio < [-threshold]) {
                # is h >> w
                # keeps the width
                # and scale the height
                scale = [
                    scale[1],
                    scale[1] * ratio_scale
                ];
            } else {
                # the ratio is nearly 1:1
                # do nothing
            }
        }
    }

    scale;
}

#' Evaluate the canvas size
#' 
#' @return A size numeric vector with two elements: w,h
#' 
const .auto_size_internal = function(dims, padding, scale = 1) {
    const vpad = padding[1] + padding[3];
    const hpad = padding[2] + padding[4]; 

    dims = dims * scale;
    dims = [
        dims[1] + hpad, # width
        dims[2] + vpad  # height
    ];

    dims;
}