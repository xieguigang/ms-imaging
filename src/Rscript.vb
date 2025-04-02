#Region "Microsoft.VisualBasic::b4ca8523eadf1fcb25c348b2753c1ba8, Rscript\Library\MSI_app\src\Rscript.vb"

' Author:
' 
'       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
' 
' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
' 
' 
' MIT License
' 
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all
' copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
' SOFTWARE.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 704
'    Code Lines: 438 (62.22%)
' Comment Lines: 198 (28.12%)
'    - Xml Docs: 94.44%
' 
'   Blank Lines: 68 (9.66%)
'     File Size: 29.15 KB


' Module Rscript
' 
'     Function: ConfigMSIDimensionSize, CreateMSIheatmap, createPixelPack, gaussBlurOpt, geom_cmyk
'               geom_color, geom_MSIbackground, geom_MSIfilters, geom_msiheatmap, geom_msimaging
'               geom_MSIruler, geom_sample_outline, getChannel, hqx_opts, KnnFill
'               raster_blending, unionlayers
' 
' /********************************************************************************/

#End Region

Imports System.Data
Imports System.Drawing
Imports System.IO
Imports BioNovoGene.Analytical.MassSpectrometry
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports ggplot
Imports ggplot.colors
Imports ggplot.elements
Imports ggplot.layers
Imports ggplotMSImaging.data
Imports ggplotMSImaging.layers
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D.MarchingSquares
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports SMRUCC.Rsharp.Interpreter
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Closure
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.DataSets
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Operators
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports any = Microsoft.VisualBasic.Scripting
Imports PixelData = BioNovoGene.Analytical.MassSpectrometry.MsImaging.PixelData
Imports Polygon2D = Microsoft.VisualBasic.Imaging.Math2D.Polygon2D
Imports renv = SMRUCC.Rsharp.Runtime
Imports RInternal = SMRUCC.Rsharp.Runtime.Internal

#If NET48 Then
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports FontStyle = Microsoft.VisualBasic.Imaging.FontStyle
#End If

