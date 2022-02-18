Imports ggplot.options
Imports any = Microsoft.VisualBasic.Scripting

Public Class MSIBackgroundOption : Inherits ggplotOption

    ''' <summary>
    ''' the background color or texture brush object
    ''' </summary>
    ''' <returns></returns>
    Public Property background As Object

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        ggplot.ggplotTheme.gridFill = any.ToString(background)
        Return ggplot
    End Function
End Class
