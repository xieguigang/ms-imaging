#Region "Microsoft.VisualBasic::fe2741327df7529481f394a34d8294d4, Rscript\Library\MSI_app\src\ggplot\options\MSIHqxScaleOption.vb"

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

    '   Total Lines: 26
    '    Code Lines: 20 (76.92%)
    ' Comment Lines: 1 (3.85%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 5 (19.23%)
    '     File Size: 822 B


    ' Class MSIHqxScaleOption
    ' 
    '     Properties: hqx
    ' 
    '     Function: Config
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.layers
Imports ggplot.options
Imports ggplotMSImaging.layers
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.hqx

Public Class MSIHqxScaleOption : Inherits ggplotOption

    Public Property hqx As HqxScales = HqxScales.Hqx_4x

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        ' 1 means no scale, none
        If hqx = 1 Then
            hqx = HqxScales.None
        End If

        For Each layer As ggplotLayer In ggplot.layers
            If TypeOf layer Is MSImagingLayer Then
                DirectCast(layer, MSImagingLayer).hqx = hqx
            ElseIf TypeOf layer Is MSITICOverlap Then
                DirectCast(layer, MSITICOverlap).hqx = hqx
            End If
        Next

        Return ggplot
    End Function
End Class

