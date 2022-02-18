Imports ggplot.options

Public Class MSIGaussBlurOption : Inherits ggplotOption

    Public Property blurLevels As Integer

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        ggplot.args.add("gauss_blur", blurLevels)
        Return ggplot
    End Function
End Class
