Imports ggplot
Imports ggplot.elements
Imports SMRUCC.Rsharp.Runtime
Public Class HeatMapReader : Inherits ggplotReader

    Public ReadOnly Property heatmap As MSIHeatMap

    Sub New(heatmap As MSIHeatMap)
        Me.heatmap = heatmap
    End Sub

    Public Overrides Function getMapData(data As Object, env As Environment) As ggplotData
        Dim raw As MSIHeatMap = DirectCast(data, MSIHeatMap)
        Dim x As Double() = New Double() {0, raw.dimension.Width}
        Dim y As Double() = New Double() {0, raw.dimension.Height}

        Return New ggplotData With {
           .x = axisMap.FromNumeric(x),
           .y = axisMap.FromNumeric(y)
        }
    End Function

End Class