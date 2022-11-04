#' optmise for single ion ms-imaging
#'
const default_MSIfilter = function() {
	geom_MSIfilters(
		denoise_scale() > TrIQ_scale(0.8) > knn_scale() > soften_scale()
	);
}