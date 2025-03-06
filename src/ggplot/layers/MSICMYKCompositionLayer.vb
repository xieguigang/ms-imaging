#Region "Microsoft.VisualBasic::f49da734f72f47eaef74cb96009437d1, Rscript\Library\MSI_app\src\ggplot\layers\MSICMYKCompositionLayer.vb"

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

    '   Total Lines: 78
    '    Code Lines: 64 (82.05%)
    ' Comment Lines: 4 (5.13%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 10 (12.82%)
    '     File Size: 3.37 KB


    '     Class MSICMYKCompositionLayer
    ' 
    '         Properties: cyan, key, magenta, mzdiff, raster
    '                     yellow
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render

Namespace layers

    Public Class MSICMYKCompositionLayer : Inherits ggplotMSILayer

        ''' <summary>
        ''' the raster image background for the TIC overlaps
        ''' </summary>
        ''' <returns></returns>
        Public Property raster As Image

        Public Property cyan As MzAnnotation
        Public Property magenta As MzAnnotation
        Public Property yellow As MzAnnotation
        Public Property key As MzAnnotation
        Public Property mzdiff As Tolerance

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim rect As Rectangle = stream.canvas.PlotRegion(css)
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim engine As New RectangleRender(ggplot.driver, heatmapRender:=False)
            Dim c As SingleIonLayer = getIonlayer(cyan, mzdiff, ggplot)
            Dim m As SingleIonLayer = getIonlayer(magenta, mzdiff, ggplot)
            Dim y As SingleIonLayer = getIonlayer(yellow, mzdiff, ggplot)
            Dim k As SingleIonLayer = getIonlayer(key, mzdiff, ggplot)
            Dim dims As Size

            If Not ggplot.dimension_size.IsEmpty Then
                dims = ggplot.dimension_size
            Else
                dims = MSIRGBCompositionLayer.getDimSize(c, m, y, k)
            End If

            c = ApplyRasterFilter(c, ggplot)
            m = ApplyRasterFilter(m, ggplot)
            y = ApplyRasterFilter(y, ggplot)
            k = ApplyRasterFilter(k, ggplot)

            Using buf As IGraphics = DriverLoad.CreateGraphicsDevice(dims, Color.Black)
                Call engine.ChannelCompositions(
                    buf,
                    region:=New GraphicsRegion With {.Size = dims, .Padding = Padding.Zero},
                    C:=c,
                    M:=m,
                    Y:=y,
                    K:=k,
                    dimension:=dims,
                    background:=stream.theme.gridFill
                )

                stream.g.DrawImage(ScaleImageImpls(DirectCast(buf, GdiRasterGraphics).ImageResource, stream), rect)
            End Using

            Return New legendGroupElement With {
                .legends = (From legend As LegendObject In {
                    MSIRGBCompositionLayer.legend("cyan", stream.theme, c),
                    MSIRGBCompositionLayer.legend("magenta", stream.theme, m),
                    MSIRGBCompositionLayer.legend("yellow", stream.theme, y),
                    MSIRGBCompositionLayer.legend("black", stream.theme, k)
                } Where Not legend Is Nothing).ToArray
            }
        End Function
    End Class
End Namespace
