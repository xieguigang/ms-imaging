#Region "Microsoft.VisualBasic::7aa19e2342db3f9b48508990830505b4, Rscript\Library\MSI_app\src\ggplot\MSIRGBCompositionLayer.vb"

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

    ' Class MSIRGBCompositionLayer
    ' 
    '     Properties: blue, green, red
    ' 
    '     Function: Plot
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Imaging
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D

Public Class MSIRGBCompositionLayer : Inherits ggplotMSILayer

    Public Property red As ggplotMSILayer
    Public Property blue As ggplotMSILayer
    Public Property green As ggplotMSILayer

    Public Overrides Function Plot(g As IGraphics,
                                   canvas As GraphicsRegion,
                                   baseData As ggplotData,
                                   x() As Double,
                                   y() As Double,
                                   scale As DataScaler,
                                   ggplot As ggplot.ggplot,
                                   theme As Theme) As IggplotLegendElement

        Dim rect As Rectangle = canvas.PlotRegion
        Dim MSI As Image
        Dim engine As Renderer = If(pixelDrawer, New PixelRender, New RectangleRender)
        Dim redLayer As SingleIonLayer = DirectCast(red, MSIChannelLayer).getIonlayer(ggplot)
        Dim greenLayer As SingleIonLayer = DirectCast(green, MSIChannelLayer).getIonlayer(ggplot)
        Dim blueLayer As SingleIonLayer = DirectCast(blue, MSIChannelLayer).getIonlayer(ggplot)
        Dim qcutRed As DoubleRange = {0, Renderer.AutoCheckCutMax(redLayer.GetIntensity, 0.8)}
        Dim qcutGreen As DoubleRange = {0, Renderer.AutoCheckCutMax(greenLayer.GetIntensity, 0.8)}
        Dim qcutBlue As DoubleRange = {0, Renderer.AutoCheckCutMax(blueLayer.GetIntensity, 0.8)}
        Dim dims As New Size With {
            .Width = {redLayer.DimensionSize.Width, greenLayer.DimensionSize.Width, blueLayer.DimensionSize.Width}.Max,
            .Height = {redLayer.DimensionSize.Height, greenLayer.DimensionSize.Height, blueLayer.DimensionSize.Height}.Max
        }

        MSI = engine.ChannelCompositions(redLayer, greenLayer, blueLayer, dims, Nothing, cut:=(qcutRed, qcutGreen, qcutBlue), background:="black")
        MSI = Drawer.ScaleLayer(MSI, rect.Width, rect.Height, InterpolationMode.Bilinear)

        Call g.DrawImage(MSI, rect)

        Return New legendGroupElement With {
            .legends = {
                New LegendObject With {.color = "red", .fontstyle = theme.legendLabelCSS, .style = LegendStyles.Square, .title = $"m/z {redLayer.IonMz.ToString("F4")}"},
                New LegendObject With {.color = "green", .fontstyle = theme.legendLabelCSS, .style = LegendStyles.Square, .title = $"m/z {greenLayer.IonMz.ToString("F4")}"},
                New LegendObject With {.color = "blue", .fontstyle = theme.legendLabelCSS, .style = LegendStyles.Square, .title = $"m/z {blueLayer.IonMz.ToString("F4")}"}
            }
        }
    End Function
End Class

