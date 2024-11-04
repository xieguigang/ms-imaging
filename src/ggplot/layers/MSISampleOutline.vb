#Region "Microsoft.VisualBasic::265b76924d2395d63b9be826b340b1cc, Rscript\Library\MSI_app\src\ggplot\layers\MSISampleOutline.vb"

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

    '   Total Lines: 95
    '    Code Lines: 82 (86.32%)
    ' Comment Lines: 1 (1.05%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 12 (12.63%)
    '     File Size: 3.98 KB


    '     Class MSISampleOutline
    ' 
    '         Properties: contour_scale, degree, line_stroke, q, resolution
    '                     spots, threshold
    ' 
    '         Function: getShapes, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D.MarchingSquares
Imports Microsoft.VisualBasic.Linq
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

Namespace layers

    Public Class MSISampleOutline : Inherits ggplotLayer

        Public Property contour_scale As Integer = 5
        Public Property degree As Single = 20
        Public Property resolution As Integer = 1000
        Public Property q As Double = 0.1
        Public Property line_stroke As Stroke
        Public Property threshold As Double = 0

        ''' <summary>
        ''' the region spot data
        ''' </summary>
        ''' <returns></returns>
        Public Property spots As Point()
        ''' <summary>
        ''' a pre-computed cache data
        ''' </summary>
        ''' <returns></returns>
        Public Property precomputed As GeneralPath

        Private Function getShapes(summary As MSISummary) As GeneralPath
            Dim spots As New List(Of iPixelIntensity)(summary.rowScans.IteratesALL)

            If Not precomputed Is Nothing Then
                ' use the cached data directly
                Return precomputed
            End If

            If Me.spots.IsNullOrEmpty Then
                ' use all pixels
                ' draw the sample outline
                Return MSIRegionPlot.MeasureRegionPolygon(
                    spots.X.ToArray,
                    spots.Y.ToArray,
                    contour_scale, degree, resolution, q)
            Else
                ' draw the current region outline
                Return MSIRegionPlot.MeasureRegionPolygon(
                    Me.spots.X,
                    Me.spots.Y,
                    contour_scale, degree, resolution, q)
            End If
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim stroke As Pen = css.GetPen(line_stroke)
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim reader As PixelReader = base.reader
            Dim summary As MSISummary = reader.GetSummary

            If threshold > 0 Then
                Throw New NotImplementedException("threshold cutoff")
            End If

            Dim rect As Rectangle = stream.canvas.PlotRegion(css)
            Dim xscale = d3js.scale.linear.domain(values:={0.0, summary.size.Width}).range(values:=New Double() {rect.Left, rect.Right})
            Dim yscale = d3js.scale.linear.domain(values:={0.0, summary.size.Height}).range(values:=New Double() {rect.Top, rect.Bottom})
            Dim scale As New DataScaler(rev:=True) With {
                .AxisTicks = (Nothing, Nothing),
                .region = rect,
                .X = xscale,
                .Y = yscale
            }
            Dim shape As GeneralPath = getShapes(summary)

            For Each region As PointF() In shape.GetPolygons
                region = region _
                    .Select(Function(pi)
                                Return scale.Translate(pi)
                            End Function) _
                    .ToArray

                Call stream.g.DrawPolygon(stroke, region)
            Next

            Return Nothing
        End Function
    End Class
End Namespace
