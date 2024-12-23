Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace layers

    Public Class MSICMYKCompositionLayer : Inherits ggplotMSILayer

        ''' <summary>
        ''' the raster image background for the TIC overlaps
        ''' </summary>
        ''' <returns></returns>
        Public Property raster As Image

        Public Property cyan As MzAnnotation
        Public Property magenta As MzAnnotation
        Public Property yellow As MzAnnotation
        Public Property key As MzAnnotation
        Public Property mzdiff As Tolerance

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim rect As Rectangle = stream.canvas.PlotRegion(css)
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim engine As New RectangleRender(ggplot.driver, heatmapRender:=False)
            Dim c As SingleIonLayer = getIonlayer(cyan, mzdiff, ggplot)
            Dim m As SingleIonLayer = getIonlayer(magenta, mzdiff, ggplot)
            Dim y As SingleIonLayer = getIonlayer(yellow, mzdiff, ggplot)
            Dim k As SingleIonLayer = getIonlayer(key, mzdiff, ggplot)
            Dim dims As Size

            If Not ggplot.dimension_size.IsEmpty Then
                dims = ggplot.dimension_size
            Else
                dims = MSIRGBCompositionLayer.getDimSize(c, m, y, k)
            End If

            c = ApplyRasterFilter(c, ggplot)
            m = ApplyRasterFilter(m, ggplot)
            y = ApplyRasterFilter(y, ggplot)
            k = ApplyRasterFilter(k, ggplot)

            Using buf As IGraphics = DriverLoad.CreateGraphicsDevice(dims, Color.Black)
                Call engine.ChannelCompositions(
                    buf,
                    region:=New GraphicsRegion With {.Size = dims, .Padding = Padding.Zero},
                    C:=c,
                    M:=m,
                    Y:=y,
                    K:=k,
                    dimension:=dims,
                    background:=stream.theme.gridFill
                )

                stream.g.DrawImage(ScaleImageImpls(DirectCast(buf, GdiRasterGraphics).ImageResource, stream), rect)
            End Using

            Return New legendGroupElement With {
                .legends = (From legend As LegendObject In {
                    MSIRGBCompositionLayer.legend("cyan", stream.theme, c),
                    MSIRGBCompositionLayer.legend("magenta", stream.theme, m),
                    MSIRGBCompositionLayer.legend("yellow", stream.theme, y),
                    MSIRGBCompositionLayer.legend("black", stream.theme, k)
                } Where Not legend Is Nothing).ToArray
            }
        End Function
    End Class
End Namespace