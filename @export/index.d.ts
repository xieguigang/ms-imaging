// export R# source type define for javascript/typescript language
//
// package_source=MSImaging

declare namespace MSImaging {
   module _ {
      /**
        * @param scale default value Is ``1``.
      */
      function auto_size_internal(dims: any, padding: any, scale?: any): object;
      /**
      */
      function onLoad(): object;
   }
   /**
     * @param scale default value Is ``1``.
     * @param is_multiple_combine_wide default value Is ``false``.
     * @param ratio_threshold default value Is ``1.25``.
     * @param ratio_scale default value Is ``1.5``.
   */
   function autoSize(dims: any, padding: any, scale?: any, is_multiple_combine_wide?: any, ratio_threshold?: any, ratio_scale?: any): object;
   /**
   */
   function default_MSIfilter(): object;
   /**
     * @param tolerance default value Is ``Call "getOption"("mzdiff", "default" <- "da:0.3")``.
   */
   function geom_blanket(mz: any, tolerance?: any): object;
   /**
     * @param tolerance default value Is ``Call "getOption"("mzdiff", "default" <- "da:0.3")``.
   */
   function geom_blue(mz: any, tolerance?: any): object;
   /**
     * @param tolerance default value Is ``Call "getOption"("mzdiff", "default" <- "da:0.3")``.
   */
   function geom_green(mz: any, tolerance?: any): object;
   /**
     * @param tolerance default value Is ``Call "getOption"("mzdiff", "default" <- "da:0.3")``.
   */
   function geom_red(mz: any, tolerance?: any): object;
   /**
     * @param savePng default value Is ``./Rplot.png``.
     * @param ionName default value Is ``null``.
     * @param size default value Is ``[2400, 1000]``.
     * @param colorMap default value Is ``null``.
     * @param MSI_colorset default value Is ``viridis:turbo``.
     * @param ggStatPlot default value Is ``null``.
     * @param padding_top default value Is ``150``.
     * @param padding_right default value Is ``200``.
     * @param padding_bottom default value Is ``150``.
     * @param padding_left default value Is ``150``.
     * @param interval default value Is ``50``.
     * @param combine_layout default value Is ``[4, 5]``.
     * @param jitter_size default value Is ``8``.
     * @param TrIQ default value Is ``0.65``.
     * @param backcolor default value Is ``black``.
   */
   function MSI_ionStatPlot(mzpack: any, mz: any, met: any, sampleinfo: any, savePng?: any, ionName?: any, size?: any, colorMap?: any, MSI_colorset?: any, ggStatPlot?: any, padding_top?: any, padding_right?: any, padding_bottom?: any, padding_left?: any, interval?: any, combine_layout?: any, jitter_size?: any, TrIQ?: any, backcolor?: any): object;
   /**
   */
   function npixels(raw: any): object;
   /**
     * @param layout default value Is ``[3, 3]``.
     * @param colorSet default value Is ``Jet``.
     * @param MSI_TrIQ default value Is ``0.8``.
     * @param gaussian default value Is ``3``.
     * @param size default value Is ``[2700, 2000]``.
     * @param canvasPadding default value Is ``[50, 300, 50, 50]``.
     * @param cellPadding default value Is ``[200, 100, 0, 100]``.
     * @param strict default value Is ``true``.
   */
   function PlotMSIMatrixHeatmap(ions_data: any, layout?: any, colorSet?: any, MSI_TrIQ?: any, gaussian?: any, size?: any, canvasPadding?: any, cellPadding?: any, strict?: any): object;
}
