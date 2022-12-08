Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.TissueMorphology
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers
Imports SMRUCC.genomics.Analysis.HTS.DataFrame

Namespace layers.spatial

    Public Class ggplotSpatialTranscriptomicsLayer : Inherits ggplotLayer

        Public Property spots As SpotMap()
        Public Property STdata As HTSMatrixReader
        Public Property geneID As String
        Public Property label As String

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement

        End Function
    End Class
End Namespace