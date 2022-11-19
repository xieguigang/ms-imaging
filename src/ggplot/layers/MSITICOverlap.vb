Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers

Namespace layers

    Public Class MSITICOverlap : Inherits ggplotLayer

        Public Property summary As IntensitySummary

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace