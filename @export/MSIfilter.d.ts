// export R# package module type define for javascript/typescript language
//
//    imports "MSIfilter" from "MSImaging";
//
// ref=ggplotMSImaging.Filters@MSImaging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace MSIfilter {
   /**
     * @param q default value Is ``0.01``.
   */
   function denoise_scale(q?: number): object;
   /**
     * @param k default value Is ``3``.
     * @param q default value Is ``0.65``.
   */
   function knn_scale(k?: object, q?: number): object;
   /**
     * @param base default value Is ``2``.
   */
   function log_scale(base?: number): object;
   /**
     * @param q default value Is ``0.5``.
   */
   function quantile_scale(q?: number): object;
   /**
   */
   function soften_scale(): object;
   /**
     * @param q default value Is ``0.6``.
   */
   function TrIQ_scale(q?: number): object;
}
