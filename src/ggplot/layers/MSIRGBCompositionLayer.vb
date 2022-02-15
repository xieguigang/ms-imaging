#Region "Microsoft.VisualBasic::7aa19e2342db3f9b48508990830505b4, Rscript\Library\MSI_app\src\ggplot\MSIRGBCompositionLayer.vb"

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

' Class MSIRGBCompositionLayer
' 
'     Properties: blue, green, red
' 
'     Function: Plot
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Imaging
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging

Namespace layers

    Public Class MSIRGBCompositionLayer : Inherits ggplotMSILayer

        Public Property red As ggplotMSILayer
        Public Property blue As ggplotMSILayer
        Public Property green As ggplotMSILayer

        Public ReadOnly Property MeanZIndex As Integer
            Get
                Return Aggregate layer As ggplotMSILayer
                       In {red, blue, green}
                       Where Not layer Is Nothing
                       Into Average(layer.zindex)
            End Get
        End Property

        Private Shared Function getDimSize(redLayer As SingleIonLayer, greenLayer As SingleIonLayer, blueLayer As SingleIonLayer) As Size
            Dim dimSizes As Size() = (From layer As SingleIonLayer
                                      In {redLayer, greenLayer, blueLayer}
                                      Where Not layer Is Nothing
                                      Select layer.DimensionSize).ToArray
            Dim w As Integer = (Aggregate [dim] As Size In dimSizes Into Max([dim].Width))
            Dim h As Integer = (Aggregate [dim] As Size In dimSizes Into Max([dim].Height))

            Return New Size(w, h)
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim MSI As Image
            Dim ggplot As ggplot.ggplot = stream.ggplot
            Dim engine As Renderer = If(pixelDrawer, New PixelRender, New RectangleRender)
            Dim redLayer As SingleIonLayer = DirectCast(red, MSIChannelLayer)?.getIonlayer(ggplot)
            Dim greenLayer As SingleIonLayer = DirectCast(green, MSIChannelLayer)?.getIonlayer(ggplot)
            Dim blueLayer As SingleIonLayer = DirectCast(blue, MSIChannelLayer)?.getIonlayer(ggplot)
            Dim cut As IQuantizationThreshold = AddressOf If(threshold, New TrIQThreshold).ThresholdValue
            Dim qcutRed As DoubleRange = {0, cut(redLayer?.GetIntensity)}
            Dim qcutGreen As DoubleRange = {0, cut(greenLayer?.GetIntensity)}
            Dim qcutBlue As DoubleRange = {0, cut(blueLayer?.GetIntensity)}
            Dim dims As Size = getDimSize(redLayer, greenLayer, blueLayer)

            MSI = engine.ChannelCompositions(
                R:=redLayer,
                G:=greenLayer,
                B:=blueLayer,
                dimension:=dims,
                dimSize:=Nothing,
                cut:=(qcutRed, qcutGreen, qcutBlue),
                background:=stream.theme.gridFill
            )
            MSI = Drawer.ScaleLayer(MSI, rect.Width, rect.Height, InterpolationMode.Bilinear)

            Call stream.g.DrawImage(MSI, rect)

            Return New legendGroupElement With {
                .legends = (From legend As LegendObject In {
                    legend("red", stream.theme, redLayer),
                    legend("green", stream.theme, greenLayer),
                    legend("blue", stream.theme, blueLayer)
                } Where Not legend Is Nothing).ToArray
            }
        End Function

        Private Shared Function legend(color As String, theme As Theme, layer As SingleIonLayer) As LegendObject
            If layer Is Nothing Then
                Return Nothing
            Else
                Return New LegendObject With {
                    .color = color,
                    .fontstyle = theme.legendLabelCSS,
                    .style = LegendStyles.Square,
                    .title = SingleIonLayer.ToString(layer)
                }
            End If
        End Function
    End Class

End Namespace