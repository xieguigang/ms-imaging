Imports ggplot.options
Imports ggplotMSImaging.layers
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.hqx

Public Class MSIHqxScaleOption : Inherits ggplotOption

    Public Property hqx As HqxScales = HqxScales.Hqx_4x

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        For Each layer In ggplot.layers
            If TypeOf layer Is MSImagingLayer Then
                DirectCast(layer, MSImagingLayer).hqx = hqx
            End If
        Next

        Return ggplot
    End Function
End Class
