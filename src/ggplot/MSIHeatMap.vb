﻿#Region "Microsoft.VisualBasic::00e392847b79576bafea5c52c466d014, Rscript\Library\MSI_app\src\ggplot\MSIHeatMap.vb"

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

    '   Total Lines: 30
    '    Code Lines: 24
    ' Comment Lines: 1
    '   Blank Lines: 5
    '     File Size: 1005 B


    ' Class MSIHeatMap
    ' 
    '     Properties: B, dimension, G, R
    ' 
    '     Function: CreateLayer
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Public Class MSIHeatMap

    Public Property R As SingleIonLayer
    Public Property G As SingleIonLayer
    Public Property B As SingleIonLayer

    Public Property dimension As Size

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
End Class
