Imports ggplot.options

Public Class MSIKnnFillOption : Inherits ggplotOption

    Public Property k As Integer = 3
    Public Property qcut As Double = 0.8

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        ggplot.args.add("knn", k)
        ggplot.args.add("qcut", qcut)
        ggplot.args.add("knnFill", True)

        Return ggplot
    End Function
End Class
