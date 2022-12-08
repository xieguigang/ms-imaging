Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.TissueMorphology
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports MSImaging.layers.spatial
Imports SMRUCC.genomics.Analysis.HTS.DataFrame

''' <summary>
''' spatial mapping between the STdata and SMdata based on the ggplot framework
''' </summary>
<Package("ggspatial")>
Public Module ggplotSpatial

    ''' <summary>
    ''' add a spatial overlaps of the STdata on current SMdata imaging
    ''' </summary>
    ''' <param name="tile"></param>
    ''' <returns></returns>
    <ExportAPI("geom_spatialtile")>
    Public Function geom_spatialtile(tile As SpatialMapping, geneID As String, STdata As HTSMatrixReader) As Object
        Return New ggplotSpatialTranscriptomicsLayer With {
            .geneID = geneID,
            .spots = tile.spots _
                .Where(Function(s) s.flag > 0) _
                .ToArray,
            .STdata = STdata,
            .label = tile.label
        }
    End Function
End Module