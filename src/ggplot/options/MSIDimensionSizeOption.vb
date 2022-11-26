Imports System.Drawing
Imports ggplot.options

Public Class MSIDimensionSizeOption : Inherits ggplotOption

    Public Property dimension_size As Size

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        DirectCast(ggplot, ggplotMSI).dimension_size = dimension_size
        Return ggplot
    End Function
End Class
