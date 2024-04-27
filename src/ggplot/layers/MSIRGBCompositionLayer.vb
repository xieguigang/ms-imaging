#Region "Microsoft.VisualBasic::764bf83f7186e089ddf0c0526a751393, G:/mzkit/Rscript/Library/MSI_app/src//ggplot/layers/MSIRGBCompositionLayer.vb"

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

    '   Total Lines: 106
    '    Code Lines: 82
    ' Comment Lines: 10
    '   Blank Lines: 14
    '     File Size: 4.61 KB


    '     Class MSIRGBCompositionLayer
    ' 
    '         Properties: blue, green, MeanZIndex, red
    ' 
    '         Function: getDimSize, legend, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace layers

    ''' <summary>
    ''' The rgb composition rendering layer
    ''' </summary>
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

        ''' <summary>
        ''' Try get the dimension size of the canvas based on the layer data
        ''' </summary>
        ''' <param name="redLayer"></param>
        ''' <param name="greenLayer"></param>
        ''' <param name="blueLayer"></param>
        ''' <returns></returns>
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
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim engine As New RectangleRender(ggplot.driver, heatmapRender:=False)
            Dim redLayer As SingleIonLayer = DirectCast(red, MSIChannelLayer)?.getIonlayer(ggplot)
            Dim greenLayer As SingleIonLayer = DirectCast(green, MSIChannelLayer)?.getIonlayer(ggplot)
            Dim blueLayer As SingleIonLayer = DirectCast(blue, MSIChannelLayer)?.getIonlayer(ggplot)
            Dim dims As Size

            If Not ggplot.dimension_size.IsEmpty Then
                dims = ggplot.dimension_size
            Else
                dims = getDimSize(redLayer, greenLayer, blueLayer)
            End If

            redLayer = ApplyRasterFilter(redLayer, ggplot)
            greenLayer = ApplyRasterFilter(greenLayer, ggplot)
            blueLayer = ApplyRasterFilter(blueLayer, ggplot)

            Using buf As Graphics2D = dims.CreateGDIDevice(filled:=Color.Black)
                Call engine.ChannelCompositions(
                    buf,
                    region:=New GraphicsRegion With {.Size = dims, .Padding = Padding.Zero},
                    R:=redLayer,
                    G:=greenLayer,
                    B:=blueLayer,
                    dimension:=dims,
                    background:=stream.theme.gridFill
                )

                stream.g.DrawImage(ScaleImageImpls(buf.ImageResource, stream), rect)
            End Using

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
