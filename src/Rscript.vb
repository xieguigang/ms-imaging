
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports ggplot
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("ggplot")>
Public Module Rscript

    <ExportAPI("geom_msimaging")>
    <RApiReturn(GetType(ggplotLayer))>
    Public Function geom_msimaging(mz As Double,
                                   Optional tolerance As Object = "da:0.1",
                                   Optional env As Environment = Nothing) As Object

        Dim mzdiff = Math.getTolerance(tolerance, env)

        If mzdiff Like GetType(Message) Then
            Return mzdiff.TryCast(Of Message)
        End If

        Return New MSImagingLayer With {
            .reader = New ggplotReader With {
                .args = New list With {
                    .slots = New Dictionary(Of String, Object) From {
                        {"mz", mz},
                        {"mzdiff", mzdiff.TryCast(Of Tolerance)}
                    }
                }
            }
        }
    End Function

    <ExportAPI("geom_color")>
    Public Function geom_color(mz As Double, color As Object, Optional env As Environment = Nothing) As ggplotLayer

    End Function
End Module
