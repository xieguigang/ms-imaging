const default_MSIfilter = function() {
	geom_MSIfilters(
		TrIQ_scale(0.8) > knn_scale() > soften_scale()
	);
}