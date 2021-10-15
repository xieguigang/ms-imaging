Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports ggplot
Imports SMRUCC.Rsharp.Runtime

Public Class MSIReader : Inherits ggplotReader

    ''' <summary>
    ''' returns the dimensions of the MSI raw data
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    Public Overrides Function getMapData(data As Object, env As Environment) As ggplotData
        Dim raw As mzPack = DirectCast(data, mzPack)
        Dim points = raw.MS.Select(Function(scan) scan.GetMSIPixel).ToArray
        Dim x As Double() = points.Select(Function(p) CDbl(p.X)).ToArray
        Dim y As Double() = points.Select(Function(p) CDbl(p.Y)).ToArray

        Return New ggplotData With {
            .x = x,
            .y = y
        }
    End Function
End Class
