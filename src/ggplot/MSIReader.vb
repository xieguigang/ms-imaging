#Region "Microsoft.VisualBasic::c7e83a681b79dc82a4dffbdfc2d67ee4, Rscript\Library\MSI_app\src\ggplot\MSIReader.vb"

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

    '   Total Lines: 106
    '    Code Lines: 81 (76.42%)
    ' Comment Lines: 9 (8.49%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 16 (15.09%)
    '     File Size: 4.05 KB


    ' Class MSIReader
    ' 
    '     Properties: ggplot, offset, reader
    ' 
    '     Constructor: (+2 Overloads) Sub New
    ' 
    '     Function: getMapData
    ' 
    '     Sub: readFromMzPack
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.mzData.mzWebCache
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.TissueMorphology
Imports ggplot
Imports ggplot.elements
Imports ggplotMSImaging.data
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports SMRUCC.Rsharp.Runtime

''' <summary>
''' read ms-imaging raw data
''' </summary>
Public Class MSIReader : Inherits ggplotReader

    Public ReadOnly Property reader As PixelReader
    Public ReadOnly Property ggplot As ggplotMSI
    Public ReadOnly Property offset As PointF

    Sub New(raw As mzPack, ggplot As ggplotMSI)
        _reader = New ReadRawPack(mzpack:=raw, verbose:=False)
        _ggplot = ggplot
    End Sub

    Sub New(pack As PointPack, ggplot As ggplotMSI)
        _reader = New ReadPixelPack(pack.pixels, pack.GetDimensionSize)
        _ggplot = ggplot
    End Sub

    Private Sub readFromMzPack(raw As mzPack, env As Environment, ByRef x As Double(), ByRef y As Double())
        If {"region", "tissue"}.Any(AddressOf args.hasName) Then
            Dim tissue As TissueRegion = args.getValue(Of TissueRegion)({"region", "tissue"}, env)
            Dim polygon As Polygon2D = tissue.GetPolygons.First
            Dim rect = polygon.GetRectangle
            Dim dims As Size = rect.Size.ToSize
            Dim offset = rect.Location
            Dim dataPoints = raw.MS _
                .Where(Function(d) polygon.inside(d.GetMSIPixel)) _
                .Select(Function(scan)
                            Dim metadata As New Dictionary(Of String, String)(scan.meta)
                            Dim p As Point = scan.GetMSIPixel

                            metadata!x = p.X - offset.X
                            metadata!y = p.Y - offset.Y

                            Return New ScanMS1 With {
                                .BPC = scan.BPC,
                                .into = scan.into,
                                .mz = scan.mz,
                                .products = Nothing,
                                .rt = scan.rt,
                                .scan_id = scan.scan_id,
                                .TIC = scan.TIC,
                                .meta = metadata
                            }
                        End Function) _
                .ToArray

            If raw.metadata Is Nothing Then
                raw.metadata = New Dictionary(Of String, String)
            End If

            raw.MS = dataPoints
            raw.metadata!scan_x = dims.Width
            raw.metadata!scan_y = dims.Height

            _offset = offset
            _ggplot.dimension_size = dims
        End If

        Dim points As Point() = raw.MS _
            .Select(Function(scan) scan.GetMSIPixel) _
            .ToArray

        x = points.Select(Function(p) CDbl(p.X)).ToArray
        y = points.Select(Function(p) CDbl(p.Y)).ToArray
    End Sub

    ''' <summary>
    ''' returns the dimensions of the MSI raw data
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    Public Overrides Function getMapData(data As Object, env As Environment) As ggplotData
        Dim x As Double() = Nothing
        Dim y As Double() = Nothing

        If TypeOf data Is mzPack Then
            Call readFromMzPack(DirectCast(data, mzPack), env, x, y)
        ElseIf TypeOf data Is PointPack Then
            Dim pack As PointPack = DirectCast(data, PointPack)

            x = pack.pixels.Select(Function(p) CDbl(p.x)).ToArray
            y = pack.pixels.Select(Function(p) CDbl(p.y)).ToArray
        Else
            Throw New NotImplementedException
        End If

        Return New ggplotData With {
            .x = axisMap.FromNumeric(x),
            .y = axisMap.FromNumeric(y)
        }
    End Function
End Class
