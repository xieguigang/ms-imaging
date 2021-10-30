
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports ggplot
Imports ggplot.layers
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("ggplot")>
Public Module Rscript

    Private Function unionlayers(layers As IEnumerable(Of ggplotLayer)) As IEnumerable(Of ggplotLayer)
        Dim all = layers.ToArray

        If all.Any(Function(l) TypeOf l Is MSIChannelLayer) Then
            Dim list As New List(Of ggplotLayer)(all)
            Dim union As New MSIRGBCompositionLayer With {
                .red = getChannel(list, MSIChannelLayer.Channels.Red),
                .blue = getChannel(list, MSIChannelLayer.Channels.Blue),
                .green = getChannel(list, MSIChannelLayer.Channels.Green)
            }

            list.Insert(0, union)

            If Not union.red Is Nothing Then list.Remove(union.red)
            If Not union.green Is Nothing Then list.Remove(union.green)
            If Not union.blue Is Nothing Then list.Remove(union.blue)

            Return list
        Else
            Return all
        End If
    End Function

    Private Function getChannel(all As IEnumerable(Of ggplotLayer), channel As MSIChannelLayer.Channels) As MSIChannelLayer
        Return (From layer As ggplotLayer
                In all
                Where TypeOf layer Is MSIChannelLayer
                Where DirectCast(layer, MSIChannelLayer).channel = channel
                Select layer).FirstOrDefault
    End Function

    ''' <summary>
    ''' create a MSI data reader.
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("MSImaging")>
    Public Function MSIReader() As ggplotReader
        Return New MSIReader
    End Function

    <ExportAPI("geom_msimaging")>
    <RApiReturn(GetType(ggplotLayer))>
    Public Function geom_msimaging(mz As Double,
                                   Optional tolerance As Object = "da:0.1",
                                   Optional pixel_render As Boolean = False,
                                   <RRawVectorArgument(GetType(Double))>
                                   Optional cutoff As Object = "0.05,0.65",
                                   <RRawVectorArgument>
                                   Optional color As Object = "Jet",
                                   Optional env As Environment = Nothing) As Object

        Dim mzdiff = Math.getTolerance(tolerance, env)
        Dim cutoffRange = ApiArgumentHelpers.GetDoubleRange(cutoff, env)

        If cutoffRange Like GetType(Message) Then
            Return cutoffRange.TryCast(Of Message)
        ElseIf mzdiff Like GetType(Message) Then
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
            },
            .pixelDrawer = pixel_render,
            .cutoff = cutoffRange.TryCast(Of DoubleRange),
            .colorMap = If(color Is Nothing, Nothing, ggplotColorMap.CreateColorMap(map:=color, env))
        }
    End Function

    <ExportAPI("geom_color")>
    <RApiReturn(GetType(ggplotLayer))>
    Public Function geom_color(mz As Double, color As Object,
                               Optional tolerance As Object = "da:0.1",
                               Optional pixel_render As Boolean = False,
                               Optional env As Environment = Nothing) As Object

        Dim mzdiff = Math.getTolerance(tolerance, env)

        If mzdiff Like GetType(Message) Then
            Return mzdiff.TryCast(Of Message)
        Else
            ggplot.ggplot.UnionGgplotLayers = AddressOf unionlayers
        End If

        Return New MSIChannelLayer With {
            .reader = New ggplotReader With {
                .args = New list With {
                    .slots = New Dictionary(Of String, Object) From {
                        {"mz", mz},
                        {"mzdiff", mzdiff.TryCast(Of Tolerance)}
                    }
                }
            },
            .pixelDrawer = pixel_render,
            .colorMap = New ggplotColorLiteral With {
                .colorMap = color
            }
        }
    End Function
End Module
