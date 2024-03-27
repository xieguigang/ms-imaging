// export R# package module type define for javascript/typescript language
//
//    imports "MSIfilter" from "MSImaging";
//
// ref=ggplotMSImaging.Filters@MSImaging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * Helper function module for create the image filter pipeline
 * 
*/
declare namespace MSIfilter {
   /**
     * @param q default value Is ``0.01``.
   */
   function denoise_scale(q?: number): object;
   /**
    * removes low intensity spots
    * 
    * 
     * @param threshold -
     * 
     * + default value Is ``0.05``.
     * @param quantile -
     * 
     * + default value Is ``false``.
   */
   function intensity_cut(threshold?: number, quantile?: boolean): object;
   /**
    * Trying to fill the missing spatial spot on the imaging via knn method
    * 
    * 
     * @param k -
     * 
     * + default value Is ``3``.
     * @param q -
     * 
     * + default value Is ``0.65``.
     * @param random 
     * + default value Is ``false``.
   */
   function knn_scale(k?: object, q?: number, random?: boolean): object;
   /**
    * Normalized the raw input intensity value via log(N)
    * 
    * 
     * @param base log(N), N=2 by default
     * 
     * + default value Is ``2``.
   */
   function log_scale(base?: number): object;
   /**
     * @param q default value Is ``0.5``.
   */
   function quantile_scale(q?: number): object;
   /**
    * Make convolution of the spatial data for make the imaging render result soften
    * 
    * 
   */
   function soften_scale(): object;
   /**
    * Trim the raw input intensity value via the TrIQ algorithm
    * 
    * 
     * @param q -
     * 
     * + default value Is ``0.6``.
   */
   function TrIQ_scale(q?: number): object;
}
