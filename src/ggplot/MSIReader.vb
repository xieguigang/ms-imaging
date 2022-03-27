#Region "Microsoft.VisualBasic::cea25db9530bbf141e6228831140fa17, mzkit\Rscript\Library\MSI_app\src\ggplot\MSIReader.vb"

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

'   Total Lines: 55
'    Code Lines: 35
' Comment Lines: 9
'   Blank Lines: 11
'     File Size: 1.71 KB


' Class MSIReader
' 
'     Properties: reader
' 
'     Constructor: (+1 Overloads) Sub New
'     Function: getMapData
' 
' Class HeatMapReader
' 
'     Properties: heatmap
' 
'     Constructor: (+1 Overloads) Sub New
'     Function: getMapData
' 
' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplot
Imports SMRUCC.Rsharp.Runtime

''' <summary>
''' read ms-imaging raw data
''' </summary>
Public Class MSIReader : Inherits ggplotReader

    Public ReadOnly Property reader As PixelReader

    Sub New(raw As mzPack)
        _reader = New ReadRawPack(mzpack:=raw)
    End Sub

    Sub New(pack As PointPack)
        _reader = New ReadPixelPack(pack.pixels)
    End Sub

    ''' <summary>
    ''' returns the dimensions of the MSI raw data
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    Public Overrides Function getMapData(data As Object, env As Environment) As ggplotData
        Dim x As Double()
        Dim y As Double()

        If TypeOf data Is mzPack Then
            Dim raw As mzPack = DirectCast(data, mzPack)
            Dim points = raw.MS.Select(Function(scan) scan.GetMSIPixel).ToArray

            x = points.Select(Function(p) CDbl(p.X)).ToArray
            y = points.Select(Function(p) CDbl(p.Y)).ToArray
        ElseIf TypeOf data Is PointPack Then
            Dim pack As PointPack = DirectCast(data, PointPack)

            x = pack.pixels.Select(Function(p) CDbl(p.x)).ToArray
            y = pack.pixels.Select(Function(p) CDbl(p.y)).ToArray
        Else
            Throw New NotImplementedException
        End If

        Return New ggplotData With {
            .x = x,
            .y = y
        }
    End Function
End Class

Public Class HeatMapReader : Inherits ggplotReader

    Public ReadOnly Property heatmap As MSIHeatMap

    Sub New(heatmap As MSIHeatMap)
        Me.heatmap = heatmap
    End Sub

    Public Overrides Function getMapData(data As Object, env As Environment) As ggplotData
        Dim raw As MSIHeatMap = DirectCast(data, MSIHeatMap)
        Dim x As Double() = New Double() {0, raw.dimension.Width}
        Dim y As Double() = New Double() {0, raw.dimension.Height}

        Return New ggplotData With {
           .x = x,
           .y = y
        }
    End Function

End Class

Public Class PointPack

    Public Property pixels As PixelData()

End Class