Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports ggplot
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public Class MSImagingLayer : Inherits ggplotLayer

    Public Overrides Function Plot(g As IGraphics,
                                   canvas As GraphicsRegion,
                                   baseData As ggplotData,
                                   x() As Double,
                                   y() As Double,
                                   scale As DataScaler,
                                   ggplot As ggplot.ggplot,
                                   theme As Theme) As legendGroupElement

        Dim args = reader.args
        Dim mz As Double = args.getValue(Of Double)("mz", ggplot.environment)
        Dim mzdiff As Tolerance = args.getValue(Of Tolerance)("mzdiff", ggplot.environment)

        If mz <= 0 Then
            Throw New InvalidProgramException("invalid ion m/z value!")
        End If
        If mzdiff Is Nothing Then
            mzdiff = Tolerance.DeltaMass(0.1)
            ggplot.environment.AddMessage("missing 'tolerance' parameter, use the default da:0.1 as mzdiff tolerance value!")
        End If

        Throw New NotImplementedException()
    End Function
End Class
