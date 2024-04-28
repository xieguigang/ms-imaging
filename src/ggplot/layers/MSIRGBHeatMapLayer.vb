﻿#Region "Microsoft.VisualBasic::cc6487a7d1bfe13bb8ae3f61cf873fe2, E:/mzkit/Rscript/Library/MSI_app/src//ggplot/layers/MSIRGBHeatMapLayer.vb"

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

    '   Total Lines: 60
    '    Code Lines: 50
    ' Comment Lines: 3
    '   Blank Lines: 7
    '     File Size: 2.51 KB


    '     Class MSIRGBHeatMapLayer
    ' 
    '         Function: legend, Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging

Namespace layers

    ''' <summary>
    ''' rgb render, just used for combine with the <see cref="MSIHeatMap"/> object
    ''' </summary>
    Public Class MSIRGBHeatMapLayer : Inherits ggplotMSILayer

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim MSI As Image
            Dim ggplot As ggplot.ggplot = stream.ggplot
            Dim data = DirectCast(ggplot.data, MSIHeatMap)
            Dim engine As New RectangleRender(ggplot.driver, heatmapRender:=True)
            Dim redLayer As SingleIonLayer = ApplyRasterFilter(data.R, ggplot)
            Dim greenLayer As SingleIonLayer = ApplyRasterFilter(data.G, ggplot)
            Dim blueLayer As SingleIonLayer = ApplyRasterFilter(data.B, ggplot)
            Dim dims As Size = data.dimension

            MSI = engine.ChannelCompositions(
                R:=redLayer,
                G:=greenLayer,
                B:=blueLayer,
                dimension:=dims,
                background:=stream.theme.gridFill
            ).AsGDIImage

            Call stream.g.DrawImage(ScaleImageImpls(MSI, stream), rect)

            Return New legendGroupElement With {
                .legends = (From legend As LegendObject In {
                    legend("red", stream.theme, redLayer),
                    legend("green", stream.theme, greenLayer),
                    legend("blue", stream.theme, blueLayer)
                } Where Not legend Is Nothing).ToArray
            }
        End Function

        Private Shared Function legend(color As String, theme As Theme, layer As SingleIonLayer) As LegendObject
            If layer Is Nothing Then
                Return Nothing
            Else
                Return New LegendObject With {
                    .color = color,
                    .fontstyle = theme.legendLabelCSS,
                    .style = LegendStyles.Square,
                    .title = SingleIonLayer.ToString(layer)
                }
            End If
        End Function
    End Class
End Namespace
