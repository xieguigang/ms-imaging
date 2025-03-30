#Region "Microsoft.VisualBasic::c15c7a366a29b9b4c41e749c3fdff6d8, Rscript\Library\MSI_app\src\ggplot\layers\MSITICOverlap.vb"

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

    '   Total Lines: 102
    '    Code Lines: 85 (83.33%)
    ' Comment Lines: 5 (4.90%)
    '    - Xml Docs: 60.00%
    ' 
    '   Blank Lines: 12 (11.76%)
    '     File Size: 4.33 KB


    '     Class MSITICOverlap
    ' 
    '         Properties: hqx, summary
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly.MarkupData.imzML
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplot
Imports ggplot.colors
Imports ggplot.elements.legend
Imports ggplot.layers
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.hqx
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports any = Microsoft.VisualBasic.Scripting
Imports HeatMapParameters = Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.HeatMapParameters

#If NET48 Then
#Else
Imports Bitmap = Microsoft.VisualBasic.Imaging.Bitmap
#End If

Namespace layers

    ''' <summary>
    ''' do overlaps of the TIC imaging plot in grayscale
    ''' </summary>
    Public Class MSITICOverlap : Inherits ggplotLayer

        Public Property summary As IntensitySummary
        Public Property hqx As HqxScales = HqxScales.Hqx_4x

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim reader As PixelReader = base.reader
            Dim summary As MSISummary = reader.GetSummary
            Dim pixels As PixelData() = summary.GetLayer(Me.summary) _
                .Where(Function(p) p.totalIon > 0) _
                .Select(Function(p)
                            Return New PixelData With {
                                .X = p.x,
                                .Y = p.y,
                                .intensity = p.totalIon
                            }
                        End Function) _
                .ToArray

            Dim colorSet As String = "gray"

            If Not colorMap Is Nothing Then
                If TypeOf colorMap Is ggplotColorPalette Then
                    colorSet = any.ToString(DirectCast(colorMap, ggplotColorPalette).colorMap)
                End If
            End If

            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim rect As Rectangle = stream.canvas.PlotRegion(css)
            Dim black = DirectCast(DriverLoad.CreateGraphicsDevice(reader.dimension.Scale(2), Color.Black), GdiRasterGraphics).ImageResource
            Dim heatmap As New HeatMapParameters(colorSet, 100, defaultFill:="black")
            Dim TIC As Bitmap = New RectangleRender(Drivers.GDI, False).RenderPixels(
                pixels:=pixels,
                dimension:=ggplot.GetDimensionSize(reader.dimension),
                heatmap:=heatmap
            ).AsGDIImage _
             .DoCall(Function(img) New Bitmap(img))

            If hqx <> HqxScales.None AndAlso hqx <> 1 Then
                TIC = New Drawing2D.HeatMap.RasterScaler(TIC).Scale(hqx:=hqx)
            End If

            stream.theme.gridFill = "transparent"
            stream.g.DrawImage(black, rect)

#If NETCOREAPP Then
            ' 20241030 there is a draw image resize bug
            ' try to avoid this bug by resize image manually
            Using skia As New Microsoft.VisualBasic.Drawing.Graphics(rect.Width, rect.Height, NameOf(Color.Transparent))
                Call skia.DrawImage(TIC, 0, 0, rect.Width, rect.Height)
                Call skia.Flush()

                Call stream.g.DrawImage(skia.ImageResource, rect.X, rect.Y, rect.Width, rect.Height)
            End Using
#Else
            Call stream.g.DrawImage(TIC, rect)
#End If
            Return Nothing
        End Function
    End Class
End Namespace
