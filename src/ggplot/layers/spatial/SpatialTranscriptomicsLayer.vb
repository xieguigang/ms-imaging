Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.TissueMorphology
Imports ggplot
Imports ggplot.colors
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports SMRUCC.genomics.Analysis.HTS.DataFrame

Namespace layers.spatial

    Public Class ggplotSpatialTranscriptomicsLayer : Inherits ggplotLayer

        Public Property spots As SpotMap()
        Public Property STdata As MatrixViewer
        Public Property geneID As String
        Public Property label As String
        Public Property ordinal As Integer

        Private Function loadSpots() As Dictionary(Of String, Double)
            Dim type As IntensitySummary? = Nothing
            Dim spotVals As New Dictionary(Of String, Double)
            Dim data As Double

            Select Case geneID.ToLower
                Case "sum" : type = IntensitySummary.Total
                Case "max" : type = IntensitySummary.BasePeak
                Case "avg", "mean" : type = IntensitySummary.Average
            End Select

            For Each spot As SpotMap In spots
                Dim spotData As Double() = STdata.GetGeneExpression(spot.barcode)

                If type Is Nothing Then
                    ' rendering for a specific gene id
                    ' which is indexed via ordinal in the matrix column
                    data = spotData(ordinal)
                Else
                    Select Case type.Value
                        Case IntensitySummary.Average : data = spotData.Average
                        Case IntensitySummary.BasePeak : data = spotData.Max
                        Case IntensitySummary.Total : data = spotData.Sum
                    End Select
                End If

                Call spotVals.Add(spot.barcode, data)
            Next

            Return spotVals
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim dimension_size As Size = ggplot.GetDimensionSize(Nothing)
            ' create a new transparent layer on
            ' current ms-imaging render layer
            Dim factor As Double = 4
            Dim layer As Graphics2D = dimension_size.Scale(factor).CreateGDIDevice(filled:=Color.Transparent)
            Dim data As Dictionary(Of String, Double) = loadSpots()
            Dim colors = DirectCast(colorMap, ggplotColorPalette).ColorHandler(ggplot, data.Values.ToArray)

            For Each spot As SpotMap In spots
                Dim poly As New Polygon2D(spot.SMX, spot.SMY)
                Dim val As Double = data(spot.barcode)
                Dim fill As Brush = colors(val).GetBrush
                Dim shape1 = poly.GetRectangle
                Dim pos As New PointF(
                    x:=shape1.Left * factor,
                    y:=shape1.Top * factor
                )
                Dim size As New SizeF(
                    width:=shape1.Width * factor,
                    height:=shape1.Height * factor
                )
                Dim shape2 As New RectangleF With {
                    .Location = pos,
                    .Size = size
                }

                Call layer.FillEllipse(fill, shape2)
            Next

            Call layer.Flush()
            Call stream.g.DrawImage(layer.ImageResource, rect)

            Return Nothing
        End Function
    End Class
End Namespace