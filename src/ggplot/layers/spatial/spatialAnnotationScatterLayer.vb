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

            For i As Integer = 0 To x.Length - 1
                b = color.ComputeIfAbsent(colors(i), Function(c) New SolidBrush(c.TranslateColor.Alpha(opacity)))
                xi = stream.scale.TranslateX(x(i))
                yi = stream.scale.TranslateY(y(i))
                stream.g.DrawCircle(New PointF(xi, yi), size, b)
            Next

            Return Nothing
        End Function
    End Class
End Namespace