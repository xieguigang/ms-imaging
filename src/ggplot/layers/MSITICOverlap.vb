#Region "Microsoft.VisualBasic::aaaf2be6e92ea1c05f13a76b3ec5fba9, Rscript\Library\MSI_app\src\ggplot\layers\MSITICOverlap.vb"

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

    '   Total Lines: 70
    '    Code Lines: 57 (81.43%)
    ' Comment Lines: 3 (4.29%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 10 (14.29%)
    '     File Size: 2.83 KB


    '     Class MSITICOverlap
    ' 
    '         Properties: summary
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
Imports any = Microsoft.VisualBasic.Scripting

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
    ''' do overlaps of the TIC imaging plot in grayscale
    ''' </summary>
    Public Class MSITICOverlap : Inherits ggplotLayer

        Public Property summary As IntensitySummary

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim reader As PixelReader = base.reader
            Dim summary As MSISummary = reader.GetSummary
            Dim pixels As PixelData() = summary.GetLayer(Me.summary) _
                .Where(Function(p) p.totalIon > 0) _
                .Select(Function(p)
                            Return New PixelData With {
                                .x = p.x,
                                .y = p.y,
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

            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim black = DirectCast(DriverLoad.CreateGraphicsDevice(reader.dimension.Scale(2), Color.Black), GdiRasterGraphics).ImageResource
            Dim TIC As Bitmap = New RectangleRender(Drivers.Default, False).RenderPixels(
                pixels:=pixels,
                dimension:=ggplot.GetDimensionSize(reader.dimension),
                colorSet:=colorSet,
                mapLevels:=250,
                defaultFill:="black"
            ).AsGDIImage _
             .DoCall(Function(img) New Bitmap(img))

            TIC = New Drawing2D.HeatMap.RasterScaler(TIC).Scale(hqx:=HqxScales.Hqx_4x)

            stream.theme.gridFill = "transparent"
            stream.g.DrawImage(black, rect)
            stream.g.DrawImage(TIC, rect)

            Return Nothing
        End Function
    End Class
End Namespace
