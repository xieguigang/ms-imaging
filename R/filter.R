const default_MSIfilter = function() {
	geom_MSIfilters(log_scale() > TrIQ_scale() > knn_scale() |> soften_scale());
}