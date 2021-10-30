Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports ggplot
Imports ggplot.layers

Public MustInherit Class ggplotMSILayer : Inherits ggplotLayer

    Public Property pixelDrawer As Boolean = False

    Public Function getIonlayer(mz As Double, mzdiff As Tolerance, ggplot As ggplot.ggplot) As SingleIonLayer
        Dim base = DirectCast(ggplot.base.reader, MSIReader)
        Dim ion As SingleIonLayer = SingleIonLayer.GetLayer(mz, base.reader, mzdiff)

        Return ion
    End Function
End Class
