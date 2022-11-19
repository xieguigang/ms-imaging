Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.MIME.Html.CSS

Namespace layers

    Public Class MSIRuler : Inherits ggplotLayer

        Public Property color As Color

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot = stream.ggplot
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim reader As PixelReader = base.reader
            Dim resolution As Double = reader.resolution
            Dim dimsize As Size = reader.dimension
            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim ratio As Double = rect.Width / dimsize.Width
            Dim rulerWidth As Double = rect.Width * 0.2
            Dim pen As Pen = Stroke.TryParse(stream.theme.lineStroke).GDIObject
            Dim font As Font = CSSFont.TryParse(stream.theme.tagCSS).GDIObject(stream.g.Dpi)
            Dim physical As String = (rulerWidth / ratio * resolution).ToString("F2") & " um"
            Dim fontsize As SizeF = stream.g.MeasureString(physical, font)
            Dim bottom As Double = rect.Bottom - fontsize.Height * 2
            Dim left As New PointF(rect.Left + rect.Width * 0.05, bottom)
            Dim right As New PointF(left.X + rulerWidth, bottom)
            Dim center As New PointF(left.X + (rulerWidth - fontsize.Width) / 2, bottom + 10)

#Disable Warning
            pen.Color = color

            Call stream.g.DrawLine(pen, left, right)
            Call stream.g.DrawString(physical, font, New SolidBrush(color), center)
#Enable Warning

            Return Nothing
        End Function
    End Class
End Namespace