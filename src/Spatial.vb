Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.TissueMorphology
Imports ggplot.colors
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports MSImaging.layers.spatial
Imports SMRUCC.genomics.Analysis.HTS.DataFrame
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components

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
                                     STdata As Object,
                                     Optional colorSet As Object = "grays",
                                     Optional size As Double = 13,
                                     Optional env As Environment = Nothing) As Object

        Dim STMatrix As MatrixViewer

        If STdata Is Nothing Then
            Return Internal.debug.stop("the required STdata matrix can not be nothing!", env)
        ElseIf TypeOf STdata Is Matrix Then
            STMatrix = New HTSMatrixViewer(DirectCast(STdata, Matrix))
        ElseIf TypeOf STdata Is MatrixViewer Then
            STMatrix = STdata
        Else
            Return Message.InCompatibleType(GetType(Matrix), STdata.GetType, env)
        End If

        Dim index As Integer = STMatrix.GetSampleOrdinal(geneID)

        If index = -1 Then
            If geneID.TextEquals("sum") OrElse
                geneID.TextEquals("max") OrElse
                geneID.TextEquals("avg") OrElse
                geneID.TextEquals("mean") Then
                ' do nothing
            Else
                ' generate error for missing symbol id
                Return Internal.debug.stop({
                    $"target gene symbol({geneID}) is not found in your STdata matrix!",
                    $"target_symbol: {geneID}"
                }, env)
            End If
        End If

        Return New ggplotSpatialTranscriptomicsLayer With {
            .geneID = geneID,
            .spots = tile.spots _
                .Where(Function(s) s.flag > 0) _
                .ToArray,
            .STdata = STMatrix,
            .label = tile.label,
            .ordinal = index,
            .colorMap = ggplotColorMap.CreateColorMap(colorSet, 1, env),
            .spotSize = size
        }
    End Function
End Module