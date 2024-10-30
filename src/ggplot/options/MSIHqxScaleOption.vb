Imports ggplot.layers
Imports ggplot.options
Imports ggplotMSImaging.layers
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.hqx

Public Class MSIHqxScaleOption : Inherits ggplotOption

    Public Property hqx As HqxScales = HqxScales.Hqx_4x

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        ' 1 means no scale, none
        If hqx = 1 Then
            hqx = HqxScales.None
        End If

        For Each layer As ggplotLayer In ggplot.layers
            If TypeOf layer Is MSImagingLayer Then
                DirectCast(layer, MSImagingLayer).hqx = hqx
            ElseIf TypeOf layer Is MSITICOverlap Then
                DirectCast(layer, MSITICOverlap).hqx = hqx
            End If
        Next

        Return ggplot
    End Function
End Class
