Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers

Namespace layers

    Public Class MSIRuler : Inherits ggplotLayer

        Public Property color As Color

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim reader As PixelReader = base.reader
            Dim resolution As Double = reader.resolution
            Dim dimsize As Size = ggplot.GetDimensionSize(reader.dimension)
            Dim rect As Rectangle = stream.canvas.PlotRegion

            Call New Ruler(stream.theme).DrawOnCanvas(
                g:=stream.g,
                dimsize:=dimsize,
                rect:=rect,
                color:=color,
                resolution:=resolution
            )

            Return Nothing
        End Function
    End Class
End Namespace