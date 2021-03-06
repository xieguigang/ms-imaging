#Region "Microsoft.VisualBasic::9f0fb08aef468bf59f574d0e670e1b9c, mzkit\Rscript\Library\MSI_app\src\ggplot\ggplotMSI.vb"

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

'   Total Lines: 25
'    Code Lines: 22
' Comment Lines: 0
'   Blank Lines: 3
'     File Size: 887.00 B


' Class ggplotMSI
' 
'     Constructor: (+1 Overloads) Sub New
'     Function: CreateReader
' 
' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports ggplot
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas

Public Class ggplotMSI : Inherits ggplot.ggplot

    Public Sub New(theme As Theme)
        MyBase.New(theme)

        titleOffset = 1.125
    End Sub

    Public Overrides Function CreateReader(mapping As ggplot.ggplotReader) As ggplot.ggplotBase
        Select Case template
            Case GetType(mzPack) : Return New ggplotBase With {.reader = New MSIReader(DirectCast(data, mzPack))}
            Case GetType(MSIHeatMap) : Return New ggplotBase() With {.reader = New HeatMapReader(DirectCast(data, MSIHeatMap))}
            Case GetType(PixelData()) : Return New ggplotBase() With {.reader = New MSIReader(New PointPack With {.pixels = DirectCast(data, PixelData())})}
            Case GetType(PointPack) : Return New ggplotBase With {.reader = New MSIReader(DirectCast(data, PointPack))}
            Case Else
                Throw New NotImplementedException(template.FullName)
        End Select
    End Function
End Class
