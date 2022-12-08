Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.TissueMorphology
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports SMRUCC.genomics.Analysis.HTS.DataFrame

Namespace layers.spatial

    Public Class ggplotSpatialTranscriptomicsLayer : Inherits ggplotLayer

        Public Property spots As SpotMap()
        Public Property STdata As HTSMatrixReader
        Public Property geneID As String
        Public Property label As String
        Public Property ordinal As Integer

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim dimension_size As Size = ggplot.GetDimensionSize(Nothing)
            ' create a new transparent layer on current ms-imaging render layer
            Dim layer As Graphics2D = dimension_size.CreateGDIDevice(filled:=Color.Transparent)
            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim fill As Brush = Brushes.Red

            For Each spot As SpotMap In spots
                Dim poly As New Polygon2D(spot.SMX, spot.SMY)
                Dim data As Double = STdata.GetGeneExpression(spot.barcode)(ordinal)

                Call layer.FillEllipse(fill, poly.GetRectangle)
            Next

            Call layer.Flush()
            Call stream.g.DrawImage(layer.ImageResource, rect)

            Return Nothing
        End Function
    End Class
End Namespace