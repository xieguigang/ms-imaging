Imports ggplot.options

Public Class MSIBackgroundOption : Inherits ggplotOption

    ''' <summary>
    ''' the background color or texture brush object
    ''' </summary>
    ''' <returns></returns>
    Public Property background As Object

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        Throw New NotImplementedException()
    End Function
End Class
