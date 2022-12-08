Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.TissueMorphology
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports MSImaging.layers.spatial
Imports SMRUCC.genomics.Analysis.HTS.DataFrame
Imports SMRUCC.Rsharp.Runtime

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
    Public Function geom_spatialtile(tile As SpatialMapping,
                                     geneID As String,
                                     STdata As HTSMatrixReader,
                                     Optional env As Environment = Nothing) As Object

        Dim index As Integer = STdata.GetSampleOrdinal(geneID)

        If index = -1 Then
            Return Internal.debug.stop({
                $"target gene symbol({geneID}) is not found in your STdata matrix!",
                $"target_symbol: {geneID}"
            }, env)
        End If

        Return New ggplotSpatialTranscriptomicsLayer With {
            .geneID = geneID,
            .spots = tile.spots _
                .Where(Function(s) s.flag > 0) _
                .ToArray,
            .STdata = STdata,
            .label = tile.label,
            .ordinal = index
        }
    End Function
End Module