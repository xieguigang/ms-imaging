Imports ggplot
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public Class MSIRGBCompositionLayer : Inherits ggplotLayer

    Public Overrides Function Plot(g As IGraphics,
                                   canvas As GraphicsRegion,
                                   baseData As ggplotData,
                                   x() As Double,
                                   y() As Double,
                                   scale As DataScaler,
                                   ggplot As ggplot.ggplot,
                                   theme As Theme) As IggplotLegendElement

        Throw New NotImplementedException()
    End Function
End Class
