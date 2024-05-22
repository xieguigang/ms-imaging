#Region "Microsoft.VisualBasic::dfd9d774e152d02684f9def5abf9a4e2, Rscript\Library\MSI_app\src\Spatial.vb"

    ' Author:
    ' 
    '       xieguigang (gg.xie@bionovogene.com, BioNovoGene Co., LTD.)
    ' 
    ' Copyright (c) 2018 gg.xie@bionovogene.com, BioNovoGene Co., LTD.
    ' 
    ' 
    ' MIT License
    ' 
    ' 
    ' Permission is hereby granted, free of charge, to any person obtaining a copy
    ' of this software and associated documentation files (the "Software"), to deal
    ' in the Software without restriction, including without limitation the rights
    ' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ' copies of the Software, and to permit persons to whom the Software is
    ' furnished to do so, subject to the following conditions:
    ' 
    ' The above copyright notice and this permission notice shall be included in all
    ' copies or substantial portions of the Software.
    ' 
    ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ' SOFTWARE.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 126
    '    Code Lines: 84 (66.67%)
    ' Comment Lines: 29 (23.02%)
    '    - Xml Docs: 89.66%
    ' 
    '   Blank Lines: 13 (10.32%)
    '     File Size: 5.30 KB


    ' Module ggplotSpatial
    ' 
    '     Function: geom_spatialScatter, geom_spatialtile
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.TissueMorphology
Imports ggplot.colors
Imports ggplotMSImaging.layers.spatial
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.genomics.Analysis.HTS.DataFrame
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop
Imports SMRUCC.Rsharp.Runtime.Vectorization

''' <summary>
''' spatial mapping between the STdata and SMdata based on the ggplot framework
''' </summary>
<Package("ggspatial")>
Public Module ggplotSpatial

    ''' <summary>
    ''' draw scatter layer with a given x and y coordinates.
    ''' </summary>
    ''' <param name="x">a numeric vector of pixel x</param>
    ''' <param name="y">a numeric vector of pixel y</param>
    ''' <param name="colors">the colors for rendering each scatter spot</param>
    ''' <param name="size">the spot size for the drawing</param>
    ''' <param name="alpha">the transparency alpha value for drawing the scatter points.</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("geom_spatialScatter")>
    <RApiReturn(GetType(SpatialAnnotationScatterLayer))>
    Public Function geom_spatialScatter(<RRawVectorArgument> x As Object,
                                        <RRawVectorArgument> y As Object,
                                        <RRawVectorArgument> colors As Object,
                                        Optional size As Single = 3,
                                        Optional alpha As Single = 0.8,
                                        Optional env As Environment = Nothing) As Object

        Dim px As Double() = CLRVector.asNumeric(x)
        Dim py As Double() = CLRVector.asNumeric(y)
        Dim colorSet As String() = CLRVector.asCharacter(colors)

        If colorSet Is Nothing Then
            Return Internal.debug.stop("invalid color set value!", env)
        End If

        If colorSet.Length = 1 Then
            colorSet = colorSet(0).Replicate(px.Length).ToArray
        End If

        Return New SpatialAnnotationScatterLayer With {
            .x = px,
            .y = py,
            .size = size,
            .alpha = alpha,
            .colors = colorSet,
            .showLegend = False
        }
    End Function

    ''' <summary>
    ''' add a spatial overlaps of the STdata on current SMdata imaging
    ''' </summary>
    ''' <param name="tile">A collection of the spatial spot mapping</param>
    ''' <param name="STdata">the spatial transcriptomics data matrix</param>
    ''' <param name="geneID">
    ''' the gene ID for pull the spatial expression from the <paramref name="STdata"/>. 
    ''' </param>
    ''' <param name="colorSet">
    ''' the color set data for rendering the gene expression spatial heatmap
    ''' </param>
    ''' <param name="size">the spot size when drawing the spatial expression</param>
    ''' <returns></returns>
    <ExportAPI("geom_spatialtile")>
    <RApiReturn(GetType(ggplotSpatialTranscriptomicsLayer))>
    Public Function geom_spatialtile(tile As SpatialMapping,
                                     Optional geneID As String = Nothing,
                                     Optional STdata As Object = Nothing,
                                     Optional colorSet As Object = "grays",
                                     Optional size As Double = 13,
                                     Optional env As Environment = Nothing) As Object

        Dim STMatrix As MatrixViewer = Nothing

        If STdata Is Nothing Then
            ' Return Internal.debug.stop("the required STdata matrix can not be nothing!", env)
        ElseIf TypeOf STdata Is Matrix Then
            STMatrix = New HTSMatrixViewer(DirectCast(STdata, Matrix))
        ElseIf TypeOf STdata Is MatrixViewer Then
            STMatrix = STdata
        Else
            Return Message.InCompatibleType(GetType(Matrix), STdata.GetType, env)
        End If

        Dim index As Integer = -1

        If Not STMatrix Is Nothing Then
            index = STMatrix.GetSampleOrdinal(geneID)

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
