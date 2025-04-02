
#' Apply Default Intensity Filtering Pipeline to MSI Ion Layer
#'
#' @description 
#' Processes an ion layer through a predefined sequence of intensity correction 
#' steps including gap filling, spatial smoothing, and intensity range truncation 
#' using Trimmed Quantile Scaling (TrIQ).
#'
#' @param ion An `ion` object containing MSI data with at least a `layer` component 
#'           (raster data) and `TrIQ` field for storing scaling factors.
#' @param MSI_TrIQ Quantile threshold (0-1) for intensity truncation. Default 0.8 
#'               (retains intensities below 80th percentile).
#'
#' @details 
#' Processing workflow:
#' 1. ​**kNN Imputation**: Fills missing values using \code{knnFill()}
#' 2. ​**Spatial Smoothing**: Applies \code{soften_scale()} for edge-preserving smoothing
#' 3. ​**TrIQ Scaling**: Calculates intensity thresholds with \code{TrIQ()}
#' 4. ​**Intensity Truncation**: Limits layer values using \code{intensityLimits()}
#'
#' The modified `ion` object will contain:
#' - Processed `layer` raster with adjusted intensities
#' - Calculated `TrIQ` values stored in the object
#'
#' @return Modified `ion` object with updated `layer` and `TrIQ` components.
#' 
#' @examples
#' \dontrun{
#' processed_ion <- default_intensity_filter(sample_ion)
#' plot_raster(processed_ion$layer)
#' }
#' 
#' @seealso 
#' \code{\link{custom_intensity_filter}} for user-defined filtering sequences
#' @export
const default_intensity_filter = function(ion, MSI_TrIQ = 0.8) {
    ion$layer = ion$layer |> knnFill() |> soften_scale();
    ion$TrIQ  = TrIQ(ion$layer, q = MSI_TrIQ) * max(intensity(ion$layer));
    ion$layer = intensityLimits(ion$layer, max = ion$TrIQ[2]);
    ion;
}

#' Apply Custom Filter Sequence to MSI Ion Layer
#'
#' @description 
#' Processes an ion layer using user-defined raster filters followed by automatic
#' intensity scaling through Trimmed Quantile Scaling (TrIQ).
#'
#' @param ion An `ion` object containing MSI data with `layer` raster data
#' @param filters List of raster filters to apply in sequence (function objects)
#' @param MSI_TrIQ Quantile threshold for intensity truncation (default 0.8)
#'
#' @details 
#' Key features:
#' - Allows custom filter sequences via \code{apply_raster_filter()}
#' - Maintains automatic TrIQ scaling after custom filtering
#' - Preserves original object structure while updating `layer` and `TrIQ`
#'
#' @return Modified `ion` object with filtered `layer` and updated `TrIQ` values
#' 
#' @examples
#' \dontrun{
#' custom_filters <- list(denoise_scale(), median_filter(3))
#' custom_ion <- custom_intensity_filter(sample_ion, custom_filters)
#' }
#' 
#' @seealso 
#' \code{\link{default_intensity_filter}} for the predefined filtering sequence
#' @export
const custom_intensity_filter = function(ion, filters, MSI_TrIQ = 0.8) {
    ion$layer = apply_raster_filter(filters, ion$layer);
    ion$TrIQ  = TrIQ(ion$layer, q = MSI_TrIQ) * max(intensity(ion$layer));
    ion;
}