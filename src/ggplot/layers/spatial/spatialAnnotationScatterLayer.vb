#Region "Microsoft.VisualBasic::c9c7e061b795f1d3a3c3c61f97d2f0ba, Rscript\Library\MSI_app\src\ggplot\layers\spatial\spatialAnnotationScatterLayer.vb"

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

    '   Total Lines: 41
    '    Code Lines: 32 (78.05%)
    ' Comment Lines: 1 (2.44%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 8 (19.51%)
    '     File Size: 1.40 KB


    '     Class SpatialAnnotationScatterLayer
    ' 
    '         Properties: colors, size, x, y
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Imaging

Namespace layers.spatial

    Public Class SpatialAnnotationScatterLayer : Inherits ggplotLayer

        Public Property x As Double()
        Public Property y As Double()
        Public Property colors As String()
        Public Property size As Single

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim color As New Dictionary(Of String, Brush)
            Dim b As Brush
            Dim xi, yi As Single
            Dim opacity As Integer = alpha * 255

            ' is transparent?
            If opacity = 0 Then
                Return Nothing
            End If

            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim paddingTop = rect.Top

            For i As Integer = 0 To x.Length - 1
                b = color.ComputeIfAbsent(colors(i), Function(c) New SolidBrush(c.TranslateColor.Alpha(opacity)))
                xi = stream.scale.TranslateX(x(i))
                yi = rect.Bottom - stream.scale.TranslateY(y(i)) + paddingTop
                stream.g.DrawCircle(New PointF(xi, yi), size, b)
            Next

            Return Nothing
        End Function
    End Class
End Namespace
