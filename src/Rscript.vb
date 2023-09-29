#Region "Microsoft.VisualBasic::25c213636430d3222caef3913ca468fb, mzkit\Rscript\Library\MSI_app\src\Rscript.vb"

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

'   Total Lines: 444
'    Code Lines: 306
' Comment Lines: 91
'   Blank Lines: 47
'     File Size: 16.33 KB


' Module Rscript
' 
'     Function: BuildPipeline, ConfigMSIDimensionSize, CreateMSIheatmap, createPixelPack, gaussBlurOpt
'               geom_color, geom_MSIbackground, geom_MSIfilters, geom_msiheatmap, geom_msimaging
'               geom_MSIruler, getChannel, KnnFill, unionlayers, unionLayers
' 
' /********************************************************************************/

#End Region

Imports System.Data
Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports ggplot
Imports ggplot.colors
Imports ggplot.layers
Imports ggplotMSImaging.layers
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Closure
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.DataSets
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Operators
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports any = Microsoft.VisualBasic.Scripting

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
            Return Internal.debug.stop(New MissingPrimaryKeyException("missing of the basic heatmap layer key!"), env)
        ElseIf matrix.rownames.IsNullOrEmpty Then
            Return Internal.debug.stop(New MissingFieldException("no pixels data, you should assign the pixel points to the row names!"), env)
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
            Return New MSIRGBHeatMapLayer
        Else
            Dim colorSet = RColorPalette.getColorSet(colors, [default]:="viridis:turbo")

            Return New MSITICOverlap With {
                .summary = layer,
                .colorMap = ggplotColorMap.CreateColorMap(colorSet, 1, env)
            }
        End If
    End Function

    <ExportAPI("raster_blending")>
    Public Function raster_blending(pixels As PixelScanIntensity(), <RRawVectorArgument> dims As Object,
                                    Optional scale As String = "gray",
                                    Optional levels As Integer = 255,
                                    Optional env As Environment = Nothing) As Bitmap

        Dim dimSize = InteropArgumentHelper.getSize(dims, env, "0,0").SizeParser

        If dimSize.IsEmpty Then
            dimSize = New Polygon2D(pixels).GetSize
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
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("geom_msimaging")>
    <RApiReturn(GetType(ggplotLayer))>
    Public Function geom_msimaging(mz As Double(),
                                   Optional tolerance As Object = "da:0.1",
                                   Optional pixel_render As Boolean = False,
                                   Optional TrIQ As Double = 0.99,
                                   <RRawVectorArgument>
                                   Optional color As Object = "viridis:turbo",
                                   Optional knnFill As Boolean = True,
                                   Optional colorLevels As Integer = 120,
                                   Optional raster As Object = Nothing,
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
            .raster = rasterLayer
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
    Public Function geom_MSIruler(<RRawVectorArgument>
                                  Optional color As Object = "white",
                                  Optional width As Double? = Nothing) As Object

        Return New MSIRuler With {
            .width = width,
            .color = RColorPalette.GetRawColor(color, [default]:="white")
        }
    End Function

    ''' <summary>
    ''' config of the background of the MS-imaging charting plot.
    ''' </summary>
    ''' <param name="background">
    ''' the background color value or character vector ``TIC`` or ``BPC``.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("geom_MSIbackground")>
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
    Public Function geom_MSIfilters(<RLazyExpression>
                                    <RRawVectorArgument>
                                    Optional filters As Object = Nothing,
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
    <RApiReturn(GetType(ggplotLayer))>
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
