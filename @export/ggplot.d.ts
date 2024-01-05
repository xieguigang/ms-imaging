// export R# package module type define for javascript/typescript language
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
   function geom_color(mz: number, color: any, tolerance?: any, pixel_render?: boolean, env?: object): object;
   /**
    * config of the background of the MS-imaging charting plot.
    * 
    * 
     * @param background the background color value or character vector ``TIC`` or ``BPC``.
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
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function geom_msimaging(mz: number, tolerance?: any, pixel_render?: boolean, TrIQ?: number, color?: any, knnFill?: boolean, colorLevels?: object, raster?: any, env?: object): object;
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
   function geom_MSIruler(color?: any, width?: number): object;
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
     * @param scale default value Is ``'gray'``.
     * @param levels default value Is ``255``.
     * @param env default value Is ``null``.
   */
   function raster_blending(pixels: object, dims: any, scale?: string, levels?: object, env?: object): object;
}
