
Imports ggplot
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Internal.Object

<Package("ggplot")>
Public Module Rscript

    <ExportAPI("geom_msimaging")>
    Public Function geom_msimaging(mz As Double) As ggplotLayer
        Return New MSImagingLayer With {
            .reader = New ggplotReader With {
                .args = New list With {
                    .slots = New Dictionary(Of String, Object) From {
                        {"mz", mz}
                    }
                }
            }
        }
    End Function

    <ExportAPI("geom_color")>
    Public Function geom_color(mz As Double, color As Object, Optional env As Environment = Nothing) As ggplotLayer

    End Function
End Module
