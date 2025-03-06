#Region "Microsoft.VisualBasic::48eb8bfe7f80c690a3cd28d2b1045b82, Rscript\Library\MSI_app\src\ggplot\MSIHeatMap.vb"

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

    '   Total Lines: 92
    '    Code Lines: 68 (73.91%)
    ' Comment Lines: 11 (11.96%)
    '    - Xml Docs: 90.91%
    ' 
    '   Blank Lines: 13 (14.13%)
    '     File Size: 3.08 KB


    ' Class MSIHeatMap
    ' 
    '     Properties: B, C, dimension, G, K
    '                 M, R, Y
    ' 
    '     Function: CreateLayer, MeasureHeight, MeasureWidth, ToString, UnionLayers
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports Microsoft.VisualBasic.Math.LinearAlgebra

''' <summary>
''' rgb/cmyk data source mapping
''' </summary>
Public Class MSIHeatMap

#Region "rgb"
    Public Property R As SingleIonLayer
    Public Property G As SingleIonLayer
    Public Property B As SingleIonLayer
#End Region

#Region "cmyk"
    Public Property C As SingleIonLayer
    Public Property M As SingleIonLayer
    Public Property Y As SingleIonLayer
    Public Property K As SingleIonLayer
#End Region

    Public Property dimension As Size

    Public Overrides Function ToString() As String
        Return $"[{dimension.Width} x {dimension.Height}]"
    End Function

    Public Shared Function CreateLayer(layerName As String, pixels As Point(), heatmap As Vector) As SingleIonLayer
        ' scale to [0,1]
        heatmap = (heatmap - heatmap.Min) / (heatmap.Max - heatmap.Min)

        Return New SingleIonLayer With {
            .IonMz = layerName,
            .MSILayer = pixels _
                .Select(Function(pt, i)
                            Return New PixelData With {
                                .intensity = heatmap(i),
                                .x = pt.X,
                                .y = pt.Y
                            }
                        End Function) _
                .ToArray
        }
    End Function

    Private Shared Function MeasureWidth(layerR As SingleIonLayer, layerG As SingleIonLayer, layerB As SingleIonLayer) As Integer
        Return {
            layerR.DimensionSize.Width,
            layerG.DimensionSize.Width,
            layerB.DimensionSize.Width
        }.Max
    End Function

    Private Shared Function MeasureHeight(layerR As SingleIonLayer, layerG As SingleIonLayer, layerB As SingleIonLayer) As Integer
        Return {
            layerR.DimensionSize.Height,
            layerG.DimensionSize.Height,
            layerB.DimensionSize.Height
        }.Max
    End Function

    ''' <summary>
    ''' Create a heatmap imaging plot source data object
    ''' </summary>
    ''' <param name="layerR"></param>
    ''' <param name="layerG"></param>
    ''' <param name="layerB"></param>
    ''' <returns></returns>
    Public Shared Function UnionLayers(layerR As SingleIonLayer,
                                       layerG As SingleIonLayer,
                                       layerB As SingleIonLayer,
                                       Optional dims As Size = Nothing) As MSIHeatMap

        If dims.IsEmpty Then
            Dim w As Integer = MeasureWidth(layerR, layerG, layerB)
            Dim h As Integer = MeasureHeight(layerR, layerG, layerB)

            dims = New Size With {
                .Width = w,
                .Height = h
            }
        End If

        Return New MSIHeatMap With {
            .R = layerR,
            .G = layerG,
            .B = layerB,
            .dimension = dims
        }
    End Function
End Class
