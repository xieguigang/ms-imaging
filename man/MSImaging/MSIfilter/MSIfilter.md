# MSIfilter

Helper function module for create the image filter pipeline

+ [intensity_cut](MSIfilter/intensity_cut.1) removes low intensity spots
+ [log_scale](MSIfilter/log_scale.1) Normalized the raw input intensity value via log(N)
+ [quantile_scale](MSIfilter/quantile_scale.1) 
+ [TrIQ_scale](MSIfilter/TrIQ_scale.1) Trim the raw input intensity value via the TrIQ algorithm
+ [soften_scale](MSIfilter/soften_scale.1) Make convolution of the spatial data for make the imaging render result soften
+ [knn_scale](MSIfilter/knn_scale.1) Trying to fill the missing spatial spot on the imaging via knn method
+ [denoise_scale](MSIfilter/denoise_scale.1) 
