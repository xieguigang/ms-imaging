Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D.MarchingSquares
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace layers

    Public Class MSISampleOutline : Inherits ggplotLayer

        Public Property contour_scale As Integer = 5
        Public Property degree As Single = 20
        Public Property resolution As Integer = 1000
        Public Property q As Double = 0.1
        Public Property line_stroke As Stroke
        Public Property threshold As Double = 0

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim stroke As Pen = css.GetPen(line_stroke)
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim reader As PixelReader = base.reader
            Dim summary As MSISummary = reader.GetSummary
            Dim spots As New List(Of iPixelIntensity)

            If threshold > 0 Then
                Throw New NotImplementedException("threshold cutoff")
            Else
                ' use all pixels
                Call spots.AddRange(summary.rowScans.IteratesALL)
            End If

            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim xscale = d3js.scale.linear.domain(values:={0.0, summary.size.Width}).range(values:=New Double() {rect.Left, rect.Right})
            Dim yscale = d3js.scale.linear.domain(values:={0.0, summary.size.Height}).range(values:=New Double() {rect.Top, rect.Bottom})
            Dim scale As New DataScaler() With {
                .AxisTicks = (Nothing, Nothing),
                .region = rect,
                .X = xscale,
                .Y = yscale
            }
            Dim shape As GeneralPath = MSIRegionPlot.MeasureRegionPolygon(
                spots.X.ToArray,
                spots.Y.ToArray,
                contour_scale, degree, resolution, q)

            For Each region As PointF() In shape.GetPolygons
                region = region _
                    .Select(Function(pi)
                                Return scale.Translate(pi)
                            End Function) _
                    .ToArray

                Call stream.g.DrawPolygon(stroke, region)
            Next

            Return Nothing
        End Function
    End Class
End Namespace