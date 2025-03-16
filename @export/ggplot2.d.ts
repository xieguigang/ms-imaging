// export R# package module type define for javascript/typescript language
//
//    imports "ggplot2" from "ggplot";
//
// ref=ggplot.ggplot2@ggplot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * Create Elegant Data Visualisations Using the Grammar of Graphics
 * 
*/
declare namespace ggplot2 {
   /**
    * ### Construct aesthetic mappings
    *  
    *  Aesthetic mappings describe how variables in the data are mapped 
    *  to visual properties (aesthetics) of geoms. Aesthetic mappings 
    *  can be set in ggplot() and in individual layers.
    * 
    * > This function also standardises aesthetic names by converting color to 
    * >  colour (also in substrings, e.g., point_color to point_colour) and 
    * >  translating old style R names to ggplot names (e.g., pch to shape and 
    * >  cex to size).
    * 
     * @param x List of name-value pairs in the form aesthetic = variable describing 
     *  which variables in the layer data should be mapped to which aesthetics 
     *  used by the paired geom/stat. The expression variable is evaluated 
     *  within the layer data, so there is no need to refer to the original 
     *  dataset (i.e., use ggplot(df, aes(variable)) instead of 
     *  ``ggplot(df, aes(df$variable)))``. The names for x and y aesthetics 
     *  are typically omitted because they are so common; all other aesthetics
     *  must be named.
     * 
     * + default value Is ``null``.
     * @param y -
     * 
     * + default value Is ``null``.
     * @param z 
     * + default value Is ``null``.
     * @param label data source for the scatter annotation label text
     * 
     * + default value Is ``null``.
     * @param color The color mapping name
     * 
     * + default value Is ``null``.
     * @param colour the alias name of the color parameter
     * 
     * + default value Is ``null``.
     * @param alpha 
     * + default value Is ``null``.
     * @param fill 
     * + default value Is ``null``.
     * @param title 
     * + default value Is ``null``.
     * @param shape 
     * + default value Is ``null``.
     * @param class mapping data of the element class group.
     * 
     * + default value Is ``null``.
     * @param args 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
     * @return A list with class uneval. Components of the list are either quosures 
     *  or constants.
   */
   function aes(x?: any, y?: any, z?: any, label?: any, color?: any, colour?: any, alpha?: any, fill?: any, title?: string, shape?: any, class?: any, args?: object, env?: object): object;
   /**
    * annotation_raster: Annotation: high-performance rectangular tiling
    *  
    *  This is a special version of geom_raster() optimised for static 
    *  annotations that are the same in every panel. These annotations 
    *  will not affect scales (i.e. the x and y axes will not grow to cover 
    *  the range of the raster, and the raster must already have its own 
    *  colours). This is useful for adding bitmap images.
    * 
    * 
     * @param raster raster object to display, may be an array or a nativeRaster
   */
   function annotation_raster(raster: any): any;
   /**
    * Swapping X- and Y-Axes
    * 
    * 
   */
   function coord_flip(): object;
   /**
    * means nothing
    * 
    * 
   */
   function element_blank(): object;
   /**
    * Theme element: line.
    * 
    * 
     * @param colour line colour
     * 
     * + default value Is ``null``.
     * @param size line size
     * 
     * + default value Is ``null``.
     * @param linetype line type
     * 
     * + default value Is ``null``.
     * @param lineend line end
     * 
     * + default value Is ``null``.
     * @param color an alias for ``colour``
     * 
     * + default value Is ``null``.
   */
   function element_line(colour?: any, size?: any, linetype?: any, lineend?: any, color?: any): object;
   /**
    * ## Theme elements
    *  
    *  In conjunction with the theme system, the ``element_`` functions
    *  specify the display of how non-data components of the plot are 
    *  drawn.
    *  
    *  borders and backgrounds.
    * 
    * 
     * @param fill Fill colour.
     * 
     * + default value Is ``null``.
     * @param colour Line/border colour. Color is an alias for colour.
     * 
     * + default value Is ``null``.
     * @param size Line/border size in mm; text size in pts.
     * 
     * + default value Is ``1``.
     * @param linetype Line type. An integer (0:8), a name (blank, solid, dashed, dotted, 
     *  dotdash, longdash, twodash), or a string with an even number (up 
     *  to eight) of hexadecimal digits which give the lengths in consecutive 
     *  positions in the string.
     * 
     * + default value Is ``null``.
     * @param color Line/border colour. Color is an alias for colour.
     * 
     * + default value Is ``null``.
     * @param inherit_blank Should this element inherit the existence of an element_blank 
     *  among its parents? If TRUE the existence of a blank element
     *  among its parents will cause this element to be blank as well. 
     *  If FALSE any blank parent element will be ignored when calculating
     *  final element state.
     * 
     * + default value Is ``false``.
     * @param env 
     * + default value Is ``null``.
   */
   function element_rect(fill?: any, colour?: any, size?: number, linetype?: object, color?: any, inherit_blank?: boolean, env?: object): object;
   /**
    * ### Theme elements
    *  
    *  text.
    * 
    * 
     * @param family Font family
     * 
     * + default value Is ``null``.
     * @param face Font face ("plain", "italic", "bold", "bold.italic")
     * 
     * + default value Is ``null``.
     * @param size Line/border size in mm; text size in pts.
     * 
     * + default value Is ``0``.
     * @param hjust Horizontal justification (in [0, 1])
     * 
     * + default value Is ``0``.
     * @param vjust Vertical justification (in [0, 1])
     * 
     * + default value Is ``0``.
     * @param angle Angle (in [0, 360])
     * 
     * + default value Is ``0``.
     * @param lineheight Line height
     * 
     * + default value Is ``0``.
     * @param color Line/border colour. Color is an alias for colour.
     * 
     * + default value Is ``null``.
     * @param margin Margins around the text. See margin() for more details. When creating a theme, 
     *  the margins should be placed on the side of the text facing towards the center 
     *  of the plot.
     * 
     * + default value Is ``0``.
     * @param debug If TRUE, aids visual debugging by drawing a solid rectangle behind the complete 
     *  text area, and a point where each label is anchored.
     * 
     * + default value Is ``false``.
     * @param inherit_blank Should this element inherit the existence of an element_blank among its parents? 
     *  If TRUE the existence of a blank element among its parents will cause this 
     *  element to be blank as well. If FALSE any blank parent element will be ignored 
     *  when calculating final element state.
     * 
     * + default value Is ``false``.
   */
   function element_text(family?: string, face?: string, size?: number, hjust?: number, vjust?: number, angle?: number, lineheight?: number, color?: string, margin?: number, debug?: boolean, inherit_blank?: boolean): object;
   /**
    * ### Bar charts
    *  
    *  There are two types of bar charts: geom_bar() and geom_col(). geom_bar() 
    *  makes the height of the bar proportional to the number of cases in each
    *  group (or if the weight aesthetic is supplied, the sum of the weights).
    *  If you want the heights of the bars to represent values in the data, use 
    *  geom_col() instead. geom_bar() uses stat_count() by default: it counts 
    *  the number of cases at each x position. geom_col() uses stat_identity():
    *  it leaves the data as is.
    * 
    * 
     * @param stat -
     * 
     * + default value Is ``'identity'``.
     * @param position -
     * 
     * + default value Is ``'stack'``.
     * @param color -
     * 
     * + default value Is ``'black'``.
     * @param width -
     * 
     * + default value Is ``0.7``.
     * @param size -
     * 
     * + default value Is ``0.25``.
     * @param show_legend -
     * 
     * + default value Is ``true``.
     * @param env -
     * 
     * + default value Is ``null``.
     * @return A bar plot layer
   */
   function geom_bar(stat?: any, position?: any, color?: any, width?: number, size?: number, show_legend?: boolean, env?: object): object;
   /**
     * @param color default value Is ``null``.
     * @param width default value Is ``1``.
     * @param alpha default value Is ``0.95``.
     * @param env default value Is ``null``.
   */
   function geom_barplot(color?: object, width?: number, alpha?: number, env?: object): object;
   /**
    * ## A box and whiskers plot (in the style of Tukey)
    *  
    *  The boxplot compactly displays the distribution of a continuous variable. 
    *  It visualises five summary statistics (the median, two hinges and two 
    *  whiskers), and all "outlying" points individually.
    * 
    * > ## Orientation
    * >  This geom treats Each axis differently And, thus, can thus have two 
    * >  orientations. Often the orientation Is easy To deduce from a combination 
    * >  Of the given mappings And the types Of positional scales In use. Thus, 
    * >  ggplot2 will by Default Try To guess which orientation the layer should 
    * >  have. Under rare circumstances, the orientation Is ambiguous And guessing 
    * >  may fail. In that Case the orientation can be specified directly Using the 
    * >  orientation parameter, which can be either "x" Or "y". The value gives 
    * >  the axis that the geom should run along, "x" being the Default orientation 
    * >  you would expect For the geom.
    * > 
    * >  ## Summary statistics
    * >  The lower And upper hinges correspond To the first And third quartiles 
    * >  (the 25th And 75th percentiles). This differs slightly from the method 
    * >  used by the boxplot() Function, And may be apparent With small samples. 
    * >  See boxplot.stats() For For more information On how hinge positions are 
    * >  calculated For boxplot().
    * > 
    * >  The upper whisker extends from the hinge To the largest value no further 
    * >  than 1.5 * IQR from the hinge (where IQR Is the inter-quartile range, Or 
    * >  distance between the first And third quartiles). The lower whisker extends 
    * >  from the hinge To the smallest value at most 1.5 * IQR Of the hinge. Data 
    * >  beyond the End Of the whiskers are called "outlying" points And are plotted 
    * >  individually.
    * > 
    * >  In a notched box plot, the notches extend 1.58 * IQR / sqrt(n). This gives a 
    * >  roughly 95% confidence interval for comparing medians. See McGill et al. 
    * >  (1978) for more details.
    * > 
    * >  ## Aesthetics
    * >  geom_boxplot() understands the following aesthetics (required aesthetics are 
    * >  in bold):
    * > 
    * >  + x Or y
    * >  + lower Or xlower
    * >  + upper Or xupper
    * >  + middle Or xmiddle
    * >  + ymin Or xmin
    * >  + ymax Or xmax
    * >  + alpha
    * >  + colour
    * >  + fill
    * >  + group
    * >  + linetype
    * >  + shape
    * >  + size
    * >  + weight
    * > 
    * >  Learn more about setting these aesthetics In vignette("ggplot2-specs").
    * > 
    * >  ## Computed variables
    * >  stat_boxplot() provides the following variables, some of which depend on the orientation:
    * > 
    * >  + width: width of boxplot
    * >  + ymin Or xmin: lower whisker = smallest observation greater than Or equal To lower hinge - 1.5 * IQR
    * >  + lower Or xlower: lower hinge, 25% quantile
    * >  + notchlower: lower edge Of notch = median - 1.58 * IQR / sqrt(n)
    * >  + middle Or xmiddle: median, 50% quantile
    * >  + notchupper: upper edge Of notch = median + 1.58 * IQR / sqrt(n)
    * >  + upper Or xupper: upper hinge, 75% quantile
    * >  + ymax Or xmax: upper whisker = largest observation less than Or equal To upper hinge + 1.5 * IQR
    * > 
    * >  ## References
    * >  
    * >  > McGill, R., Tukey, J. W. And Larsen, W. A. (1978) Variations of box plots. 
    * >    The American Statistician 32, 12-16.
    * 
     * @param color 
     * + default value Is ``null``.
     * @param width 
     * + default value Is ``1``.
     * @param alpha 
     * + default value Is ``0.95``.
     * @param env 
     * + default value Is ``null``.
   */
   function geom_boxplot(color?: object, width?: number, alpha?: number, env?: object): object;
   /**
    * ### Bar charts
    *  
    *  There are two types of bar charts: geom_bar() and geom_col(). geom_bar() 
    *  makes the height of the bar proportional to the number of cases in each
    *  group (or if the weight aesthetic is supplied, the sum of the weights).
    *  If you want the heights of the bars to represent values in the data, use 
    *  geom_col() instead. geom_bar() uses stat_count() by default: it counts 
    *  the number of cases at each x position. geom_col() uses stat_identity():
    *  it leaves the data as is.
    * 
    * 
     * @param mapping Set of aesthetic mappings created by aes(). If specified
     *  and inherit.aes = TRUE (the default), it is combined with the default mapping 
     *  at the top level of the plot. You must supply mapping if there is no plot 
     *  mapping.
     * 
     * + default value Is ``null``.
     * @param data The data to be displayed in this layer. There are three options:
     *  If NULL, the Default, the data Is inherited from the plot data As specified 
     *  In the Call To ggplot().
     *  A data.frame, Or other Object, will override the plot data. All objects will be
     *  fortified To produce a data frame. See fortify() For which variables will be
     *  created.
     *  A Function will be called With a Single argument, the plot data. The Return 
     *  value must be a data.frame, And will be used As the layer data. A Function 
     *  can be created from a formula (e.g. ~ head(.x, 10)).
     * 
     * + default value Is ``null``.
     * @param position Position adjustment, either as a string naming the 
     *  adjustment (e.g. "jitter" to use position_jitter), or the result of a call 
     *  to a position adjustment function. Use the latter if you need to change the
     *  settings of the adjustment.
     * 
     * + default value Is ``'stack'``.
     * @param just Adjustment for column placement. Set to 0.5 by default, 
     *  meaning that columns will be centered about axis breaks. Set to 0 or 1 to place 
     *  columns to the left/right of axis breaks. Note that this argument may have 
     *  unintended behaviour when used with alternative positions, e.g. position_dodge().
     * 
     * + default value Is ``0.5``.
     * @param width Bar width. By default, set to 90% of the resolution() of
     *  the data.
     * 
     * + default value Is ``0``.
     * @param na_rm If FALSE, the default, missing values are removed with a
     *  warning. If TRUE, missing values are silently removed.
     * 
     * + default value Is ``false``.
     * @param show_legend logical. Should this layer be included in the legends? 
     *  NA, the default, includes if any aesthetics are mapped. FALSE never includes, 
     *  and TRUE always includes. It can also be a named logical vector to finely select 
     *  the aesthetics to display.
     * 
     * + default value Is ``null``.
     * @param inherit_aes If FALSE, overrides the default aesthetics, rather 
     *  than combining with them. This is most useful for helper functions that define 
     *  both data and aesthetics and shouldn't inherit behaviour from the default plot 
     *  specification, e.g. borders().
     * 
     * + default value Is ``true``.
     * @param args Other arguments passed on to layer(). These are often aesthetics,
     *  used to set an aesthetic to a fixed value, like colour = "red" or size = 3. They 
     *  may also be parameters to the paired geom/stat.
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function geom_col(mapping?: any, data?: any, position?: any, just?: number, width?: number, na_rm?: boolean, show_legend?: object, inherit_aes?: boolean, args?: object, env?: object): any;
   /**
     * @param mapping default value Is ``null``.
     * @param alpha default value Is ``1``.
   */
   function geom_convexHull(mapping?: object, alpha?: number): object;
   /**
    * ## Histograms and frequency polygons
    *  
    *  Visualise the distribution of a single continuous variable by dividing 
    *  the x axis into bins and counting the number of observations in each bin. 
    *  Histograms (``geom_histogram()``) display the counts with bars;
    * 
    * 
     * @param bins Number of bins. Overridden by binwidth. Defaults to 30.
     * 
     * + default value Is ``20``.
     * @param position 
     * + default value Is ``null``.
     * @param color 
     * + default value Is ``null``.
     * @param alpha 
     * + default value Is ``1``.
     * @param binwidth 
     * + default value Is ``0.1``.
     * @param range 
     * + default value Is ``null``.
     * @param env 
     * + default value Is ``null``.
     * @return A histogram plot layer
   */
   function geom_histogram(bins?: object, position?: object, color?: any, alpha?: number, binwidth?: number, range?: any, env?: object): object;
   /**
    * ### Reference line defined by Y intercept. Useful for annotating plots.
    *  
    *  Using the described geometry, you can insert a simple geometric 
    *  object into your data visualization – a line defined by a position 
    *  on the Y axis.
    * 
    * 
     * @param yintercept -
     * @param color -
     * 
     * + default value Is ``'black'``.
     * @param line_width 
     * + default value Is ``3``.
     * @param linetype -
     * 
     * + default value Is ``null``.
   */
   function geom_hline(yintercept: number, color?: any, line_width?: number, linetype?: object): object;
   /**
     * @param mapping default value Is ``null``.
     * @param data default value Is ``null``.
     * @param stat default value Is ``'identity'``.
     * @param width default value Is ``0.5``.
     * @param radius default value Is ``10``.
     * @param alpha default value Is ``0.85``.
     * @param color default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function geom_jitter(mapping?: object, data?: any, stat?: any, width?: number, radius?: number, alpha?: number, color?: object, env?: object): object;
   /**
    * ### Connect observations
    *  
    *  geom_path() connects the observations in the order in which they appear in 
    *  the data. geom_line() connects them in order of the variable on the x axis. 
    *  geom_step() creates a stairstep plot, highlighting exactly when changes 
    *  occur. The group aesthetic determines which cases are connected together.
    * 
    * > An alternative parameterisation is geom_segment(), where each line corresponds 
    * >  to a single case which provides the start and end coordinates.
    * 
     * @param mapping 
     * + default value Is ``null``.
     * @param color 
     * + default value Is ``null``.
     * @param width 
     * + default value Is ``5``.
     * @param show_legend 
     * + default value Is ``true``.
     * @param alpha 
     * + default value Is ``1``.
     * @param bspline options for the b-spline smooth of the line plot
     * 
     * + default value Is ``false``.
     * @param env 
     * + default value Is ``null``.
     * @return A line chart plot layer
   */
   function geom_line(mapping?: object, color?: any, width?: number, show_legend?: boolean, alpha?: number, bspline?: boolean, env?: object): object;
   /**
    * ## Connect observations
    *  
    *  geom_path() connects the observations in the order in which they 
    *  appear in the data. geom_line() connects them in order of the 
    *  variable on the x axis. geom_step() creates a stairstep plot, highlighting 
    *  exactly when changes occur. The group aesthetic determines which 
    *  cases are connected together.
    * 
    * 
   */
   function geom_path(): object;
   /**
   */
   function geom_pie(): object;
   /**
    * ### Scatter Points
    *  
    *  The point geom is used to create scatterplots. The scatterplot is most 
    *  useful for displaying the relationship between two continuous variables. 
    *  It can be used to compare one continuous and one categorical variable, 
    *  or two categorical variables, but a variation like geom_jitter(), 
    *  geom_count(), or geom_bin2d() is usually more appropriate. A bubblechart 
    *  is a scatterplot with a third variable mapped to the size of points.
    * 
    * 
     * @param mapping 
     * + default value Is ``null``.
     * @param color -
     * 
     * + default value Is ``null``.
     * @param shape -
     * 
     * + default value Is ``null``.
     * @param stroke 
     * + default value Is ``null``.
     * @param size -
     * 
     * + default value Is ``2``.
     * @param show_legend logical. Should this layer be included in the legends? NA, the default, 
     *  includes if any aesthetics are mapped. FALSE never includes, And TRUE 
     *  always includes. It can also be a named logical vector to finely select 
     *  the aesthetics to display.
     * 
     * + default value Is ``true``.
     * @param alpha 
     * + default value Is ``1``.
     * @param env -
     * 
     * + default value Is ``null``.
     * @return a scatter plot layer
   */
   function geom_point(mapping?: object, color?: any, shape?: object, stroke?: any, size?: number, show_legend?: boolean, alpha?: number, env?: object): object;
   /**
     * @param layout default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function geom_raster(bitmap: any, layout?: any, env?: object): any;
   /**
     * @param colors default value Is ``null``.
     * @param env default value Is ``null``.
   */
   function geom_scatterheatmap(data: string, colors?: string, env?: object): object;
   /**
   */
   function geom_scatterpie(data: string): object;
   /**
    * ### Line segments and curves
    *  
    *  geom_segment() draws a straight line between points (x, y) and (xend, yend). Both geoms 
    *  draw a single segment/curve per case. See geom_path() if you need to connect points 
    *  across multiple cases.
    * 
    * > geom_segment() understands the following aesthetics (required aesthetics are in bold):
    * >  
    * >  + **x**
    * >  + **y**
    * >  + **xend or yend**
    * >  + alpha
    * >  + colour
    * >  + group
    * >  + linetype
    * >  + linewidth
    * >  
    * >  Learn more about setting these aesthetics in vignette("ggplot2-specs").
    * 
     * @param mapping Set of aesthetic mappings created by aes(). If specified and ``inherit.aes = TRUE`` (the default),
     *  it is combined with the default mapping at the top level of the plot. You must supply mapping if there is no 
     *  plot mapping.
     * 
     * + default value Is ``null``.
     * @param data The data to be displayed in this layer. There are three options:
     *  
     *  If NULL, the default, the data is inherited from the plot data as specified in the call to ggplot().
     *  
     *  A data.frame, or other object, will override the plot data. All objects will be fortified to 
     *  produce a data frame. See fortify() for which variables will be created.
     *  
     *  A function will be called with a single argument, the plot data. The return value must be a data.frame, 
     *  and will be used as the layer data. A function can be created from a formula (e.g. ~ head(.x, 10)).
     * 
     * + default value Is ``null``.
     * @param stat The statistical transformation to use on the data for this layer. When using a geom_*() 
     *  function to construct a layer, the stat argument can be used the override the default coupling between geoms
     *  and stats. The stat argument accepts the following:
     *  
     *  A Stat ggproto subclass, for example StatCount.
     *  
     *  A string naming the stat. To give the stat as a string, strip the function name of the stat_ prefix. 
     *  For example, to use stat_count(), give the stat as "count".
     *  
     *  For more information and other ways to specify the stat, see the layer stat documentation.
     * 
     * + default value Is ``'identity'``.
     * @param position A position adjustment to use on the data for this layer. This can be used in 
     *  various ways, including to prevent overplotting and improving the display. The position argument 
     *  accepts the following:
     *  
     *  The result of calling a position function, such as position_jitter(). This method allows for passing 
     *  extra arguments to the position.
     *  
     *  A string naming the position adjustment. To give the position as a string, strip the function name of 
     *  the position_ prefix. For example, to use position_jitter(), give the position as "jitter".
     *  
     *  For more information and other ways to specify the position, see the layer position documentation.
     * 
     * + default value Is ``'identity'``.
     * @param arrow specification for arrow heads, as created by grid::arrow().
     * 
     * + default value Is ``null``.
     * @param arrow_fill fill colour to use for the arrow head (if closed). NULL means use colour aesthetic.
     * 
     * + default value Is ``null``.
     * @param lineend Line end style (round, butt, square).
     * 
     * + default value Is ``'butt'``.
     * @param linejoin Line join style (round, mitre, bevel).
     * 
     * + default value Is ``'round'``.
     * @param na_rm If FALSE, the default, missing values are removed with a warning. If TRUE, missing values are silently removed.
     * 
     * + default value Is ``false``.
     * @param show_legend logical. Should this layer be included in the legends? NA, the default, 
     *  includes if any aesthetics are mapped. FALSE never includes, and TRUE always includes. It can also 
     *  be a named logical vector to finely select the aesthetics to display.
     * 
     * + default value Is ``false``.
     * @param inherit_aes If FALSE, overrides the default aesthetics, rather than combining with them. 
     *  This is most useful for helper functions that define both data and aesthetics and shouldn't inherit 
     *  behaviour from the default plot specification, e.g. borders().
     * 
     * + default value Is ``true``.
     * @param args Other arguments passed on to layer()'s params argument. These arguments broadly 
     *  fall into one of 4 categories below. Notably, further arguments to the position argument, or aesthetics 
     *  that are required can not be passed through .... Unknown arguments that are not part of the 4 
     *  categories below are ignored.
     *  
     *  Static aesthetics that are not mapped to a scale, but are at a fixed value and apply to the layer as 
     *  a whole. For example, colour = "red" or linewidth = 3. The geom's documentation has an Aesthetics 
     *  section that lists the available options. The 'required' aesthetics cannot be passed on to the params.
     *  Please note that while passing unmapped aesthetics as vectors is technically possible, the order and 
     *  required length is not guaranteed to be parallel to the input data.
     *  
     *  When constructing a layer using a stat_*() function, the ... argument can be used to pass on parameters 
     *  to the geom part of the layer. An example of this is stat_density(geom = "area", outline.type = "both").
     *  The geom's documentation lists which parameters it can accept.
     *  
     *  Inversely, when constructing a layer using a geom_*() function, the ... argument can be used to pass on 
     *  parameters to the stat part of the layer. An example of this is geom_area(stat = "density", adjust = 0.5).
     *  The stat's documentation lists which parameters it can accept.
     *  
     *  The key_glyph argument of layer() may also be passed on through .... This can be one of the functions 
     *  described as key glyphs, to change the display of the layer in the legend.
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function geom_segment(mapping?: object, data?: any, stat?: string, position?: string, arrow?: object, arrow_fill?: any, lineend?: string, linejoin?: string, na_rm?: boolean, show_legend?: boolean, inherit_aes?: boolean, args?: object, env?: object): object;
   /**
    * ## Create significance layer
    * 
    * 
     * @param comparisons the comparision groups tuple list, each tuple value should be 
     *  two group label for extract the corresponding sample vector 
     *  from the ggplot input raw dataframe for run the ``t.test``.
     * @param test the stats test method between two groups: t.test or wilcox.test
     * 
     * + default value Is ``'t.test'``.
   */
   function geom_signif(comparisons: object, test?: string): object;
   /**
    * ### Text
    *  
    *  Text geoms are useful for labeling plots. They can be used by themselves 
    *  as scatterplots or in combination with other geoms, for example, for 
    *  labeling points or for annotating the height of bars. geom_text() adds 
    *  only text to the plot. geom_label() draws a rectangle behind the text, 
    *  making it easier to read.
    * 
    * 
     * @param mapping Set of aesthetic mappings created by aes() or aes_(). If specified and 
     *  inherit.aes = TRUE (the default), it is combined with the default mapping 
     *  at the top level of the plot. You must supply mapping if there is no plot 
     *  mapping.
     * 
     * + default value Is ``null``.
     * @param data The data to be displayed in this layer. There are three options:
     *  
     *  If NULL, the Default, the data Is inherited from the plot data As 
     *  specified In the Call To ggplot().
     *  
     *  A data.frame, Or other Object, will override the plot data. All objects 
     *  will be fortified To produce a data frame. See fortify() For which 
     *  variables will be created.
     *  
     *  A Function will be called With a Single argument, the plot data. The Return 
     *  value must be a data.frame, And will be used As the layer data. A Function 
     *  can be created from a formula (e.g. ~ head(.x, 10)).
     * 
     * + default value Is ``null``.
     * @param stat The statistical transformation to use on the data for this layer, as a 
     *  string.
     * 
     * + default value Is ``'identity'``.
     * @param position Position adjustment, either as a string, or the result of a call to a 
     *  position adjustment function. Cannot be jointy specified with nudge_x or 
     *  nudge_y.
     * 
     * + default value Is ``'identity'``.
     * @param parse If TRUE, the labels will be parsed into expressions and displayed as 
     *  described in ?plotmath.
     * 
     * + default value Is ``false``.
     * @param nudge_x Horizontal and vertical adjustment to nudge labels by. Useful for 
     *  offsetting text from points, particularly on discrete scales. Cannot be 
     *  jointly specified with position.
     * 
     * + default value Is ``0``.
     * @param nudge_y Horizontal and vertical adjustment to nudge labels by. Useful for 
     *  offsetting text from points, particularly on discrete scales. Cannot be 
     *  jointly specified with position.
     * 
     * + default value Is ``0``.
     * @param check_overlap If TRUE, text that overlaps previous text in the same layer will not be 
     *  plotted. check_overlap happens at draw time and in the order of the data. 
     *  Therefore data should be arranged by the label column before calling 
     *  geom_text(). Note that this argument is not supported by geom_label().
     * 
     * + default value Is ``false``.
     * @param na_rm If False, the Default, missing values are removed With a warning. 
     *  If True, missing values are silently removed.
     * 
     * + default value Is ``false``.
     * @param show_legend logical. Should this layer be included in the legends? NA, the default, 
     *  includes if any aesthetics are mapped. FALSE never includes, and TRUE 
     *  always includes. It can also be a named logical vector to finely select
     *  the aesthetics to display.
     * 
     * + default value Is ``false``.
     * @param inherit_aes If False, Overrides the Default aesthetics, rather than combining With 
     *  them. This Is most useful For helper functions that define both data And 
     *  aesthetics And shouldn't inherit behaviour from the default plot 
     *  specification, e.g. borders().
     * 
     * + default value Is ``true``.
     * @param color -
     * 
     * + default value Is ``'steelblue'``.
     * @param which 
     * + default value Is ``null``.
     * @param alpha 
     * + default value Is ``1``.
     * @param size 
     * + default value Is ``null``.
     * @param args Other arguments passed On To layer(). These are often aesthetics, used To 
     *  Set an aesthetic To a fixed value, Like colour = "red" Or size = 3. They
     *  may also be parameters To the paired geom/stat.
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function geom_text(mapping?: object, data?: any, stat?: string, position?: string, parse?: boolean, nudge_x?: number, nudge_y?: number, check_overlap?: boolean, na_rm?: boolean, show_legend?: boolean, inherit_aes?: boolean, color?: any, which?: object, alpha?: number, size?: object, args?: object, env?: object): object;
   /**
     * @param mapping default value Is ``null``.
   */
   function geom_tile(mapping?: object): object;
   /**
     * @param color default value Is ``null``.
     * @param width default value Is ``0.9``.
     * @param alpha default value Is ``0.95``.
     * @param env default value Is ``null``.
   */
   function geom_violin(color?: object, width?: number, alpha?: number, env?: object): object;
   /**
    * ## Reference lines: horizontal, vertical, and diagonal
    *  
    *  These geoms add reference lines (sometimes called rules) to a plot, 
    *  either horizontal, vertical, or diagonal (specified by slope and
    *  intercept). These are useful for annotating plots.
    * 
    * 
     * @param xintercept Parameters that control the position of the line. If these are set, 
     *  data, mapping and show.legend are overridden.
     * @param color -
     * 
     * + default value Is ``'black'``.
     * @param line_width 
     * + default value Is ``2``.
     * @param linetype -
     * 
     * + default value Is ``null``.
   */
   function geom_vline(xintercept: number, color?: any, line_width?: number, linetype?: object): object;
   /**
    * ### Create a new ggplot
    *  
    *  ``ggplot()`` initializes a ggplot object. It can be used to declare 
    *  the input data frame for a graphic and to specify the set of 
    *  plot aesthetics intended to be common throughout all subsequent 
    *  layers unless specifically overridden.
    * 
    * > ``ggplot()`` is used to construct the initial plot object, and is 
    * >  almost always followed by + to add component to the plot. There 
    * >  are three common ways to invoke ``ggplot()``:
    * >  
    * >  + ``ggplot(df, aes(x, y, other aesthetics))``
    * >  + ``ggplot(df)``
    * >  + ``ggplot()``
    * >  
    * >  
    * >  The first method Is recommended If all layers use the same data 
    * >  And the same Set Of aesthetics, although this method can also be 
    * >  used To add a layer Using data from another data frame. See the 
    * >  first example below. The second method specifies the Default 
    * >  data frame To use For the plot, but no aesthetics are defined up 
    * >  front. This Is useful When one data frame Is used predominantly 
    * >  As layers are added, but the aesthetics may vary from one layer 
    * >  To another. The third method initializes a skeleton ggplot Object
    * >  which Is fleshed out As layers are added. This method Is useful 
    * >  When multiple data frames are used To produce different layers, 
    * >  As Is often the Case In complex graphics.
    * 
     * @param data Default dataset to use for plot. If not already a data.frame, 
     *  will be converted to one by fortify(). If not specified, must be 
     *  supplied in each layer added to the plot.
     * 
     * + default value Is ``null``.
     * @param mapping Default list of aesthetic mappings to use for plot. If not specified, 
     *  must be supplied in each layer added to the plot.
     * 
     * + default value Is ``null``.
     * @param colorSet 
     * + default value Is ``'paper'``.
     * @param args Other arguments passed on to methods. Not currently used.
     * 
     * + default value Is ``null``.
     * @param environment -
     * 
     * + default value Is ``null``.
     * @return a ggplot base layer object which can be rendering to graphics by 
     *  invoke the ``plot`` function.
   */
   function ggplot(data?: any, mapping?: any, colorSet?: any, args?: object, environment?: object): object;
   /**
    * 
    * 
     * @param filename -
     * @param plot -
     * 
     * + default value Is ``null``.
     * @param device -
     * 
     * + default value Is ``null``.
     * @param path -
     * 
     * + default value Is ``null``.
     * @param scale -
     * 
     * + default value Is ``1``.
     * @param width -
     * 
     * + default value Is ``null``.
     * @param height -
     * 
     * + default value Is ``null``.
     * @param units -
     * 
     * + default value Is ``null``.
     * @param dpi -
     * 
     * + default value Is ``300``.
     * @param limitsize -
     * 
     * + default value Is ``true``.
     * @param bg -
     * 
     * + default value Is ``null``.
     * @param args -
     * 
     * + default value Is ``null``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function ggsave(filename: string, plot?: any, device?: any, path?: any, scale?: number, width?: object, height?: object, units?: object, dpi?: object, limitsize?: boolean, bg?: any, args?: object, env?: object): any;
   /**
    * ### Modify axis, legend, and plot labels
    *  
    *  Good labels are critical for making your plots accessible 
    *  to a wider audience. Always ensure the axis and legend 
    *  labels display the full variable name. Use the plot title 
    *  and subtitle to explain the main findings. It's common to 
    *  use the caption to provide information about the data 
    *  source. tag can be used for adding identification tags to 
    *  differentiate between multiple plots.
    * 
    * > You can also set axis and legend labels in the individual 
    * >  scales (using the first argument, the name). If you're
    * >  changing other scale options, this is recommended.
    * >  
    * >  If a plot already has a title, subtitle, caption, etc., And
    * >  you want To remove it, you can Do so by setting the respective 
    * >  argument To NULL. For example, If plot p has a subtitle, Then
    * >  p + labs(subtitle = NULL) will remove the subtitle from the 
    * >  plot.
    * 
     * @param title The text for the title.
     * @param text_wrap 
     * + default value Is ``false``.
   */
   function ggtitle(title: string, text_wrap?: boolean): object;
   /**
    * ## Modify axis, legend, and plot labels
    *  
    *  Good labels are critical for making your plots accessible to 
    *  a wider audience. Always ensure the axis and legend labels 
    *  display the full variable name. Use the plot title and subtitle 
    *  to explain the main findings. It's common to use the caption 
    *  to provide information about the data source. tag can be used 
    *  for adding identification tags to differentiate between multiple 
    *  plots.
    * 
    * > You can also set axis and legend labels in the individual scales 
    * >  (using the first argument, the name). If you're changing other 
    * >  scale options, this is recommended.
    * >  
    * >  If a plot already has a title, subtitle, caption, etc., And you want 
    * >  To remove it, you can Do so by setting the respective argument To 
    * >  NULL. For example, If plot p has a subtitle, Then p + labs(subtitle = NULL) 
    * >  will remove the subtitle from the plot.
    * 
     * @param x -
     * 
     * + default value Is ``null``.
     * @param y -
     * 
     * + default value Is ``null``.
     * @param title The text for the title.
     * 
     * + default value Is ``null``.
     * @param subtitle The text For the subtitle For the plot which will be displayed 
     *  below the title.
     * 
     * + default value Is ``null``.
     * @param caption The text for the caption which will be displayed in the 
     *  bottom-right of the plot by default.
     * 
     * + default value Is ``null``.
     * @param tag The text for the tag label which will be displayed at the top-left 
     *  of the plot by default.
     * 
     * + default value Is ``null``.
     * @param alt Text used for the generation of alt-text for the plot. See 
     *  get_alt_text for examples.
     * 
     * + default value Is ``null``.
     * @param alt_insight -
     * 
     * + default value Is ``null``.
     * @param args A list of new name-value pairs. The name should be an aesthetic.
     * 
     * + default value Is ``null``.
     * @param env 
     * + default value Is ``null``.
   */
   function labs(x?: string, y?: string, title?: string, subtitle?: string, caption?: string, tag?: any, alt?: any, alt_insight?: any, args?: object, env?: object): object;
   /**
    * ### Retrieve the last plot to be modified or created.
    *  
    *  Retrieve the last plot to be modified or created.
    * 
    * 
   */
   function last_plot(): object;
   /**
    * Sequential, diverging and qualitative colour scales from ColorBrewer
    * 
    * > The brewer scales provide sequential, diverging and qualitative colour
    * >  schemes from ColorBrewer. These are particularly well suited to display 
    * >  discrete values on a map. See https://colorbrewer2.org for more 
    * >  information.
    * >  
    * >  The brewer scales were carefully designed and tested on discrete data. 
    * >  They were not designed to be extended to continuous data, but results 
    * >  often look good. Your mileage may vary.
    * >  
    * >  The following palettes are available for use with these scales:
    * >  
    * >  #### Diverging
    * >  BrBG, PiYG, PRGn, PuOr, RdBu, RdGy, RdYlBu, RdYlGn, Spectral
    * >  
    * >  #### Qualitative
    * >  Accent, Dark2, Paired, Pastel1, Pastel2, Set1, Set2, Set3
    * >  
    * >  #### Sequential
    * >  Blues, BuGn, BuPu, GnBu, Greens, Greys, Oranges, OrRd, PuBu, PuBuGn, PuRd, Purples, RdPu, Reds, YlGn, YlGnBu, YlOrBr, YlOrRd
    * >  
    * >  Modify the palette through the palette argument.
    * 
     * @param name The name of the scale. Used as the axis or legend title. If waiver(), the default,
     *  the name of the scale is taken from the first mapping used for that aesthetic. 
     *  If NULL, the legend title will be omitted.
     * 
     * + default value Is ``["BrBG","PiYG","PRGn","PuOr","RdBu","RdGy","RdYlBu","RdYlGn","Spectral","Accent","Dark2","Paired","Pastel1","Pastel2","Set1","Set2","Set3","Blues","BuGn","BuPu","GnBu","Greens","Greys","Oranges","OrRd","PuBu","PuBuGn","PuRd","Purples","RdPu","Reds","YlGn","YlGnBu","YlOrBr","YlOrRd"]``.
     * @param direction Sets the order Of colours In the scale. If 1, the Default, colours are 
     *  As output by RColorBrewer:brewer.pal(). If -1, the order of colours 
     *  Is reversed.
     * 
     * + default value Is ``1``.
     * @param alpha color alpha channel value between [0,1]
     * 
     * + default value Is ``1``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function scale_color_brewer(name?: any, direction?: object, alpha?: number, env?: object): object;
   /**
    * ### Create your own discrete scale
    *  
    *  These functions allow you to specify your own set of 
    *  mappings from levels in the data to aesthetic values.
    * 
    * > ### Color Blindness
    * >  
    * >  Many color palettes derived from RGB combinations (Like 
    * >  the "rainbow" color palette) are Not suitable To support 
    * >  all viewers, especially those With color vision 
    * >  deficiencies. 
    * >  
    * >  Using viridis type, which Is perceptually uniform In both 
    * >  colour And black-And-white display Is an easy Option To 
    * >  ensure good perceptive properties Of your visulizations. 
    * >  The colorspace package offers functionalities.
    * >  
    * >  to generate color palettes with good perceptive properties,
    * >  
    * >  to analyse a given color palette, Like emulating color 
    * >  blindness,
    * >  
    * >  And to modify a given color palette for better perceptivity.
    * >  
    * >  For more information on color vision deficiencies And 
    * >  suitable color choices see the paper on the colorspace 
    * >  package And references therein.
    * 
     * @param values -
     * @param alpha 
     * + default value Is ``1``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function scale_colour_manual(values: any, alpha?: number, env?: object): object;
   /**
     * @param palette default value Is ``'YlGnBu'``.
     * @param direction default value Is ``1``.
     * @param alpha default value Is ``1``.
     * @param env default value Is ``null``.
   */
   function scale_fill_distiller(palette?: string, direction?: object, alpha?: number, env?: object): object;
   /**
     * @param alpha default value Is ``1``.
     * @param env default value Is ``null``.
   */
   function scale_fill_manual(values: any, alpha?: number, env?: object): object;
   /**
    * Position scales for continuous data (x & y)
    * 
    * 
     * @param labels One of:
     *  
     *  + ``NULL`` for no labels
     *  + waiver() for the default labels computed by the transformation object
     *  + A character vector giving labels (must be same length As breaks)
     *  + A Function that() takes the breaks As input And returns labels As output. Also accepts rlang lambda Function notation.
     * 
     * + default value Is ``null``.
     * @param limits 
     * + default value Is ``null``.
     * @param env 
     * + default value Is ``null``.
   */
   function scale_x_continuous(labels?: string, limits?: number, env?: object): object;
   /**
    * ### Position scales for continuous data (x & y)
    * 
    * 
     * @param labels One of:
     *  
     *  + ``NULL`` for no labels
     *  + waiver() for the default labels computed by the transformation object
     *  + A character vector giving labels (must be same length As breaks)
     *  + A Function that() takes the breaks As input And returns labels As output. Also accepts rlang lambda Function notation.
     * 
     * + default value Is ``null``.
     * @param limits 
     * + default value Is ``null``.
     * @param env 
     * + default value Is ``null``.
   */
   function scale_y_continuous(labels?: string, limits?: number, env?: object): object;
   /**
    * ### Position scales for continuous data (x & y)
    * 
    * 
   */
   function scale_y_reverse(): object;
   /**
    * default create anova test for compares all groups
    * 
    * 
     * @param method -
     * 
     * + default value Is ``'anova'``.
     * @param ref_group -
     * 
     * + default value Is ``'.all.'``.
     * @param hide_ns hide not sig result?
     * 
     * + default value Is ``true``.
   */
   function stat_compare_means(method?: string, ref_group?: string, hide_ns?: boolean): object;
   /**
    * set stats p-value for the plot
    * 
    * 
     * @param comparisons a dataframe object that should contains the data fiels at least:
     *  
     *  + group1: the label name of the group 1
     *  + group2: the label name of the group 2
     *  + pvalue: a numeric vector of the t-test pvalue between group 1 and group 2.
   */
   function stat_pvalue_manual(comparisons: object): object;
   /**
    * ## Modify components of a theme
    *  
    *  Themes are a powerful way to customize the non-data components of 
    *  your plots: i.e. titles, labels, fonts, background, gridlines, and 
    *  legends. Themes can be used to give plots a consistent customized 
    *  look. Modify a single plot's theme using theme(); see theme_update() 
    *  if you want modify the active theme, to affect all subsequent plots. 
    *  Use the themes available in complete themes if you would like to use 
    *  a complete theme such as theme_bw(), theme_minimal(), and more. 
    *  
    *  Theme elements are documented together according to inheritance, read
    *  more about theme inheritance below.
    * 
    * > Theme elements inherit properties from other theme elements hierarchically. 
    * >  For example, axis.title.x.bottom inherits from axis.title.x which inherits 
    * >  from axis.title, which in turn inherits from text. All text elements inherit
    * >  directly or indirectly from text; all lines inherit from line, and all 
    * >  rectangular objects inherit from rect. This means that you can modify the 
    * >  appearance of multiple elements by setting a single high-level component.
    * >  
    * >  Learn more about setting these aesthetics In vignette("ggplot2-specs").
    * 
     * @param text all text elements (element_text())
     * 
     * + default value Is ``null``.
     * @param axis_text -
     * 
     * + default value Is ``null``.
     * @param axis_title 
     * + default value Is ``null``.
     * @param axis_line lines along axes (element_line()). Specify lines 
     *  along all axes (axis.line), lines for each plane (using axis.line.x or 
     *  axis.line.y), or individually for each axis (using axis.line.x.bottom, 
     *  axis.line.x.top, axis.line.y.left, axis.line.y.right). ``axis.line.*.*`` 
     *  inherits from axis.line.* which inherits from axis.line, which in turn 
     *  inherits from line
     * 
     * + default value Is ``'stroke: black; stroke-width: 2px; stroke-dash: solid;'``.
     * @param axis_text_x 
     * + default value Is ``null``.
     * @param legend_background background of legend (element_rect(); inherits from rect)
     * 
     * + default value Is ``'white'``.
     * @param legend_text legend item labels (element_text(); inherits from text)
     * 
     * + default value Is ``null``.
     * @param legend_tick 
     * + default value Is ``null``.
     * @param legend_title 
     * + default value Is ``null``.
     * @param legend_split 
     * + default value Is ``6``.
     * @param plot_background background of the entire plot (element_rect(); inherits from rect)
     * 
     * + default value Is ``null``.
     * @param plot_title 
     * + default value Is ``null``.
     * @param panel_background background of plotting area, drawn underneath plot (element_rect(); inherits from rect)
     * 
     * + default value Is ``null``.
     * @param panel_grid 
     * + default value Is ``'stroke: lightgray; stroke-width: 2px; stroke-dash: dash;'``.
     * @param panel_grid_major 
     * + default value Is ``'stroke: lightgray; stroke-width: 2px; stroke-dash: dash;'``.
     * @param panel_border 
     * + default value Is ``null``.
     * @return A style mapper for create the internal @``T:Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas.Theme`` object
   */
   function theme(text?: object, axis_text?: object, axis_title?: object, axis_line?: any, axis_text_x?: object, legend_background?: string, legend_text?: object, legend_tick?: object, legend_title?: object, legend_split?: object, plot_background?: string, plot_title?: object, panel_background?: string, panel_grid?: any, panel_grid_major?: any, panel_border?: object): object;
   /**
   */
   function theme_minimal(): object;
   /**
    * ### A waiver object.
    *  
    *  A waiver is a "flag" object, similar to NULL, that indicates the calling 
    *  function should just use the default value. It is used in certain functions 
    *  to distinguish between displaying nothing (NULL) and displaying a default 
    *  value calculated elsewhere (waiver())
    * 
    * 
   */
   function waiver(): object;
   /**
    * ## Modify axis, legend, and plot labels
    *  
    *  Good labels are critical for making your plots accessible to a 
    *  wider audience. Always ensure the axis and legend labels display 
    *  the full variable name. Use the plot title and subtitle to 
    *  explain the main findings. It's common to use the caption to 
    *  provide information about the data source. tag can be used for 
    *  adding identification tags to differentiate between multiple 
    *  plots.
    * 
    * > You can also set axis and legend labels in the individual scales 
    * >  (using the first argument, the name). If you're changing other 
    * >  scale options, this is recommended.
    * >  
    * >  If a plot already has a title, subtitle, caption, etc., And you want 
    * >  To remove it, you can Do so by setting the respective argument To 
    * >  NULL. For example, If plot p has a subtitle, Then p + labs(subtitle = NULL) 
    * >  will remove the subtitle from the plot.
    * 
     * @param label The title of the respective axis (for xlab() or ylab()) or 
     *  of the plot (for ggtitle()).
   */
   function xlab(label: string): object;
   /**
    * ## Modify axis, legend, and plot labels
    *  
    *  Good labels are critical for making your plots accessible to a 
    *  wider audience. Always ensure the axis and legend labels display 
    *  the full variable name. Use the plot title and subtitle to 
    *  explain the main findings. It's common to use the caption to 
    *  provide information about the data source. tag can be used for 
    *  adding identification tags to differentiate between multiple 
    *  plots.
    * 
    * > You can also set axis and legend labels in the individual scales 
    * >  (using the first argument, the name). If you're changing other 
    * >  scale options, this is recommended.
    * >  
    * >  If a plot already has a title, subtitle, caption, etc., And you want 
    * >  To remove it, you can Do so by setting the respective argument To 
    * >  NULL. For example, If plot p has a subtitle, Then p + labs(subtitle = NULL) 
    * >  will remove the subtitle from the plot.
    * 
     * @param label The title of the respective axis (for xlab() or ylab()) or 
     *  of the plot (for ggtitle()).
   */
   function ylab(label: string): object;
}
