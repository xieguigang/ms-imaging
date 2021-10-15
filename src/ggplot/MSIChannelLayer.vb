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

Public Class MSIChannelLayer : Inherits ggplotMSILayer

    Public Enum Channels
        NA
        Red
        Green
        Blue
    End Enum

    Public ReadOnly Property channel As Channels
        Get
            Dim color As String = DirectCast(colorMap, ggplotColorLiteral).ToColor.ToHtmlColor

            Select Case color.ToLower
                Case "#ff0000" : Return Channels.Red

                Case "#00ff00", "#008000" : Return Channels.Green
                Case "#0000ff" : Return Channels.Blue
                Case Else
                    Return Channels.NA
            End Select
        End Get
    End Property

    Public Overloads Function getIonlayer(ggplot As ggplot.ggplot) As SingleIonLayer
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

        Return getIonlayer(mz, mzdiff, ggplot)
    End Function

    Public Overrides Function Plot(g As IGraphics,
                                   canvas As GraphicsRegion,
                                   baseData As ggplotData,
                                   x() As Double,
                                   y() As Double,
                                   scale As DataScaler,
                                   ggplot As ggplot.ggplot,
                                   theme As Theme) As IggplotLegendElement

        Dim rect As Rectangle = canvas.PlotRegion
        Dim ion As SingleIonLayer = getIonlayer(ggplot)
        Dim MSI As Image
        Dim engine As Renderer = If(pixelDrawer, New PixelRender, New RectangleRender)
        Dim color As String = DirectCast(colorMap, ggplotColorLiteral).ToColor.ToHtmlColor
        Dim colorSet As String = $"transparent,{color}"
        Dim q As DoubleRange = {0, Renderer.AutoCheckCutMax(ion.GetIntensity, 0.8)}

        Select Case color.ToLower
            Case "#ff0000"            ' red
                MSI = engine.ChannelCompositions(ion.MSILayer, {}, {}, ion.DimensionSize, Nothing, cut:=(q, q, q), background:="transparent")
            Case "#00ff00", "#008000" ' green
                MSI = engine.ChannelCompositions({}, ion.MSILayer, {}, ion.DimensionSize, Nothing, cut:=(q, q, q), background:="transparent")
            Case "#0000ff"            ' blue
                MSI = engine.ChannelCompositions({}, {}, ion.MSILayer, ion.DimensionSize, Nothing, cut:=(q, q, q), background:="transparent")
            Case Else
                MSI = engine.RenderPixels(ion.MSILayer, ion.DimensionSize, Nothing, cutoff:=q, colorSet:=colorSet)
        End Select

        MSI = Drawer.ScaleLayer(MSI, rect.Width, rect.Height, InterpolationMode.Bilinear)

        Call g.DrawImage(MSI, rect)

        Return New ggplotLegendElement With {
            .legend = New LegendObject With {
                .color = color,
                .fontstyle = theme.legendLabelCSS,
                .style = LegendStyles.Square,
                .title = $"m/z {ion.IonMz.ToString("F4")}"
            }
        }
    End Function

    Public Overrides Function ToString() As String
        Return $"[{DirectCast(colorMap, ggplotColorLiteral).ToColor.ToHtmlColor}] {reader.args!mz}"
    End Function
End Class
