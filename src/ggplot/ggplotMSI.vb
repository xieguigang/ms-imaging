Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports ggplot
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas

Public Class ggplotMSI : Inherits ggplot.ggplot

    Public Sub New(theme As Theme)
        MyBase.New(theme)
    End Sub

    Public Overrides Function CreateReader(mapping As ggplot.ggplotReader) As ggplot.ggplotBase
        Select Case template
            Case GetType(mzPack)
                Return New ggplotBase With {
                    .reader = New MSIReader(DirectCast(data, mzPack))
                }
            Case GetType(MSIHeatMap)
                Return New ggplotBase() With {
                   .reader = New HeatMapReader(DirectCast(data, MSIHeatMap))
                }
            Case Else
                Throw New NotImplementedException(template.FullName)
        End Select
    End Function
End Class
