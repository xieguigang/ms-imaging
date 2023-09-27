// export R# package module type define for javascript/typescript language
//
//    imports "ggplot3" from "ggplot";
//
// ref=ggplot.ggplot3@ggplot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * ggplot for 3D
 * 
*/
declare namespace ggplot3 {
   /**
    * Create view camera for 3D plot
    * 
    * 
     * @param view_distance 
     * + default value Is ``-75``.
     * @param fov 
     * + default value Is ``100000``.
     * @param angle 
     * + default value Is ``[31.5,65,125]``.
   */
   function view_camera(view_distance?: object, fov?: object, angle?: any): object;
}
