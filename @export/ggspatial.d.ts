// export R# package module type define for javascript/typescript language
//
//    imports "ggspatial" from "MSImaging";
//
// ref=ggplotMSImaging.ggplotSpatial@MSImaging, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * spatial mapping between the STdata and SMdata based on the ggplot framework
 * 
*/
declare namespace ggspatial {
   /**
    * add a spatial overlaps of the STdata on current SMdata imaging
    * 
    * 
     * @param tile -
     * @param colorSet 
     * + default value Is ``'grays'``.
     * @param size 
     * + default value Is ``13``.
     * @param env 
     * + default value Is ``null``.
   */
   function geom_spatialtile(tile: object, geneID: string, STdata: any, colorSet?: any, size?: number, env?: object): any;
}
