#Region "Microsoft.VisualBasic::4bd445771073234d5932925e18f3025d, Rscript\Library\MSI_app\src\ggplot\layers\MSImagingLayer.vb"

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

    '   Total Lines: 157
    '    Code Lines: 127 (80.89%)
    ' Comment Lines: 10 (6.37%)
    '    - Xml Docs: 80.00%
    ' 
    '   Blank Lines: 20 (12.74%)
    '     File Size: 6.93 KB


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
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports any = Microsoft.VisualBasic.Scripting
Imports HeatMapParameters = Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.HeatMapParameters

#If NET48 Then
#Else
Imports Pen = Microsoft.VisualBasic.Imaging.Pen
Imports Pens = Microsoft.VisualBasic.Imaging.Pens
Imports Brush = Microsoft.VisualBasic.Imaging.Brush
Imports Font = Microsoft.VisualBasic.Imaging.Font
Imports Brushes = Microsoft.VisualBasic.Imaging.Brushes
Imports SolidBrush = Microsoft.VisualBasic.Imaging.SolidBrush
Imports DashStyle = Microsoft.VisualBasic.Imaging.DashStyle
Imports Image = Microsoft.VisualBasic.Imaging.Image
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
Imports GraphicsPath = Microsoft.VisualBasic.Imaging.GraphicsPath
Imports FontStyle = Microsoft.VisualBasic.Imaging.FontStyle
#End If

Namespace layers

    ''' <summary>
    ''' The single ion ms-imaging plot layer
    ''' </summary>
    Public Class MSImagingLayer : Inherits ggplotMSILayer

        ''' <summary>
        ''' intensity scalar threshold by TrIQ algorithm
        ''' </summary>
        ''' <returns></returns>
        Public Property TrIQ As Double = 0.65
        Public Property IntensityRange As Double()

        ''' <summary>
        ''' the annotation overlaps only works when <see cref="pixelDrawer"/>
        ''' option is set to true
        ''' </summary>
        ''' <returns></returns>
        Public Property raster As Bitmap

        ''' <summary>
        ''' Rendering ion value as heatmap imaging output
        ''' </summary>
        ''' <param name="ion"></param>
        ''' <param name="theme"></param>
        ''' <param name="ggplot"></param>
        ''' <param name="colorSet"></param>
        ''' <param name="colorLevels"></param>
        ''' <returns></returns>
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

            Dim heatmap As New HeatMapParameters(colorSet, colorLevels,
                defaultFill:=If(raster IsNot Nothing, "transparent", ggplot.ggplotTheme.gridFill))

            Return engine.RenderPixels(
                pixels:=ion.MSILayer,
                dimension:=dimension_size,
                heatmap:=heatmap
            ).AsGDIImage
        End Function

        Private Function clamp(ion As SingleIonLayer) As SingleIonLayer
            If IntensityRange.IsNullOrEmpty OrElse IntensityRange.Max = 0.0 Then
                Return ion
            Else
                Dim intensityRange = Me.IntensityRange

                If intensityRange.Length = 1 Then
                    intensityRange = {0, intensityRange.Max}
                End If

                Return ion.Clamp(IntensityRange.Min, IntensityRange.Max)
            End If
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim args As list = reader.args
            Dim mz As Double() = CLRVector.asNumeric(args.getByName("mz"))
            Dim mzdiff As Tolerance = args.getValue(Of Tolerance)("mzdiff", ggplot.environment)
            Dim knnfill As Boolean = args.getValue("knnfill", ggplot.environment, False)

            If mz.Any(Function(mzi) mzi <= 0) Then
                Throw New InvalidProgramException($"invalid ion m/z value '{mz.Where(Function(mzi) mzi <= 0).First}'!")
            End If
            If mzdiff Is Nothing Then
                mzdiff = Tolerance.DeltaMass(0.1)
                ggplot.environment.AddMessage("missing 'tolerance' parameter, use the default da:0.1 as mzdiff tolerance value!")
            End If

            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim rect As Rectangle = stream.canvas.PlotRegion(css)
            Dim MSI As Image
            Dim colorSet As String = Nothing
            Dim colorLevels As Integer = Me.colorLevels
            Dim ion As SingleIonLayer = clamp(getIonlayer(mz, mzdiff, ggplot))
            Dim rawInto As Double() = ion.GetIntensity
            Dim theme As Theme = stream.theme

            ion = ApplyRasterFilter(ion, ggplot)
            MSI = MSIHeatmapRender(ion, theme, ggplot, colorSet, colorLevels)
            MSI = ScaleImageImpls(MSI, stream)

#If NETCOREAPP Then
            ' 20241030 there is a draw image resize bug
            ' try to avoid this bug by resize image manually
            Using skia As New Microsoft.VisualBasic.Drawing.Graphics(rect.Width, rect.Height, NameOf(Color.Transparent))
                skia.DrawImage(MSI, 0, 0, rect.Width, rect.Height)
                skia.Flush()

                MSI = skia.ImageResource
            End Using
#End If
            Call stream.g.DrawImage(MSI, rect.X, rect.Y, rect.Width, rect.Height)

            If mz.Length > 1 Then
                Return Nothing
            Else
                Return ScalerLegend(mz, rawInto, stream, colorSet, colorLevels)
            End If
        End Function

        Private Function ScalerLegend(mz As Double(), rawInto As Double(), stream As ggplotPipeline, colorSet As String, colorLevels As Integer) As legendColorMapElement
            Dim theme As Theme = stream.theme
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim ticks As Double() = rawInto.JoinIterates(IntensityRange).Range.CreateAxisTicks
            Dim rect As Rectangle = stream.canvas.PlotRegion(css)
            Dim padding As New PaddingLayout(stream.canvas.Padding.LayoutVector(css))

            If ticks.Any(Function(t) t = 0.0) Then
                ticks = {rawInto.Min} _
                    .JoinIterates(ticks.Where(Function(t) t > 0)) _
                    .OrderBy(Function(d) d) _
                    .ToArray
            End If

            Return New legendColorMapElement With {
                .width = padding.Right * (3 / 4),
                .height = rect.Height,
                .colorMapLegend = New ColorMapLegend(colorSet, colorLevels) With {
                    .format = "G3",
                    .tickAxisStroke = css.GetPen(Stroke.TryParse(theme.legendTickAxisStroke)),
                    .tickFont = css.GetFont(CSSFont.TryParse(theme.legendTickCSS)),
                    .ticks = ticks,
                    .title = $"m/z {mz(Scan0).ToString("F3")}",
                    .titleFont = css.GetFont(CSSFont.TryParse(theme.legendTitleCSS)),
                    .noblank = True,
                    .legendOffsetLeft = padding.Right / 10
                }
            }
        End Function
    End Class

End Namespace
