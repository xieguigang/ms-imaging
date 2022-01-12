Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports SMRUCC.Rsharp.Runtime.Interop

<Assembly: RPackageModule>

Public Class zzz

    Public Shared Sub onLoad()
        Call ggplot.ggplot.Register(GetType(mzPack), Function(theme) New ggMSI(theme))
    End Sub
End Class
