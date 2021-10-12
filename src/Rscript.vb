
Imports ggplot
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("ggplot")>
Public Module Rscript

    <ExportAPI("geom_msimaging")>
    Public Function geom_msimaging() As ggplotLayer

    End Function
End Module
