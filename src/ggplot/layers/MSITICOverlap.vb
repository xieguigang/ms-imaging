Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplot
Imports ggplot.colors
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.hqx
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports any = Microsoft.VisualBasic.Scripting

Namespace layers

    Public Class MSITICOverlap : Inherits ggplotLayer

        Public Property summary As IntensitySummary

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim reader As PixelReader = base.reader
            Dim summary As MSISummary = reader.GetSummary
            Dim pixels As PixelData() = summary.GetLayer(Me.summary) _
                .Where(Function(p) p.totalIon > 0) _
                .Select(Function(p)
                            Return New PixelData With {
                                .x = p.x,
                                .y = p.y,
                                .intensity = p.totalIon
                            }
                        End Function) _
                .ToArray

            Dim colorSet As String = "gray"

            If Not colorMap Is Nothing Then
                If TypeOf colorMap Is ggplotColorPalette Then
                    colorSet = any.ToString(DirectCast(colorMap, ggplotColorPalette).colorMap)
                End If
            End If

            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim black = reader.dimension.CreateGDIDevice(filled:=Color.Black).ImageResource
            Dim TIC As Bitmap = New RectangleRender(Drivers.Default, False).RenderPixels(
                pixels:=pixels,
                dimension:=ggplot.GetDimensionSize(reader.dimension),
                colorSet:=colorSet,
                mapLevels:=250,
                defaultFill:="black"
            ).AsGDIImage _
             .DoCall(Function(img) New Bitmap(img))

            TIC = New Drawing2D.HeatMap.RasterScaler(TIC).Scale(hqx:=HqxScales.Hqx_4x)

            stream.theme.gridFill = "transparent"
            stream.g.DrawImage(black, rect)
            stream.g.DrawImage(TIC, rect)

            Return Nothing
        End Function
    End Class
End Namespace