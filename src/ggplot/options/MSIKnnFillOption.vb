#Region "Microsoft.VisualBasic::1403761fa90f6449b94d538713bcc5e0, G:/mzkit/Rscript/Library/MSI_app/src//ggplot/options/MSIKnnFillOption.vb"

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

    '   Total Lines: 15
    '    Code Lines: 11
    ' Comment Lines: 0
    '   Blank Lines: 4
    '     File Size: 413 B


    ' Class MSIKnnFillOption
    ' 
    '     Properties: k, qcut
    ' 
    '     Function: Config
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.options

Public Class MSIKnnFillOption : Inherits ggplotOption

    Public Property k As Integer = 3
    Public Property qcut As Double = 0.8

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        ggplot.args.add("knn", k)
        ggplot.args.add("qcut", qcut)
        ggplot.args.add("knnFill", True)

        Return ggplot
    End Function
End Class
