﻿#Region "Microsoft.VisualBasic::5c64fd25d4df6d70fc07147767da2655, Rscript\Library\MSI_app\src\ggplot\options\MSIDimensionSizeOption.vb"

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

    '   Total Lines: 12
    '    Code Lines: 9
    ' Comment Lines: 0
    '   Blank Lines: 3
    '     File Size: 349 B


    ' Class MSIDimensionSizeOption
    ' 
    '     Properties: dimension_size
    ' 
    '     Function: Config
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports ggplot.options

Public Class MSIDimensionSizeOption : Inherits ggplotOption

    Public Property dimension_size As Size

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        DirectCast(ggplot, ggplotMSI).dimension_size = dimension_size
        Return ggplot
    End Function
End Class
