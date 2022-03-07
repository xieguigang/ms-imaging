Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging

Namespace layers

    Public Class MSIHeatMapLayer : Inherits ggplotMSILayer

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim MSI As Image
            Dim ggplot As ggplot.ggplot = stream.ggplot
            Dim data = DirectCast(ggplot.data, MSIHeatMap)
            Dim engine As New RectangleRender(ggplot.driver, heatmapRender:=True)
            Dim redLayer As SingleIonLayer = data.R
            Dim greenLayer As SingleIonLayer = data.G
            Dim blueLayer As SingleIonLayer = data.B
            Dim cut As IQuantizationThreshold = AddressOf If(threshold, New TrIQThreshold).ThresholdValue
            Dim qcutRed As DoubleRange = {0, cut(redLayer?.GetIntensity)}
            Dim qcutGreen As DoubleRange = {0, cut(greenLayer?.GetIntensity)}
            Dim qcutBlue As DoubleRange = {0, cut(blueLayer?.GetIntensity)}
            Dim dims As Size = data.dimension

            MSI = engine.ChannelCompositions(
                R:=redLayer,
                G:=greenLayer,
                B:=blueLayer,
                dimension:=dims,
                dimSize:=New Size(16, 16),
                cut:=(qcutRed, qcutGreen, qcutBlue),
                background:=stream.theme.gridFill
            ).AsGDIImage
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