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
    * test for the ruler formatter
    * 
    * 
     * @param x should be a numeric vector of ``μm`` for auto formatted
   */
   function format_ruler(x: any): any;
   /**
    * draw scatter layer with a given x and y coordinates.
    * 
    * 
     * @param x a numeric vector of pixel x
     * @param y a numeric vector of pixel y
     * @param colors the colors for rendering each scatter spot
     * @param size the spot size for the drawing
     * 
     * + default value Is ``3``.
     * @param alpha the transparency alpha value for drawing the scatter points.
     * 
     * + default value Is ``0.800000011920929``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function geom_spatialScatter(x: any, y: any, colors: any, size?: number, alpha?: number, env?: object): object;
   /**
    * add a spatial overlaps of the STdata on current SMdata imaging
    * 
    * 
     * @param tile A collection of the spatial spot mapping
     * @param geneID the gene ID for pull the spatial expression from the **`STdata`**.
     * 
     * + default value Is ``null``.
     * @param STdata the spatial transcriptomics data matrix
     * 
     * + default value Is ``null``.
     * @param colorSet the color set data for rendering the gene expression spatial heatmap
     * 
     * + default value Is ``'grays'``.
     * @param size the spot size when drawing the spatial expression
     * 
     * + default value Is ``13``.
     * @param env 
     * + default value Is ``null``.
   */
   function geom_spatialtile(tile: object, geneID?: string, STdata?: any, colorSet?: any, size?: number, env?: object): object;
}
