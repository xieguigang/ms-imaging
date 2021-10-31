#Region "Microsoft.VisualBasic::d064c0fd1aab098ad4246568d3bb897f, Rscript\Library\MSI_app\src\ggplot\MSIReader.vb"

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

    ' Class MSIReader
    ' 
    '     Properties: reader
    ' 
    '     Function: getMapData
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplot
Imports SMRUCC.Rsharp.Runtime

Public Class MSIReader : Inherits ggplotReader

    Public ReadOnly Property reader As PixelReader

    ''' <summary>
    ''' returns the dimensions of the MSI raw data
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    Public Overrides Function getMapData(data As Object, env As Environment) As ggplotData
        Dim raw As mzPack = DirectCast(data, mzPack)
        Dim points = raw.MS.Select(Function(scan) scan.GetMSIPixel).ToArray
        Dim x As Double() = points.Select(Function(p) CDbl(p.X)).ToArray
        Dim y As Double() = points.Select(Function(p) CDbl(p.Y)).ToArray

        _reader = New ReadRawPack(mzpack:=raw)

        Return New ggplotData With {
            .x = x,
            .y = y
        }
    End Function
End Class

