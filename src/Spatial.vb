#Region "Microsoft.VisualBasic::6dee4e9c98144a17456a79fb58513c0b, Rscript\Library\MSI_app\src\Spatial.vb"

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

    '   Total Lines: 70
    '    Code Lines: 53
    ' Comment Lines: 10
    '   Blank Lines: 7
    '     File Size: 2.65 KB


    ' Module ggplotSpatial
    ' 
    '     Function: geom_spatialtile
    ' 
    ' /********************************************************************************/

#End Region

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
