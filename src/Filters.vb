#Region "Microsoft.VisualBasic::df51dca770ea63c1db55b2a2128c070c, mzkit\Rscript\Library\MSI_app\src\Filters.vb"

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

    '   Total Lines: 37
    '    Code Lines: 30
    ' Comment Lines: 0
    '   Blank Lines: 7
    '     File Size: 1.22 KB


    ' Module Filters
    ' 
    '     Function: denoiseScaler, knnScaler, logScaler, quantileScaler, softenScaler
    '               TrIQScaler
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

''' <summary>
''' Helper function module for create the image filter pipeline
''' </summary>
<Package("MSIfilter")>
Module Filters

    ''' <summary>
    ''' Normalized the raw input intensity value via log(N)
    ''' </summary>
    ''' <param name="base">log(N), N=2 by default</param>
    ''' <returns></returns>
    <ExportAPI("log_scale")>
    Public Function logScaler(Optional base As Double = 2.0) As LogScaler
        Return New LogScaler(base)
    End Function

    <ExportAPI("quantile_scale")>
    Public Function quantileScaler(Optional q As Double = 0.5) As QuantileScaler
        Return New QuantileScaler(q)
    End Function

    ''' <summary>
    ''' Trim the raw input intensity value via the TrIQ algorithm
    ''' </summary>
    ''' <param name="q"></param>
    ''' <returns></returns>
    <ExportAPI("TrIQ_scale")>
    Public Function TrIQScaler(Optional q As Double = 0.6) As TrIQScaler
        Return New TrIQScaler(q)
    End Function

    ''' <summary>
    ''' Make convolution of the spatial data for make the imaging render result soften
    ''' </summary>
    ''' <returns></returns>
    <ExportAPI("soften_scale")>
    Public Function softenScaler() As SoftenScaler
        Return New SoftenScaler()
    End Function

    ''' <summary>
    ''' Trying to fill the missing spatial spot on the imaging via knn method 
    ''' </summary>
    ''' <param name="k"></param>
    ''' <param name="q"></param>
    ''' <returns></returns>
    <ExportAPI("knn_scale")>
    Public Function knnScaler(Optional k As Integer = 3, Optional q As Double = 0.65) As KNNScaler
        Return New KNNScaler(k, q)
    End Function

    <ExportAPI("denoise_scale")>
    Public Function denoiseScaler(Optional q As Double = 0.01) As DenoiseScaler
        Return New DenoiseScaler(q)
    End Function
End Module
