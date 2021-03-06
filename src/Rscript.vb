#Region "Microsoft.VisualBasic::6d83acd1720d495ac144b22207ec50b5, mzkit\Rscript\Library\MSI_app\src\Rscript.vb"

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

'   Total Lines: 183
'    Code Lines: 155
' Comment Lines: 3
'   Blank Lines: 25
'     File Size: 7.30 KB


' Module Rscript
' 
'     Function: CreateMSIheatmap, gaussBlurOpt, geom_color, geom_MSIbackground, geom_msiheatmap
'               geom_msimaging, getChannel, KnnFill, unionlayers
' 
' /********************************************************************************/

#End Region

#If netcore5 = 1 Then
Imports System.Data
#End If
Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports ggplot
Imports ggplot.colors
Imports ggplot.layers
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports MSImaging.layers
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop
Imports REnv = SMRUCC.Rsharp.Runtime
Imports any = Microsoft.VisualBasic.Scripting

''' <summary>
''' the ggplot api plugin for do MS-Imaging rendering
''' </summary>
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

    <ExportAPI("pixelPack")>
    Public Function createPixelPack(pixels As PixelData()) As PointPack
        Return New PointPack With {.pixels = pixels}
    End Function

    ''' <summary>
    ''' create R,G,B layers from the given dataframe columns data
    ''' </summary>
    ''' <param name="matrix"></param>
    ''' <param name="R"></param>
    ''' <param name="G"></param>
    ''' <param name="B"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("MSIheatmap")>
    Public Function CreateMSIheatmap(R As Object,
                                     Optional G As Object = Nothing,
                                     Optional B As Object = Nothing,
                                     Optional matrix As dataframe = Nothing,
                                     Optional env As Environment = Nothing) As Object

        If matrix Is Nothing Then
            Dim layerR As SingleIonLayer = R
            Dim layerG As SingleIonLayer = G
            Dim layerB As SingleIonLayer = B

            Dim w As Integer = {
                layerR.DimensionSize.Width,
                layerG.DimensionSize.Width,
                layerB.DimensionSize.Width
            }.Max

            Dim h As Integer = {
                layerR.DimensionSize.Height,
                layerG.DimensionSize.Height,
                layerB.DimensionSize.Height
            }.Max

            Dim dims As New Size With {
                .Width = w,
                .Height = h
            }

            Return New MSIHeatMap With {
                .R = R,
                .G = G,
                .B = B,
                .dimension = dims
            }
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
        Dim maxWidth As Integer = Aggregate pt As Point In pixels Into Max(pt.X)
        Dim maxHeight As Integer = Aggregate pt As Point In pixels Into Max(pt.Y)

        Return New MSIHeatMap With {
            .R = MSIHeatMap.CreateLayer(R, pixels, DirectCast(REnv.asVector(Of Double)(matrix(R)), Double())),
            .B = If(missingLayer(B), Nothing, MSIHeatMap.CreateLayer(B, pixels, DirectCast(REnv.asVector(Of Double)(matrix(B)), Double()))),
            .G = If(missingLayer(G), Nothing, MSIHeatMap.CreateLayer(G, pixels, DirectCast(REnv.asVector(Of Double)(matrix(G)), Double()))),
            .dimension = New Size(maxWidth, maxHeight)
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

    <ExportAPI("geom_msiheatmap")>
    <RApiReturn(GetType(ggplotLayer))>
    Public Function geom_msiheatmap() As Object
        Return New MSIHeatMapLayer
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="mz"></param>
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
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("geom_msimaging")>
    <RApiReturn(GetType(ggplotLayer))>
    Public Function geom_msimaging(mz As Double(),
                                   Optional tolerance As Object = "da:0.1",
                                   Optional pixel_render As Boolean = False,
                                   Optional TrIQ As Double = 0.65,
                                   <RRawVectorArgument>
                                   Optional color As Object = "viridis:turbo",
                                   Optional knnFill As Boolean = True,
                                   Optional colorLevels As Integer = 255,
                                   Optional env As Environment = Nothing) As Object

        Dim mzdiff = Math.getTolerance(tolerance, env)
        Dim colors As ggplotColorMap = Nothing

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
            .alpha = 1
        }
    End Function

    ''' <summary>
    ''' config of the background of the MS-imaging charting plot.
    ''' </summary>
    ''' <param name="background"></param>
    ''' <returns></returns>
    <ExportAPI("geom_MSIbackground")>
    Public Function geom_MSIbackground(background As Object) As Object
        Return New MSIBackgroundOption With {
            .background = background
        }
    End Function

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
