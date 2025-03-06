#Region "Microsoft.VisualBasic::d007432919a4b8d8ea69595e19661b0d, Rscript\Library\MSI_app\src\Filters.vb"

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

    '   Total Lines: 103
    '    Code Lines: 53 (51.46%)
    ' Comment Lines: 39 (37.86%)
    '    - Xml Docs: 97.44%
    ' 
    '   Blank Lines: 11 (10.68%)
    '     File Size: 3.64 KB


    ' Module Filters
    ' 
    '     Function: construct_filter, construct_pipeline, denoiseScaler, intensity_cut, knnScaler
    '               logScaler, quantileScaler, softenScaler, TrIQScaler
    ' 
    ' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' Helper function module for create the image filter pipeline
''' </summary>
<Package("MSIfilter")>
Module Filters

    ''' <summary>
    ''' removes low intensity spots
    ''' </summary>
    ''' <param name="threshold"></param>
    ''' <param name="quantile"></param>
    ''' <returns></returns>
    ''' <keywords>intensity</keywords>
    <ExportAPI("intensity_cut")>
    Public Function intensity_cut(Optional threshold As Double = 0.05, Optional quantile As Boolean = False) As IntensityCutScaler
        Return New IntensityCutScaler(threshold, quantile)
    End Function

    ''' <summary>
    ''' Normalized the raw input intensity value via log(N)
    ''' </summary>
    ''' <param name="base">log(N), N=2 by default</param>
    ''' <returns></returns>
    ''' <keywords>intensity</keywords>
    <ExportAPI("log_scale")>
    Public Function logScaler(Optional base As Double = 2.0) As LogScaler
        Return New LogScaler(base)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="q"></param>
    ''' <returns></returns>
    ''' <keywords>intensity</keywords>
    <ExportAPI("quantile_scale")>
    Public Function quantileScaler(Optional q As Double = 0.5) As QuantileScaler
        Return New QuantileScaler(q)
    End Function

    ''' <summary>
    ''' Trim the raw input intensity value via the TrIQ algorithm
    ''' </summary>
    ''' <param name="q"></param>
    ''' <returns></returns>
    ''' <keywords>intensity</keywords>
    <ExportAPI("TrIQ_scale")>
    Public Function TrIQScaler(Optional q As Double = 0.6) As TrIQScaler
        Return New TrIQScaler(q)
    End Function

    ''' <summary>
    ''' Make convolution of the spatial data for make the imaging render result soften
    ''' </summary>
    ''' <returns></returns>
    ''' <keywords>convolution</keywords>
    <ExportAPI("soften_scale")>
    <RApiReturn(GetType(SoftenScaler), GetType(SingleIonLayer))>
    Public Function softenScaler(Optional layer As SingleIonLayer = Nothing) As Object
        If layer Is Nothing Then
            Return New SoftenScaler()
        Else
            Return New SoftenScaler().DoIntensityScale(layer)
        End If
    End Function

    ''' <summary>
    ''' Trying to fill the missing spatial spot on the imaging via knn method 
    ''' </summary>
    ''' <param name="k"></param>
    ''' <param name="q"></param>
    ''' <returns></returns>
    <ExportAPI("knn_scale")>
    <RApiReturn(GetType(KNNScaler))>
    Public Function knnScaler(Optional k As Integer = 3,
                              Optional q As Double = 0.65,
                              Optional random As Boolean = False) As KNNScaler

        Return New KNNScaler(k, q, random)
    End Function

    <ExportAPI("denoise_scale")>
    <RApiReturn(GetType(DenoiseScaler))>
    Public Function denoiseScaler(Optional q As Double = 0.01) As DenoiseScaler
        Return New DenoiseScaler(q)
    End Function

    <ROperator("+")>
    Public Function construct_filter(a As Scaler, b As Scaler) As RasterPipeline
        Return New RasterPipeline(a, b)
    End Function

    <ROperator("+")>
    Public Function construct_pipeline(a As RasterPipeline, b As Scaler) As RasterPipeline
        Return a.Then(b)
    End Function
End Module
