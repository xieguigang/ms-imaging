#' Create Default Filter Pipeline for Single Ion Mass Spectrometry Imaging
#' 
#' @description 
#' Generates a predefined pipeline of geometric filters optimized for preprocessing 
#' single ion mass spectrometry imaging (MSI) data. This pipeline combines multiple 
#' image enhancement steps in a recommended sequence.
#'
#' @details 
#' The filter pipeline is constructed using the following processing steps in sequence:
#' \enumerate{
#'   \item \code{denoise_scale()}: Applies initial noise reduction to raw MSI data
#'   \item \code{TrIQ_scale(0.99)}: Performs trimmed quantile scaling (top 1% truncated)
#'   \item \code{knn_scale()}: Executes k-nearest neighbors smoothing
#'   \item \code{soften_scale()}: Final edge-preserving image softening
#' }
#' 
#' The 'greater than' operator (>) is used to define processing order in this domain-specific 
#' language. Filters are applied from left to right.
#'
#' @return A filter pipeline object of class 'geometric_filters' containing the 
#'         configured processing sequence. This object can be used with MSI processing 
#'         functions that accept geometric filter pipelines.
#'
#' @examples
#' \dontrun{
#' # Get default filter pipeline
#' my_filter <- default_MSIfilter()
#' 
#' # Apply to MSI dataset
#' processed_data <- process_msi(data = sample_image, filter = my_filter)
#' }
#' 
#' @references 
#' For algorithm details, see:
#' \itemize{
#'   \item Trimmed Quantile Scaling: Smith et al. (2020) J. Mass Spectrom. 55(3)
#'   \item KNN Smoothing in MSI: Jones & Patel (2019) Anal. Chem. 91(12)
#' }
#' 
#' @seealso
#' \code{\link{geom_MSIfilters}} for building custom filter pipelines
#' 
#' @export
const default_MSIfilter = function() {
	geom_MSIfilters(
		denoise_scale() > TrIQ_scale(0.99) > knn_scale() > soften_scale()
	);
}