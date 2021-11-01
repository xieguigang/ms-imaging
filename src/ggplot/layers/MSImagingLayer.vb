#Region "Microsoft.VisualBasic::ae9e44566fb03d0844e959258f757387, Rscript\Library\MSI_app\src\ggplot\MSImagingLayer.vb"

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

' Class MSImagingLayer
' 
'     Properties: cutoff
' 
'     Function: Plot
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Imaging
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports any = Microsoft.VisualBasic.Scripting
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace layers

    Public Class MSImagingLayer : Inherits ggplotMSILayer

        Public Property cutoff As DoubleRange = Nothing

        Public Overrides Function Plot(g As IGraphics,
                                       canvas As GraphicsRegion,
                                       baseData As ggplotData,
                                       x() As Double,
                                       y() As Double,
                                       scale As DataScaler,
                                       ggplot As ggplot.ggplot,
                                       theme As Theme) As IggplotLegendElement

            Dim args As list = reader.args
            Dim mz As Double() = REnv.asVector(Of Double)(args.getByName("mz"))
            Dim mzdiff As Tolerance = args.getValue(Of Tolerance)("mzdiff", ggplot.environment)

            If mz.Any(Function(mzi) mzi <= 0) Then
                Throw New InvalidProgramException($"invalid ion m/z value '{mz.Where(Function(mzi) mzi <= 0).First}'!")
            End If
            If mzdiff Is Nothing Then
                mzdiff = Tolerance.DeltaMass(0.1)
                ggplot.environment.AddMessage("missing 'tolerance' parameter, use the default da:0.1 as mzdiff tolerance value!")
            End If

            Dim rect As Rectangle = canvas.PlotRegion
            Dim MSI As Image
            Dim engine As Renderer = If(pixelDrawer, New PixelRender, New RectangleRender)
            Dim colorSet As String
            Dim ion As SingleIonLayer = getIonlayer(mz, mzdiff, ggplot)

            If colorMap Is Nothing Then
                colorSet = theme.colorSet
            Else
                colorSet = any.ToString(colorMap.colorMap)
            End If

            MSI = engine.RenderPixels(ion.MSILayer, ion.DimensionSize, Nothing, cutoff:=cutoff, colorSet:=colorSet)
            MSI = Drawer.ScaleLayer(MSI, rect.Width, rect.Height, InterpolationMode.Bilinear)

            Call g.DrawImage(MSI, rect)

            If mz.Length > 1 Then
                Return Nothing
            Else
                Return New legendColorMapElement With {
                    .width = canvas.Padding.Right * (3 / 4),
                    .height = rect.Height,
                    .colorMapLegend = New ColorMapLegend(colorSet, 100) With {
                        .format = "G3",
                        .tickAxisStroke = Stroke.TryParse(theme.legendTickAxisStroke).GDIObject,
                        .tickFont = CSSFont.TryParse(theme.legendTickCSS).GDIObject(g.Dpi),
                        .ticks = ion.GetIntensity.Range.CreateAxisTicks,
                        .title = $"m/z {mz(Scan0).ToString("F3")}",
                        .titleFont = CSSFont.TryParse(theme.legendTitleCSS).GDIObject(g.Dpi),
                        .noblank = True,
                        .legendOffsetLeft = canvas.Padding.Right / 10
                    }
                }
            End If
        End Function
    End Class

End Namespace