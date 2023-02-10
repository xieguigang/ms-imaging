#Region "Microsoft.VisualBasic::edf0259baae39ef56e9320d5a17e0f0b, mzkit\Rscript\Library\MSI_app\src\ggplot\layers\MSImagingLayer.vb"

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

    '   Total Lines: 129
    '    Code Lines: 103
    ' Comment Lines: 5
    '   Blank Lines: 21
    '     File Size: 5.44 KB


    '     Class MSImagingLayer
    ' 
    '         Properties: TrIQ
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports ggplot
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.hqx
Imports Microsoft.VisualBasic.Imaging.Filters
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports SMRUCC.Rsharp.Runtime.Internal.Object
Imports any = Microsoft.VisualBasic.Scripting
Imports REnv = SMRUCC.Rsharp.Runtime

Namespace layers

    Public Class MSImagingLayer : Inherits ggplotMSILayer

        Public Property TrIQ As Double = 0.65

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim args As list = reader.args
            Dim mz As Double() = DirectCast(REnv.asVector(Of Double)(args.getByName("mz")), Double())
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
            Dim engine As New RectangleRender(ggplot.driver, heatmapRender:=False)
            Dim colorSet As String
            Dim ion As SingleIonLayer = getIonlayer(mz, mzdiff, ggplot)
            Dim rawInto As Double() = ion.GetIntensity
            Dim dimension_size As Size = ggplot.GetDimensionSize(ion.DimensionSize)

            If Not ggplot.filter Is Nothing Then
                ion = ggplot.filter(ion)
                ion.MSILayer = ion.MSILayer _
                    .Where(Function(p) p.intensity >= 1) _
                    .ToArray
            End If

            Dim TrIQ As Double = Double.MaxValue  ' New TrIQThreshold().ThresholdValue(ion.GetIntensity, Me.TrIQ)
            Dim theme As Theme = stream.theme
            Dim gaussBlurs As Integer = ggplot.args.getValue("gauss_blur", ggplot.environment, 0)

            'If knnfill Then
            '    ion = ion.KnnFill(3, 3, 0.8)
            'End If

            If colorMap Is Nothing Then
                colorSet = stream.theme.colorSet
            Else
                colorSet = any.ToString(colorMap.colorMap)
            End If

            If colorLevels <= 0 Then
                colorLevels = 30
            End If

            MSI = engine.RenderPixels(
                pixels:=ion.MSILayer,
                dimension:=dimension_size,
                colorSet:=colorSet,
                defaultFill:=ggplot.ggplotTheme.gridFill,
                mapLevels:=colorLevels
            ).AsGDIImage

            ' scale size to the plot region
            ' MSI = Drawer.ScaleLayer(CType(MSI, Bitmap), rect.Width, rect.Height, InterpolationMode.HighQualityBicubic)
            MSI = New RasterScaler(CType(MSI, Bitmap)).Scale(hqx:=HqxScales.Hqx_4x)
            MSI = New RasterScaler(CType(MSI, Bitmap)).Scale(rect.Width, rect.Height)

            If gaussBlurs > 0 Then
                Dim bitmap As New Bitmap(MSI)

                For i As Integer = 0 To gaussBlurs
                    bitmap = GaussBlur.GaussBlur(bitmap)
                Next

                MSI = bitmap
            End If

            Call stream.g.DrawImage(MSI, rect)

            If mz.Length > 1 Then
                Return Nothing
            Else
                Dim ticks As Double() = rawInto.Range.CreateAxisTicks

                If ticks.Any(Function(t) t = 0.0) Then
                    ticks = {rawInto.Min} _
                        .JoinIterates(ticks.Where(Function(t) t > 0)) _
                        .OrderBy(Function(d) d) _
                        .ToArray
                End If

                Return New legendColorMapElement With {
                    .width = stream.canvas.Padding.Right * (3 / 4),
                    .height = rect.Height,
                    .colorMapLegend = New ColorMapLegend(colorSet, 100) With {
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
            End If
        End Function
    End Class

End Namespace
