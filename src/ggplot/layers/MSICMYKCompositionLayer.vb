Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Imaging

Namespace layers

    Public Class MSICMYKCompositionLayer : Inherits ggplotMSILayer

        ''' <summary>
        ''' the raster image background for the TIC overlaps
        ''' </summary>
        ''' <returns></returns>
        Public Property raster As Image

        Public Property cyan As MzAnnotation
        Public Property magenta As MzAnnotation
        Public Property yellow As MzAnnotation
        Public Property key As MzAnnotation

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Throw New NotImplementedException
        End Function
    End Class
End Namespace