''' <summary>
''' the ggplot api plugin for do MS-Imaging rendering
''' </summary>
''' <remarks>
''' <see cref="ggplotMSI"/> is the ms-imaging render.
''' </remarks>
<Package("ggplot")>
Public Module Rscript

    ''' <summary>
    ''' configs the parameters for do Knn fill of the pixels
    ''' </summary>
    ''' <param name="k"></param>
    ''' <param name="qcut">
    ''' the query block area percentage threshold value, 
    ''' the higher cutoff of this parameter, the less fitting 
    ''' will be perfermen on the pixels, the lower cutoff of 
    ''' this parameter, the more interpolation will be.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("MSI_knnfill")>
    Public Function KnnFill(Optional k As Integer = 3, Optional qcut As Double = 0.85) As MSIKnnFillOption
        Return New MSIKnnFillOption With {
            .k = k,
            .qcut = qcut
        }
    End Function

    ''' <summary>
    ''' options for gauss filter of the MS-imaging buffer
    ''' </summary>
    ''' <param name="levels"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' the gauss blur function is not working well on the linux platform
    ''' </remarks>
    <ExportAPI("MSI_gaussblur")>
    Public Function gaussBlurOpt(Optional levels As Integer = 30) As MSIGaussBlurOption
        Return New MSIGaussBlurOption With {
            .blurLevels = levels
        }
    End Function

    <ExportAPI("MSI_hqx")>
    Public Function hqx_opts(<RRawVectorArgument(TypeCodes.integer)> Optional hqx As Object = "1,2,3,4", Optional env As Environment = Nothing) As MSIHqxScaleOption
        Dim hqx_ints = CLRVector.asInteger(hqx)
        Dim opt = hqx_ints.ElementAtOrDefault(0, [default]:=4)

        If opt <> 1 AndAlso opt <> 2 AndAlso opt <> 3 AndAlso opt <> 4 Then
            opt = 4
            env.AddMessage("invalid hqx scale options, set to HQX_4 by default.")
        End If

        Return New MSIHqxScaleOption With {.hqx = opt}
    End Function

    ''' <summary>
    ''' options for config the canvas dimension size of the ms-imaging raw data scans
    ''' </summary>
    ''' <param name="dims"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("MSI_dimension")>
    <RApiReturn(GetType(MSIDimensionSizeOption))>
    Public Function ConfigMSIDimensionSize(<RRawVectorArgument>
                                           dims As Object,
                                           Optional env As Environment = Nothing) As Object

        Dim size = InteropArgumentHelper.getSize(dims, env, [default]:="0,0")
        Dim dimVals As Size = size.SizeParser

        Return New MSIDimensionSizeOption With {
            .dimension_size = dimVals
        }
    End Function

    ''' <summary>
    ''' create a pixel point pack object for create ggplot
    ''' </summary>
    ''' <param name="pixels">
    ''' A pixel point vector for create a data pack
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("pixelPack")>
    <RApiReturn(GetType(PointPack))>
    Public Function createPixelPack(pixels As MsImaging.PixelData(),
                                    <RRawVectorArgument(GetType(Integer))>
                                    Optional dims As Object = "0,0",
                                    Optional env As Environment = Nothing) As Object

        Dim size = InteropArgumentHelper.getSize(dims, env, [default]:="0,0")
        Dim dimVals As Size = size.SizeParser

        Return New PointPack With {
            .pixels = pixels,
            .dimension = dimVals
        }
    End Function

    ''' <summary>
    ''' create R,G,B layers from the given dataframe columns data
    ''' </summary>
    ''' <param name="matrix"></param>
    ''' <param name="R"></param>
    ''' <param name="G"></param>
    ''' <param name="B"></param>
    ''' <param name="env"></param>
    ''' <returns>
    ''' this function generate the data source object for the ggplot
    ''' </returns>
    <ExportAPI("MSIheatmap")>
    <RApiReturn(GetType(MSIHeatMap))>
    Public Function CreateMSIheatmap(R As Object,
                                     Optional G As Object = Nothing,
                                     Optional B As Object = Nothing,
                                     Optional matrix As dataframe = Nothing,
                                     <RRawVectorArgument>
                                     Optional dims As Object = "0,0",
                                     Optional env As Environment = Nothing) As Object

        Dim size = InteropArgumentHelper.getSize(dims, env, [default]:="0,0")
        Dim dimVals As Size = size.SizeParser

        If matrix Is Nothing Then
            Return MSIHeatMap.UnionLayers(
                layerR:=R,
                layerG:=G,
                layerB:=B,
                dims:=dimVals
            )
        Else
            R = any.ToString(R)
            G = any.ToString(G)
            B = any.ToString(B)
        End If

        Dim missingLayer =
            Function(layer As String) As Boolean
                Return layer.StringEmpty OrElse Not matrix.hasName(layer)
            End Function

        If missingLayer(R) Then
            Return RInternal.debug.stop(New MissingPrimaryKeyException("missing of the basic heatmap layer key!"), env)
        ElseIf matrix.rownames.IsNullOrEmpty Then
            Return RInternal.debug.stop(New MissingFieldException("no pixels data, you should assign the pixel points to the row names!"), env)
        End If

        Dim pixels As Point() = matrix.rownames _
            .Select(Function(pt)
                        Dim t As Integer() = pt _
                            .Split(","c) _
                            .Select(AddressOf Integer.Parse) _
                            .ToArray

                        Return New Point(t(0), t(1))
                    End Function) _
            .ToArray

        If dimVals.IsEmpty Then
            Dim maxWidth As Integer = Aggregate pt As Point In pixels Into Max(pt.X)
            Dim maxHeight As Integer = Aggregate pt As Point In pixels Into Max(pt.Y)

            dimVals = New Size(maxWidth, maxHeight)
        End If

        Return New MSIHeatMap With {
            .R = MSIHeatMap.CreateLayer(R, pixels, CLRVector.asNumeric(matrix(R))),
            .B = If(missingLayer(B), Nothing, MSIHeatMap.CreateLayer(B, pixels, CLRVector.asNumeric(matrix(B)))),
            .G = If(missingLayer(G), Nothing, MSIHeatMap.CreateLayer(G, pixels, CLRVector.asNumeric(matrix(G)))),
            .dimension = dimVals
        }
    End Function

    Private Function unionlayers(layers As IEnumerable(Of ggplotLayer)) As IEnumerable(Of ggplotLayer)
        Dim all As ggplotLayer() = layers.ToArray

        If all.Any(Function(l) TypeOf l Is MSIChannelLayer) Then
            Dim list As New List(Of ggplotLayer)(all)
            Dim union As New MSIRGBCompositionLayer With {
                .red = getChannel(list, MSIChannelLayer.Channels.Red),
                .blue = getChannel(list, MSIChannelLayer.Channels.Blue),
                .green = getChannel(list, MSIChannelLayer.Channels.Green)
            }

            list.Add(union)
            union.zindex = union.MeanZIndex

            If Not union.red Is Nothing Then list.Remove(union.red)
            If Not union.green Is Nothing Then list.Remove(union.green)
            If Not union.blue Is Nothing Then list.Remove(union.blue)

            Return list.OrderBy(Function(layer) layer.zindex)
        Else
            Return all
        End If
    End Function

    Private Function getChannel(all As IEnumerable(Of ggplotLayer), channel As MSIChannelLayer.Channels) As MSIChannelLayer
        Return (From layer As ggplotLayer
                In all
                Where TypeOf layer Is MSIChannelLayer
                Where DirectCast(layer, MSIChannelLayer).channel = channel
                Select layer).FirstOrDefault
    End Function

    ''' <summary>
    ''' create a new ms-imaging heatmap layer
    ''' </summary>
    ''' <param name="layer">
    ''' value of this parameter can be:
    ''' 
    ''' 1. nothing: means rgb heatmap layer
    ''' 2. Total: means TIC
    ''' 3. BasePeak: means BPC
    ''' 4. Average: means average ions
    ''' 
    ''' </param>
    ''' <param name="colors">
    ''' the color scaler name for the heatmap rendering, this 
    ''' parameter only works when the <paramref name="layer"/> 
    ''' parameter value is not nothing.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("geom_msiheatmap")>
    <RApiReturn(GetType(ggplotLayer))>
    Public Function geom_msiheatmap(Optional layer As IntensitySummary? = Nothing,
                                    Optional colors As Object = "viridis:turbo",
                                    Optional env As Environment = Nothing) As Object
        If layer Is Nothing Then
            Return New MSIRGBHeatMapLayer With {
                .raster = If(TypeOf colors Is Bitmap OrElse TypeOf colors Is Image, CType(colors, Image), Nothing)
            }
        Else
            Dim colorSet = RColorPalette.getColorSet(colors, [default]:="viridis:turbo")

            Return New MSITICOverlap With {
                .summary = layer,
                .colorMap = ggplotColorMap.CreateColorMap(colorSet, 1, env)
            }
        End If
    End Function

    ''' <summary>
    ''' rendering a gdi+ heatmap for create raster annotation in ggplot layer
    ''' </summary>
    ''' <param name="pixels">the pixels data</param>
    ''' <param name="dims">the spatial dimension size of the sample data</param>
    ''' <param name="scale">the color palette name</param>
    ''' <param name="levels">the color scaler levels</param>
    ''' <param name="env"></param>
    ''' <returns>
    ''' a gdi+ raster image
    ''' </returns>
    <ExportAPI("raster_blending")>
    Public Function raster_blending(pixels As PixelScanIntensity(), <RRawVectorArgument> dims As Object,
                                    Optional scale As String = "gray",
                                    Optional levels As Integer = 255,
                                    Optional filters As RasterPipeline = Nothing,
                                    Optional env As Environment = Nothing) As Bitmap

        Dim dimSize = InteropArgumentHelper.getSize(dims, env, "0,0").SizeParser

        If dimSize.IsEmpty Then
            dimSize = New Polygon2D(pixels).GetSize
        End If

        If Not filters Is Nothing Then
            pixels = filters _
                .DoIntensityScale(pixels.CreatePixelData(Of PixelData), dimSize) _
                .ExtractPixels _
                .ToArray
        End If

        Dim raster = Drawer.RenderSummaryLayer(
            layer:=pixels,
            dimension:=dimSize,
            colorSet:=scale,
            mapLevels:=levels
        ).AsGDIImage
        Dim scaleRaster As New RasterScaler(raster)

        Return scaleRaster.Scale(hqx:=hqx.HqxScales.Hqx_4x)
    End Function

    <ExportAPI("geom_cmyk")>
    <RApiReturn(GetType(MSICMYKCompositionLayer))>
    Public Function geom_cmyk(<RRawVectorArgument> c As Object,
                              <RRawVectorArgument> m As Object,
                              <RRawVectorArgument> y As Object,
                              <RRawVectorArgument> k As Object,
                              Optional tolerance As Object = "da:0.1",
                              Optional env As Environment = Nothing) As Object

        Dim cl = MsSpectrumData.GetPeakAnnotation(c, env)
        Dim ml = MsSpectrumData.GetPeakAnnotation(m, env)
        Dim yl = MsSpectrumData.GetPeakAnnotation(y, env)
        Dim kl = MsSpectrumData.GetPeakAnnotation(k, env)
        Dim mzdiff = Math.getTolerance(tolerance, env, [default]:="da:0.1")

        If mzdiff Like GetType(Message) Then
            Return mzdiff.TryCast(Of Message)
        End If

        If cl Like GetType(Message) Then
            Return cl.TryCast(Of Message)
        ElseIf ml Like GetType(Message) Then
            Return ml.TryCast(Of Message)
        ElseIf yl Like GetType(Message) Then
            Return yl.TryCast(Of Message)
        ElseIf kl Like GetType(Message) Then
            Return kl.TryCast(Of Message)
        End If

        Return New MSICMYKCompositionLayer With {
            .cyan = cl,
            .magenta = ml,
            .yellow = yl,
            .key = kl,
            .mzdiff = mzdiff
        }
    End Function

    ''' <summary>
    ''' Do ms-imaging based on a set of given metabolite ions m/z
    ''' </summary>
    ''' <param name="mz">A set of target ion m/z values for do imaging</param>
    ''' <param name="tolerance">
    ''' the mass tolerance error vaue for load intensity 
    ''' data for each pixels from the raw data files.
    ''' </param>
    ''' <param name="pixel_render"></param>
    ''' <param name="TrIQ">
    ''' the intensity cutoff threshold value for the target 
    ''' ion layer use TrIQ algorithm.
    ''' </param>
    ''' <param name="color">
    ''' the color set name
    ''' </param>
    ''' <param name="knnFill"></param>
    ''' <param name="raster">
    ''' the raster annotation image to overlaps, this parameter works when
    ''' the <paramref name="pixel_render"/> is set to value TRUE.
    ''' </param>
    ''' <param name="clamp">
    ''' the custom intensity range for make ion layer ms-imaging intensity clamp operation.
    ''' value of this parameter should be a numeric vector with [min,max] of the intensity range
    ''' for make numeric value clamp before the heatmap rendering.
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("geom_msimaging")>
    <RApiReturn(GetType(ggplotLayer), GetType(MSImagingLayer))>
    Public Function geom_msimaging(mz As Double(),
                                   Optional tolerance As Object = "da:0.1",
                                   Optional pixel_render As Boolean = False,
                                   Optional TrIQ As Double = 0.99,
                                   <RRawVectorArgument>
                                   Optional color As Object = "viridis:turbo",
                                   Optional knnFill As Boolean = True,
                                   Optional colorLevels As Integer = 120,
                                   Optional raster As Object = Nothing,
                                   <RRawVectorArgument>
                                   Optional clamp As Object = Nothing,
                                   Optional env As Environment = Nothing) As Object

        Dim mzdiff = Math.getTolerance(tolerance, env)
        Dim colors As ggplotColorMap = Nothing
        Dim rasterLayer As Bitmap = InteropArgumentHelper.getRasterImage(raster)

        If Not color Is Nothing Then
            colors = ggplotColorMap.CreateColorMap(
                map:=color,
                alpha:=1,
                env:=env
            )
        End If

        If mzdiff Like GetType(Message) Then
            Return mzdiff.TryCast(Of Message)
        End If

        Return New MSImagingLayer With {
            .reader = New ggplotReader With {
                .args = New list With {
                    .slots = New Dictionary(Of String, Object) From {
                        {"mz", mz},
                        {"mzdiff", mzdiff.TryCast(Of Tolerance)},
                        {"knnfill", knnFill}
                    }
                }
            },
            .pixelDrawer = pixel_render,
            .TrIQ = TrIQ,
            .colorMap = colors,
            .colorLevels = colorLevels,
            .alpha = 1,
            .raster = rasterLayer,
            .IntensityRange = CLRVector.asNumeric(clamp)
        }
    End Function

    ''' <summary>
    ''' Draw ruler overlaps of the ms-imaging
    ''' </summary>
    ''' <param name="color"></param>
    ''' <param name="width">
    ''' the ruler width on the imaging plot, unit of this parameter value is ``um``.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("geom_MSIruler")>
    <RApiReturn(GetType(MSIRuler))>
    Public Function geom_MSIruler(<RRawVectorArgument>
                                  Optional color As Object = "white",
                                  Optional width As Double? = Nothing) As Object

        Return New MSIRuler With {
            .width = width,
            .color = RColorPalette.GetRawColor(color, [default]:="white")
        }
    End Function

    ''' <summary>
    ''' Create a plot layer of outline for the sample data
    ''' </summary>
    ''' <param name="region">
    ''' The region data for the sample region outline drawing, data value should be a scibasic.net <see cref="GeneralPath"/>
    ''' path data object, or a dataframe object that contains the data fields of ``x`` and ``y`` for store the region 
    ''' spot data and the outline will be computed from this spot data points collection.
    ''' </param>
    ''' <param name="threshold">
    ''' the intensity threshold for make spatial spot binariation, for clean sample,
    ''' leaves this parameter default zero, for sample with background, set this 
    ''' parameter in range (0,1) for make spot cutoff.
    ''' </param>
    ''' <param name="scale">contour tracing pixel rectangle scale size</param>
    ''' <param name="degree"></param>
    ''' <param name="resolution"></param>
    ''' <param name="q"></param>
    ''' <param name="line_stroke">a <see cref="lineElement"/> that create via the ggplot function: ``element_line``.</param>
    ''' <returns></returns>
    ''' <example>
    ''' let region_spots = read.csv("/path/to/region.csv", row.names = NULL, check.names = FALSE);
    ''' 
    ''' print(region_spots, max.print = 6);
    ''' 
    ''' # the region outline will be computed when do 
    ''' # the ms-imaging plot
    ''' # create ggplot layer for ms-imaging
    ''' geom_sample_outline(region = region_spots,
    '''     threshold = 0,
    '''     scale = 5,
    '''     degree = 20,
    '''     resolution = 1000,
    '''     q = 0.1,
    '''     line_stroke = "stroke: blue; stroke-width: 9px; stroke-dash: solid;");
    '''     
    ''' # make region outline pre-computed and cached
    ''' # if there is a for loop for used for make batch drawing of the ms-imaging
    ''' # on the same sample slide data.
    ''' require(graphics2D);
    ''' 
    ''' let region_shape = graphics2D::contour_tracing(region_spots$x, region_spots$y);
    ''' let ions = [];
    ''' 
    ''' for(let mzi in ions) {
    '''    # use the cache data in batch drawing
    '''    geom_sample_outline(region = region_shape,
    '''       threshold = 0,
    '''       scale = 5,
    '''       degree = 20,
    '''       resolution = 1000,
    '''       q = 0.1,
    '''       line_stroke = "stroke: blue; stroke-width: 9px; stroke-dash: solid;");
    ''' }
    ''' </example>
    <ExportAPI("geom_sample_outline")>
    <RApiReturn(GetType(MSISampleOutline))>
    Public Function geom_sample_outline(<RRawVectorArgument>
                                        Optional region As Object = Nothing,
                                        Optional threshold As Double = 0,
                                        Optional scale As Integer = 5,
                                        Optional degree As Single = 20,
                                        Optional resolution As Integer = 1000,
                                        Optional q As Double = 0.1,
                                        Optional line_stroke As Object = "stroke: white; stroke-width: 6px; stroke-dash: solid;",
                                        Optional env As Environment = Nothing) As Object

        Dim line As Stroke = ggplotExtensions.GetStroke(line_stroke, "stroke: white; stroke-width: 6px; stroke-dash: solid;")
        Dim outline As New MSISampleOutline With {
            .line_stroke = line,
            .contour_scale = scale,
            .degree = degree,
            .q = q,
            .resolution = resolution
        }

        If Not region Is Nothing Then
            If TypeOf region Is dataframe Then
                Dim df As dataframe = region
                Dim x As Integer() = CLRVector.asInteger(df.getBySynonym("x", "X"))
                Dim y As Integer() = CLRVector.asInteger(df.getBySynonym("y", "Y"))

                outline.spots = x _
                    .Select(Function(xi, i) New Point(xi, y(i))) _
                    .ToArray
            ElseIf TypeOf region Is GeneralPath Then
                ' is pre-computed graphics path
                ' use this path for region shape drawing directly
                outline.precomputed = DirectCast(region, GeneralPath)
            Else
                Return Message.InCompatibleType(GetType(GeneralPath), region.GetType, env)
            End If
        End If

        Return outline
    End Function

    ''' <summary>
    ''' config of the background of the MS-imaging charting plot.
    ''' </summary>
    ''' <param name="background">
    ''' the background color value or character vector ``TIC`` or ``BPC``.
    ''' </param>
    ''' <returns>
    ''' this function returns clr object in types based on the <paramref name="background"/> parameter:
    ''' 
    ''' 1. "TIC" or "BPC": <see cref="MSITICOverlap"/>
    ''' 2. html color code, or supported gdi image its file path: <see cref="MSIBackgroundOption"/>
    ''' 
    ''' </returns>
    <ExportAPI("geom_MSIbackground")>
    <RApiReturn(GetType(MSIBackgroundOption), GetType(MSITICOverlap))>
    Public Function geom_MSIbackground(background As Object) As Object
        If TypeOf background Is String AndAlso (CStr(background).ToUpper = "TIC" OrElse CStr(background).ToUpper = "BPC") Then
            Return New MSITICOverlap With {
                .summary = If(
                    CStr(background).ToUpper = "TIC",
                    IntensitySummary.Total,
                    IntensitySummary.BasePeak
                )
            }
        Else
            Return New MSIBackgroundOption With {
                .background = background
            }
        End If
    End Function

    ''' <summary>
    ''' Options for apply the filter pieline on the imaging outputs
    ''' </summary>
    ''' <param name="filters"></param>
    ''' <param name="file">
    ''' this function also could read the filter pipeline file for construct the raster pipeline
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("geom_MSIfilters")>
    <RApiReturn(GetType(MSIFilterPipelineOption))>
    Public Function geom_MSIfilters(<RLazyExpression>
                                    <RRawVectorArgument>
                                    Optional filters As Object = Nothing,
                                    <RRawVectorArgument>
                                    Optional file As Object = Nothing,
                                    Optional env As Environment = Nothing) As Object

        If TypeOf filters Is BinaryExpression Then
            Dim pip As Object = BuildPipeline(filters, env, New RasterPipeline)

            If TypeOf pip Is Message Then
                Return pip
            Else
                Return New MSIFilterPipelineOption With {
                   .pipeline = pip
                }
            End If
        ElseIf TypeOf filters Is FunctionInvoke Then
            Dim eval As Object = DirectCast(filters, FunctionInvoke).Evaluate(env)

            If TypeOf eval Is Message Then
                Return eval
            End If

            Return New MSIFilterPipelineOption With {
                .pipeline = New RasterPipeline().Then(eval)
            }
        ElseIf TypeOf filters Is VectorLiteral Then
            Dim eval = DirectCast(filters, VectorLiteral).FromVector(env)

            If eval Like GetType(Message) Then
                Return eval.TryCast(Of Message)
            Else
                Return New MSIFilterPipelineOption With {
                    .pipeline = eval.TryCast(Of RasterPipeline)
                }
            End If
        ElseIf Not filters Is Nothing Then
            Dim val As Object = DirectCast(filters, Expression).Evaluate(env)

            If Program.isException(val) Then
                Return val
            End If

            If val Is Nothing Then
                Return RInternal.debug.stop("input filter could not be nothing", env)
            ElseIf val.GetType.IsArray Then
                Return New MSIFilterPipelineOption With {.pipeline = BuildFilters.FromArray(val)}
            ElseIf TypeOf val Is RasterPipeline Then
                Return New MSIFilterPipelineOption With {.pipeline = val}
            ElseIf TypeOf val Is Scaler Then
                Return New MSIFilterPipelineOption With {.pipeline = New RasterPipeline(DirectCast(val, Scaler))}
            Else
                Return New MSIFilterPipelineOption With {.pipeline = BuildFilters.FromArray(renv.asVector(Of Object)(val))}
            End If
        ElseIf Not file Is Nothing Then
            Dim buf = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Read, env)

            If buf Like GetType(Message) Then
                Return buf.TryCast(Of Message)
            End If

            Return New MSIFilterPipelineOption With {.pipeline = BuildFilters.FromFile(buf.TryCast(Of Stream))}
        Else
            Return Message.InCompatibleType(GetType(BinaryExpression), filters.GetType, env)
        End If
    End Function

    ''' <summary>
    ''' Draw a ion m/z layer with a specific color channel
    ''' </summary>
    ''' <param name="mz"></param>
    ''' <param name="color">
    ''' this parameter usually be used for the r/g/b triple layer overlaps
    ''' </param>
    ''' <param name="tolerance"></param>
    ''' <param name="pixel_render"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("geom_color")>
    <RApiReturn(GetType(ggplotLayer), GetType(MSIChannelLayer))>
    Public Function geom_color(mz As Double, color As Object,
                               Optional tolerance As Object = "da:0.1",
                               Optional pixel_render As Boolean = False,
                               Optional env As Environment = Nothing) As Object

        Dim mzdiff = Math.getTolerance(tolerance, env)

        If mzdiff Like GetType(Message) Then
            Return mzdiff.TryCast(Of Message)
        Else
            ggplot.ggplot.UnionGgplotLayers = AddressOf unionlayers
        End If

        Return New MSIChannelLayer With {
            .reader = New ggplotReader With {
                .args = New list With {
                    .slots = New Dictionary(Of String, Object) From {
                        {"mz", mz},
                        {"mzdiff", mzdiff.TryCast(Of Tolerance)}
                    }
                }
            },
            .pixelDrawer = pixel_render,
            .colorMap = New ggplotColorLiteral With {
                .colorMap = color
            }
        }
    End Function
End Module
