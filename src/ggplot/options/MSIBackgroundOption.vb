﻿#Region "Microsoft.VisualBasic::5693d6261fa2a890f66915d6c118b3e7, Rscript\Library\MSI_app\src\ggplot\options\MSIBackgroundOption.vb"

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

    '   Total Lines: 16
    '    Code Lines: 9 (56.25%)
    ' Comment Lines: 4 (25.00%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 3 (18.75%)
    '     File Size: 493 B


    ' Class MSIBackgroundOption
    ' 
    '     Properties: background
    ' 
    '     Function: Config
    ' 
    ' /********************************************************************************/

#End Region

Imports ggplot.options
Imports any = Microsoft.VisualBasic.Scripting

Public Class MSIBackgroundOption : Inherits ggplotOption

    ''' <summary>
    ''' the background color or texture brush object
    ''' </summary>
    ''' <returns></returns>
    Public Property background As Object

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        ggplot.ggplotTheme.gridFill = any.ToString(background)
        Return ggplot
    End Function
End Class
