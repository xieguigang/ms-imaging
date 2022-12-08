Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
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
            Dim type As IntensitySummary? = Nothing

            Select Case geneID.ToLower
                Case "sum" : type = IntensitySummary.Total
                Case "max" : type = IntensitySummary.BasePeak
                Case "avg", "mean" : type = IntensitySummary.Average
            End Select

            For Each spot As SpotMap In spots
                Dim poly As New Polygon2D(spot.SMX, spot.SMY)
                Dim spotData = STdata.GetGeneExpression(spot.barcode)
                Dim data As Double

                If type Is Nothing Then
                    data = spotData(ordinal)
                Else
                    Select Case type.Value
                        Case IntensitySummary.Average : data = spotData.Average
                        Case IntensitySummary.BasePeak : data = spotData.Max
                        Case IntensitySummary.Total : data = spotData.Sum
                    End Select
                End If

                Call layer.FillEllipse(fill, poly.GetRectangle)
            Next

            Call layer.Flush()
            Call stream.g.DrawImage(layer.ImageResource, rect)

            Return Nothing
        End Function
    End Class
End Namespace