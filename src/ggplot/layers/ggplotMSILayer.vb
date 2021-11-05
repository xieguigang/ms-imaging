#Region "Microsoft.VisualBasic::db3193d84e6f50354cf9bc6d8bde81db, Rscript\Library\MSI_app\src\ggplot\ggplotMSILayer.vb"

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

' Class ggplotMSILayer
' 
'     Properties: pixelDrawer
' 
'     Function: getIonlayer
' 
' /********************************************************************************/

#End Region

Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Imaging
Imports ggplot.layers

Namespace layers

    Public MustInherit Class ggplotMSILayer : Inherits ggplotLayer

        Public Property pixelDrawer As Boolean = False
        Public Property threshold As QuantizationThreshold

        Public Function getIonlayer(mz As Double, mzdiff As Tolerance, ggplot As ggplot.ggplot) As SingleIonLayer
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim ion As SingleIonLayer = SingleIonLayer.GetLayer(mz, base.reader, mzdiff)

            Return ion
        End Function

        Public Function getIonlayer(mz As Double(), mzdiff As Tolerance, ggplot As ggplot.ggplot) As SingleIonLayer
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim ion As SingleIonLayer = SingleIonLayer.GetLayer(mz, base.reader, mzdiff)

            Return ion
        End Function
    End Class
End Namespace
