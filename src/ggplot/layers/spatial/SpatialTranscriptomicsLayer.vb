﻿#Region "Microsoft.VisualBasic::1da194fc4931520031cea759ad1e566d, Rscript\Library\MSI_app\src\ggplot\layers\spatial\SpatialTranscriptomicsLayer.vb"

    ' Author:
    ' 
    '       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
    ' 
    ' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 159
    '    Code Lines: 127 (79.87%)
    ' Comment Lines: 8 (5.03%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 24 (15.09%)
    '     File Size: 6.86 KB


    '     Class ggplotSpatialTranscriptomicsLayer
    ' 
    '         Properties: geneID, label, ordinal, spots, spotSize
    '                     STdata
    ' 
    '         Function: interpolate, loadSpots, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.TissueMorphology
Imports ggplot
Imports ggplot.colors
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports SMRUCC.genomics.Analysis.HTS.DataFrame
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render




#If NET48 Then
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports FontStyle = Microsoft.VisualBasic.Imaging.FontStyle
#End If

Namespace layers.spatial

    Public Class ggplotSpatialTranscriptomicsLayer : Inherits ggplotLayer

        Public Property spots As SpotMap()
        Public Property STdata As MatrixViewer
        Public Property geneID As String
        Public Property label As String
        Public Property ordinal As Integer
        Public Property spotSize As Double

        Private Function loadSpots() As Dictionary(Of String, Double)
            Dim spotVals As New Dictionary(Of String, Double)

            If STdata Is Nothing Then
                For Each spot As SpotMap In spots
                    spotVals(spot.barcode) = spot.heatmap
                Next
            Else
                Dim type As IntensitySummary? = Nothing
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
            End If

            Return interpolate(spotVals)
        End Function

        Private Function interpolate(spotVals As Dictionary(Of String, Double)) As Dictionary(Of String, Double)
            Dim STdims As New Polygon2D(spots.Select(Function(s) New PointF(s.spotXY(0), s.spotXY(1))).ToArray)
            Dim layer As New SingleIonLayer With {
                .DimensionSize = New Size(STdims.xpoints.Max, STdims.ypoints.Max),
                .IonMz = "STdata",
                .MSILayer = spots _
                    .Select(Function(s)
                                Return New PixelData With {
                                    .intensity = spotVals(s.barcode),
                                    .x = s.spotXY(0),
                                    .y = s.spotXY(1),
                                    .sampleTag = s.barcode
                                }
                            End Function) _
                    .ToArray
            }
            Dim interpo As New SoftenScaler()

            layer = interpo.DoIntensityScale(layer)

            For Each spot As PixelData In layer.MSILayer
                spotVals(spot.sampleTag) = spot.intensity
            Next

            Return spotVals
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim rect As Rectangle = stream.canvas.PlotRegion(css)
            Dim dimension_size As Size = ggplot.GetDimensionSize(Nothing)
            Dim println = stream.ggplot.environment.WriteLineHandler
            ' create a new transparent layer on
            ' current ms-imaging render layer
            Dim layer As IGraphics = DriverLoad.CreateGraphicsDevice(dimension_size.Scale(spotSize), Color.Transparent)
            Dim data As Dictionary(Of String, Double) = loadSpots()
            Dim colors = DirectCast(colorMap, ggplotColorPalette).ColorHandler(ggplot, data.Values.ToArray)
            Dim spotSizes = (From spot As SpotMap
                             In spots
                             Let poly = New Polygon2D(spot.SMX, spot.SMY)
                             Select poly.GetRectangle.Size).ToArray
            'Dim sizeMean As New SizeF With {
            '    .Width = spotSizes.Average(Function(s) s.Width),
            '    .Height = spotSizes.Average(Function(s) s.Height)
            '}
            Dim sizeMean As New SizeF(spotSize, spotSize)
            Dim offset As PointF = DirectCast(ggplot.base.reader, MSIReader).offset

            Call println($"render {label}...")

            For Each spot As SpotMap In spots
                Dim poly As New Polygon2D(spot.SMX, spot.SMY)
                Dim val As Double = data(spot.barcode)
                Dim fill As Brush = colors(val).GetBrush
                Dim shape1 = poly.GetRectangle
                Dim pos As New PointF(
                    x:=(shape1.Left - offset.X) * spotSize,
                    y:=(shape1.Top - offset.Y) * spotSize
                )
                Dim shape2 As New RectangleF With {
                    .Location = pos,
                    .Size = sizeMean
                }

                Call layer.FillEllipse(fill, shape2)
            Next

            Call layer.Flush()
            Call stream.g.DrawImage(DirectCast(layer, GdiRasterGraphics).ImageResource, rect)

            Return Nothing
        End Function
    End Class
End Namespace
