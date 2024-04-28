#Region "Microsoft.VisualBasic::08ca1a8b2d60ad5161c8a36518e6a1c9, E:/mzkit/Rscript/Library/MSI_app/src//ggplot/layers/MSImagingLayer.vb"

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

    '   Total Lines: 128
    '    Code Lines: 102
    ' Comment Lines: 8
    '   Blank Lines: 18
    '     File Size: 5.58 KB


    '     Class MSImagingLayer
    ' 
    '         Properties: raster, TrIQ
    ' 
    '         Function: MSIHeatmapRender, Plot, ScalerLegend
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.InteropServices
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports any = Microsoft.VisualBasic.Scripting

Namespace layers

    ''' <summary>
    ''' The single ion ms-imaging plot layer
    ''' </summary>
    Public Class MSImagingLayer : Inherits ggplotMSILayer

        Public Property TrIQ As Double = 0.65

        ''' <summary>
        ''' the annotation overlaps only works when <see cref="pixelDrawer"/>
        ''' option is set to true
        ''' </summary>
        ''' <returns></returns>
        Public Property raster As Bitmap

        Private Function MSIHeatmapRender(ion As SingleIonLayer, theme As Theme, ggplot As ggplotMSI,
                                          <Out> ByRef colorSet As String,
                                          <Out> ByRef colorLevels As Integer) As Image

            Dim engine As Renderer = If(pixelDrawer,
                New PixelRender(heatmapRender:=False, overlaps:=raster),
                New RectangleRender(Drivers.GDI, heatmapRender:=False)
            )
            Dim dimension_size As Size = ggplot.GetDimensionSize(ion.DimensionSize)

            If colorMap Is Nothing Then
                colorSet = theme.colorSet
            Else
                colorSet = any.ToString(colorMap.colorMap)
            End If
            If colorLevels <= 0 Then
                colorLevels = 30
            End If

            Return engine.RenderPixels(
                pixels:=ion.MSILayer,
                dimension:=dimension_size,
                colorSet:=colorSet,
                defaultFill:=ggplot.ggplotTheme.gridFill,
                mapLevels:=colorLevels
            ).AsGDIImage
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim args As list = reader.args
            Dim mz As Double() = CLRVector.asNumeric(args.getByName("mz"))
            Dim mzdiff As Tolerance = args.getValue(Of Tolerance)("mzdiff", ggplot.environment)
            Dim knnfill As Boolean = args.getValue(Of Boolean)("knnfill", ggplot.environment, False)

            If mz.Any(Function(mzi) mzi <= 0) Then
                Throw New InvalidProgramException($"invalid ion m/z value '{mz.Where(Function(mzi) mzi <= 0).First}'!")
            End If
            If mzdiff Is Nothing Then
                mzdiff = Tolerance.DeltaMass(0.1)
                ggplot.environment.AddMessage("missing 'tolerance' parameter, use the default da:0.1 as mzdiff tolerance value!")
            End If

            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim MSI As Image
            Dim colorSet As String = Nothing
            Dim colorLevels As Integer = Me.colorLevels
            Dim ion As SingleIonLayer = getIonlayer(mz, mzdiff, ggplot)
            Dim rawInto As Double() = ion.GetIntensity
            Dim theme As Theme = stream.theme

            ion = ApplyRasterFilter(ion, ggplot)
            MSI = MSIHeatmapRender(ion, theme, ggplot, colorSet, colorLevels)
            MSI = ScaleImageImpls(MSI, stream)

            Call stream.g.DrawImage(MSI, rect)

            If mz.Length > 1 Then
                Return Nothing
            Else
                Return ScalerLegend(mz, rawInto, stream, colorSet, colorLevels)
            End If
        End Function

        Private Function ScalerLegend(mz As Double(), rawInto As Double(), stream As ggplotPipeline, colorSet As String, colorLevels As Integer) As legendColorMapElement
            Dim theme As Theme = stream.theme
            Dim ticks As Double() = rawInto.Range.CreateAxisTicks
            Dim rect As Rectangle = stream.canvas.PlotRegion

            If ticks.Any(Function(t) t = 0.0) Then
                ticks = {rawInto.Min} _
                    .JoinIterates(ticks.Where(Function(t) t > 0)) _
                    .OrderBy(Function(d) d) _
                    .ToArray
            End If

            Return New legendColorMapElement With {
                .width = stream.canvas.Padding.Right * (3 / 4),
                .height = rect.Height,
                .colorMapLegend = New ColorMapLegend(colorSet, colorLevels) With {
                    .format = "G3",
                    .tickAxisStroke = Stroke.TryParse(theme.legendTickAxisStroke).GDIObject,
                    .tickFont = CSSFont.TryParse(theme.legendTickCSS).GDIObject(stream.g.Dpi),
                    .ticks = ticks,
                    .title = $"m/z {mz(Scan0).ToString("F3")}",
                    .titleFont = CSSFont.TryParse(theme.legendTitleCSS).GDIObject(stream.g.Dpi),
                    .noblank = True,
                    .legendOffsetLeft = stream.canvas.Padding.Right / 10
                }
            }
        End Function
    End Class

End Namespace
