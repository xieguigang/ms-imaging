﻿#Region "Microsoft.VisualBasic::9e3707c9dbd20ba43944c68c1caac822, Rscript\Library\MSI_app\src\zzz.vb"

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

    '   Total Lines: 22
    '    Code Lines: 15 (68.18%)
    ' Comment Lines: 3 (13.64%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 4 (18.18%)
    '     File Size: 908 B


    ' Class zzz
    ' 
    '     Sub: onLoad
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplotMSImaging.data
Imports SMRUCC.Rsharp.Runtime.Interop
Imports RInternal = SMRUCC.Rsharp.Runtime.Internal

<Assembly: RPackageModule>

Public Class zzz

    ''' <summary>
    ''' register ggplot handler
    ''' </summary>
    Public Shared Sub onLoad()
        Call ggplot.ggplot.Register(GetType(mzPack), Function(theme) New ggplotMSI(theme))
        Call ggplot.ggplot.Register(GetType(MSIHeatMap), Function(theme) New ggplotMSI(theme))
        Call ggplot.ggplot.Register(GetType(PointPack), Function(theme) New ggplotMSI(theme))
        Call ggplot.ggplot.Register(GetType(MemoryIndexReader), Function(theme) New ggplotMSI(theme))

        Call RInternal.generic.add("plot", GetType(ggplotMSI), AddressOf ggplot.zzz.plotGGplot)
    End Sub
End Class
