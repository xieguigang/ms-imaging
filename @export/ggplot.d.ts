﻿// export R# package module type define for javascript/typescript language
//
//    imports "ggplot" from "MSImaging";
//
// ref=ggplotMSImaging.Rscript@MSImaging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * the ggplot api plugin for do MS-Imaging rendering
 * 
 * > @``T:ggplotMSImaging.ggplotMSI`` is the ms-imaging render.
*/
declare namespace ggplot {
   module as {
      /**
       * create a pixel point pack object for create ggplot
       * 
       * 
        * @param env 
        * + default value Is ``null``.
      */
      function pixelPack(layer: object, env?: object): object;
   }
   /**
     * @param tolerance default value Is ``'da:0.1'``.
     * @param env default value Is ``null``.
   */
   function geom_cmyk(c: any, m: any, y: any, k: any, tolerance?: any, env?: object): object;
   /**
    * Draw a ion m/z layer with a specific color channel
    * 
    * 
     * @param mz -
     * @param color this parameter usually be used for the r/g/b triple layer overlaps
     * @param tolerance -
     * 
     * + default value Is ``'da:0.1'``.
     * @param pixel_render -
     * 
     * + default value Is ``false``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function geom_color(mz: number, color: any, tolerance?: any, pixel_render?: boolean, env?: object): object|object;
   /**
    * config of the background of the MS-imaging charting plot.
    * 
    * 
     * @param background the background color value or character vector ``TIC`` or ``BPC``.
     * @return this function returns clr object in types based on the **`background`** parameter:
     *  
     *  1. "TIC" or "BPC": @``T:ggplotMSImaging.layers.MSITICOverlap``
     *  2. html color code, or supported gdi image its file path: @``T:ggplotMSImaging.MSIBackgroundOption``
   */
   function geom_MSIbackground(background: any): object|object;
   /**
    * Options for apply the filter pieline on the imaging outputs
    * 
    * 
     * @param filters -
     * 
     * + default value Is ``null``.
     * @param file this function also could read the filter pipeline file for construct the raster pipeline
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function geom_MSIfilters(filters?: any, file?: any, env?: object): object;
   /**
    * create a new ms-imaging heatmap layer
    * 
    * 
     * @param layer value of this parameter can be:
     *  
     *  1. nothing: means rgb heatmap layer
     *  2. Total: means TIC
     *  3. BasePeak: means BPC
     *  4. Average: means average ions
     * 
     * + default value Is ``null``.
     * @param colors the color scaler name for the heatmap rendering, this 
     *  parameter only works when the **`layer`** 
     *  parameter value is not nothing.
     * 
     * + default value Is ``'viridis:turbo'``.
     * @param env 
     * + default value Is ``null``.
   */
   function geom_msiheatmap(layer?: object, colors?: any, env?: object): object;
   /**
    * Do ms-imaging based on a set of given metabolite ions m/z
    * 
    * 
     * @param mz A set of target ion m/z values for do imaging
     * @param tolerance the mass tolerance error vaue for load intensity 
     *  data for each pixels from the raw data files.
     * 
     * + default value Is ``'da:0.1'``.
     * @param pixel_render -
     * 
     * + default value Is ``false``.
     * @param TrIQ the intensity cutoff threshold value for the target 
     *  ion layer use TrIQ algorithm.
     * 
     * + default value Is ``0.99``.
     * @param color the color set name
     * 
     * + default value Is ``'viridis:turbo'``.
     * @param knnFill -
     * 
     * + default value Is ``true``.
     * @param colorLevels 
     * + default value Is ``120``.
     * @param raster the raster annotation image to overlaps, this parameter works when
     *  the **`pixel_render`** is set to value TRUE.
     * 
     * + default value Is ``null``.
     * @param clamp the custom intensity range for make ion layer ms-imaging intensity clamp operation.
     *  value of this parameter should be a numeric vector with [min,max] of the intensity range
     *  for make numeric value clamp before the heatmap rendering.
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function geom_msimaging(mz: number, tolerance?: any, pixel_render?: boolean, TrIQ?: number, color?: any, knnFill?: boolean, colorLevels?: object, raster?: any, clamp?: any, env?: object): object|object;
   /**
    * Draw ruler overlaps of the ms-imaging
    * 
    * 
     * @param color -
     * 
     * + default value Is ``'white'``.
     * @param width the ruler width on the imaging plot, unit of this parameter value is ``um``.
     * 
     * + default value Is ``null``.
   */
   function geom_MSIruler(color?: any, width?: object): object;
   /**
    * Create a plot layer of outline for the sample data
    * 
    * 
     * @param region The region data for the sample region outline drawing, data value should be a scibasic.net @``T:Microsoft.VisualBasic.Imaging.Drawing2D.Math2D.MarchingSquares.GeneralPath``
     *  path data object, or a dataframe object that contains the data fields of ``x`` and ``y`` for store the region 
     *  spot data and the outline will be computed from this spot data points collection.
     * 
     * + default value Is ``null``.
     * @param threshold the intensity threshold for make spatial spot binariation, for clean sample,
     *  leaves this parameter default zero, for sample with background, set this 
     *  parameter in range (0,1) for make spot cutoff.
     * 
     * + default value Is ``0``.
     * @param scale contour tracing pixel rectangle scale size
     * 
     * + default value Is ``5``.
     * @param degree -
     * 
     * + default value Is ``20``.
     * @param resolution -
     * 
     * + default value Is ``1000``.
     * @param q -
     * 
     * + default value Is ``0.1``.
     * @param line_stroke a @``T:ggplot.elements.lineElement`` that create via the ggplot function: ``element_line``.
     * 
     * + default value Is ``'stroke: white; stroke-width: 6px; stroke-dash: solid;'``.
     * @param env 
     * + default value Is ``null``.
   */
   function geom_sample_outline(region?: any, threshold?: number, scale?: object, degree?: number, resolution?: object, q?: number, line_stroke?: any, env?: object): object;
   /**
    * options for config the canvas dimension size of the ms-imaging raw data scans
    * 
    * 
     * @param dims -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function MSI_dimension(dims: any, env?: object): object;
   /**
    * options for gauss filter of the MS-imaging buffer
    * 
    * > the gauss blur function is not working well on the linux platform
    * 
     * @param levels -
     * 
     * + default value Is ``30``.
   */
   function MSI_gaussblur(levels?: object): object;
   /**
     * @param hqx default value Is ``[1,2,3,4]``.
     * @param env default value Is ``null``.
   */
   function MSI_hqx(hqx?: any, env?: object): object;
   /**
    * configs the parameters for do Knn fill of the pixels
    * 
    * 
     * @param k -
     * 
     * + default value Is ``3``.
     * @param qcut the query block area percentage threshold value, 
     *  the higher cutoff of this parameter, the less fitting 
     *  will be perfermen on the pixels, the lower cutoff of 
     *  this parameter, the more interpolation will be.
     * 
     * + default value Is ``0.85``.
   */
   function MSI_knnfill(k?: object, qcut?: number): object;
   /**
    * create R,G,B layers from the given dataframe columns data
    * 
    * 
     * @param R -
     * @param G -
     * 
     * + default value Is ``null``.
     * @param B -
     * 
     * + default value Is ``null``.
     * @param matrix -
     * 
     * + default value Is ``null``.
     * @param dims 
     * + default value Is ``'0,0'``.
     * @param env -
     * 
     * + default value Is ``null``.
     * @return this function generate the data source object for the ggplot
   */
   function MSIheatmap(R: any, G?: any, B?: any, matrix?: object, dims?: any, env?: object): object;
   /**
    * create a pixel point pack object for create ggplot
    * 
    * 
     * @param pixels A pixel point vector for create a data pack
     * @param dims 
     * + default value Is ``[0,0]``.
     * @param env 
     * + default value Is ``null``.
   */
   function pixelPack(pixels: object, dims?: any, env?: object): object;
   /**
    * rendering a gdi+ heatmap for create raster annotation in ggplot layer
    * 
    * 
     * @param pixels the pixels data
     * @param dims the spatial dimension size of the sample data
     * @param scale the color palette name
     * 
     * + default value Is ``'gray'``.
     * @param levels the color scaler levels
     * 
     * + default value Is ``255``.
     * @param filters the raster filter object, data type could be @``T:BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler.RasterPipeline`` or the ggplot wrapper of the pipeline: @``T:ggplotMSImaging.MSIFilterPipelineOption``.
     * 
     * + default value Is ``null``.
     * @param backcolor 
     * + default value Is ``'black'``.
     * @param env -
     * 
     * + default value Is ``null``.
     * @return a gdi+ raster image
   */
   function raster_blending(pixels: any, dims: any, scale?: string, levels?: object, filters?: any, backcolor?: any, env?: object): object;
}
