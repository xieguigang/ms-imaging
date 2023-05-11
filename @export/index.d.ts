// export R# source type define for javascript/typescript language
//
// package_source=MSImaging

declare namespace MSImaging {
   module _ {
      /**
        * @param scale default value Is ``1``.
      */
      function auto_size_internal(dims: any, padding: any, scale?: object): object;
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
   function autoSize(dims: any, padding: any, scale?: object, is_multiple_combine_wide?: boolean, ratio_threshold?: number, ratio_scale?: number): object;
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
   function MSI_ionStatPlot(mzpack: any, mz: any, met: any, sampleinfo: any, savePng?: string, ionName?: any, size?: any, colorMap?: any, MSI_colorset?: string, ggStatPlot?: any, padding_top?: object, padding_right?: object, padding_bottom?: object, padding_left?: object, interval?: object, combine_layout?: any, jitter_size?: object, TrIQ?: number, backcolor?: string): object;
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
   function PlotMSIMatrixHeatmap(ions_data: any, layout?: any, colorSet?: string, MSI_TrIQ?: number, gaussian?: object, size?: any, canvasPadding?: any, cellPadding?: any, strict?: boolean): object;
}
