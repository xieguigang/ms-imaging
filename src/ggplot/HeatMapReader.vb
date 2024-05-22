#Region "Microsoft.VisualBasic::fc7e63ad0200c55b585488fc30d722cb, Rscript\Library\MSI_app\src\ggplot\HeatMapReader.vb"

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

    '   Total Lines: 23
    '    Code Lines: 18 (78.26%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 5 (21.74%)
    '     File Size: 711 B


    ' Class HeatMapReader
    ' 
    '     Properties: heatmap
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: getMapData
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot
Imports ggplot.elements
Imports SMRUCC.Rsharp.Runtime
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
           .x = axisMap.FromNumeric(x),
           .y = axisMap.FromNumeric(y)
        }
    End Function

End Class
