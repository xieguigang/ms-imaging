Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Imaging
Imports ggplot
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports any = Microsoft.VisualBasic.Scripting

Public Class MSImagingLayer : Inherits ggplotLayer

    Public Property pixelDrawer As Boolean = False
    Public Property cutoff As DoubleRange = Nothing

    Public Overrides Function Plot(g As IGraphics,
                                   canvas As GraphicsRegion,
                                   baseData As ggplotData,
                                   x() As Double,
                                   y() As Double,
                                   scale As DataScaler,
                                   ggplot As ggplot.ggplot,
                                   theme As Theme) As IggplotLegendElement

        Dim args = reader.args
        Dim mz As Double = args.getValue(Of Double)("mz", ggplot.environment)
        Dim mzdiff As Tolerance = args.getValue(Of Tolerance)("mzdiff", ggplot.environment)

        If mz <= 0 Then
            Throw New InvalidProgramException("invalid ion m/z value!")
        End If
        If mzdiff Is Nothing Then
            mzdiff = Tolerance.DeltaMass(0.1)
            ggplot.environment.AddMessage("missing 'tolerance' parameter, use the default da:0.1 as mzdiff tolerance value!")
        End If

        Dim rect As Rectangle = canvas.PlotRegion
        Dim base = DirectCast(ggplot.base.reader, MSIReader)
        Dim ion As SingleIonLayer = SingleIonLayer.GetLayer(mz, base.reader, mzdiff)
        Dim MSI As Image
        Dim engine As Renderer = If(pixelDrawer, New PixelRender, New RectangleRender)
        Dim colorSet As String

        If colorMap Is Nothing Then
            colorSet = theme.colorSet
        Else
            colorSet = any.ToString(colorMap.colorMap)
        End If

        MSI = engine.RenderPixels(ion.MSILayer, ion.DimensionSize, Nothing, cutoff:=cutoff, colorSet:=colorSet)
        MSI = Drawer.ScaleLayer(MSI, rect.Width, rect.Height, InterpolationMode.Bilinear)

        Call g.DrawImage(MSI, rect)

        Return New legendColorMapElement With {
            .width = canvas.Padding.Right * (3 / 4),
            .height = rect.Height,
            .colorMapLegend = New ColorMapLegend(colorSet, 100) With {
                .format = "G3",
                .tickAxisStroke = Stroke.TryParse(theme.legendTickAxisStroke).GDIObject,
                .tickFont = CSSFont.TryParse(theme.legendTickCSS).GDIObject(g.Dpi),
                .ticks = ion.GetIntensity.Range.CreateAxisTicks,
                .title = $"m/z {mz.ToString("F3")}",
                .titleFont = CSSFont.TryParse(theme.legendTitleCSS).GDIObject(g.Dpi),
                .noblank = True,
                .legendOffsetLeft = canvas.Padding.Right / 10
            }
        }
    End Function
End Class
