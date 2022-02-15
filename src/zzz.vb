Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports SMRUCC.Rsharp.Runtime.Interop

<Assembly: RPackageModule>

Public Class zzz

    ''' <summary>
    ''' register ggplot handler
    ''' </summary>
    Public Shared Sub onLoad()
        Call ggplot.ggplot.Register(GetType(mzPack), Function(theme) New ggplotMSI(theme))
        Call ggplot.ggplot.Register(GetType(MSIHeatMap), Function(theme) New ggplotMSI(theme))
    End Sub
End Class
