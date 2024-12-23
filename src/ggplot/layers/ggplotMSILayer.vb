#Region "Microsoft.VisualBasic::ffb733c1fdd610f923013f558964a010, Rscript\Library\MSI_app\src\ggplot\layers\ggplotMSILayer.vb"

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

    '   Total Lines: 142
    '    Code Lines: 89 (62.68%)
    ' Comment Lines: 31 (21.83%)
    '    - Xml Docs: 87.10%
    ' 
    '   Blank Lines: 22 (15.49%)
    '     File Size: 5.75 KB


    '     Class ggplotMSILayer
    ' 
    '         Properties: colorLevels, hqx, pixelDrawer, threshold
    ' 
    '         Function: ApplyGauss, ApplyRasterFilter, (+2 Overloads) getIonlayer, MSIInterpolation, ScaleImageImpls
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports ggplot
Imports ggplot.layers
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.hqx
Imports Microsoft.VisualBasic.Imaging.Filters
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.MIME.Html.Render
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Spectra




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
    ''' ms-imaging rendering
    ''' </summary>
    Public MustInherit Class ggplotMSILayer : Inherits ggplotLayer

        ''' <summary>
        ''' the engine for do heatmap rendering:
        ''' 
        ''' + true: <see cref="PixelRender"/> is recommended when running on windows
        ''' + false: <see cref="RectangleRender"/> is working for unix platform
        ''' </summary>
        ''' <returns></returns>
        Public Property pixelDrawer As Boolean = True
        Public Property threshold As QuantizationThreshold
        ''' <summary>
        ''' the color level for the heatmap rendering
        ''' </summary>
        ''' <returns></returns>
        Public Property colorLevels As Integer = 255
        Public Property hqx As HqxScales = HqxScales.Hqx_4x

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="layer"></param>
        ''' <param name="ggplot"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' both filter and the <paramref name="layer"/> null reference error has been handled at here
        ''' </remarks>
        Public Shared Function ApplyRasterFilter(layer As SingleIonLayer, ggplot As ggplotMSI) As SingleIonLayer
            If Not ggplot.filter Is Nothing Then
                If layer IsNot Nothing Then
                    layer = ggplot.filter.Run(layer:=layer)
                    layer.MSILayer = layer.MSILayer _
                        .Where(Function(p) p.intensity >= 1) _
                        .ToArray
                End If
            End If

            Return layer
        End Function

        Public Shared Function ApplyGauss(MSI As Image, ggplot As ggplotMSI) As Image
            Dim gaussBlurs As Integer = ggplot.args.getValue("gauss_blur", ggplot.environment, 0)
#Disable Warning
            If gaussBlurs > 0 Then
                Dim bitmap As New Bitmap(MSI)
#Enable Warning
                For i As Integer = 0 To gaussBlurs
                    bitmap = GaussBlur.GaussBlur(bitmap)
                Next

                MSI = bitmap
            End If

            Return MSI
        End Function

        Public Shared Function MSIInterpolation(layer As SingleIonLayer, ggplot As ggplot.ggplot) As SingleIonLayer
            If layer Is Nothing Then
                Return Nothing
            End If
            If ggplot.args.getValue("knnFill", ggplot.environment, [default]:=False) Then
                Dim k As Integer = ggplot.args.getValue("knn", ggplot.environment, [default]:=3)
                Dim qcut As Double = ggplot.args.getValue("qcut", ggplot.environment, [default]:=0.8)

                layer = layer.KnnFill(k, k, qcut)
            End If

            Return layer
        End Function

        Public Function getIonlayer(a As MzAnnotation, mzdiff As Tolerance, ggplot As ggplot.ggplot) As SingleIonLayer
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim ion As SingleIonLayer = SingleIonLayer.GetLayer(a.productMz, base.reader, mzdiff)

            If a.annotation.IsSimpleNumber Then
                ion.IonMz = a.productMz
            Else
                ion.IonMz = $"{a.annotation} ({a.productMz.ToString("F4")})"
            End If

            Return ion
        End Function

        Public Function getIonlayer(mz As Double, mzdiff As Tolerance, ggplot As ggplot.ggplot) As SingleIonLayer
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim ion As SingleIonLayer = SingleIonLayer.GetLayer(mz, base.reader, mzdiff)

            Return ion
        End Function

        Public Function getIonlayer(mz As Double(), mzdiff As Tolerance, ggplot As ggplot.ggplot) As SingleIonLayer
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim ion As SingleIonLayer = SingleIonLayer.GetLayer(mz, base.reader, mzdiff)

            Return ion
        End Function

        ''' <summary>
        ''' a helper function for scale the input <paramref name="MSI"/> raster image as the plot region size.
        ''' </summary>
        ''' <param name="MSI"></param>
        ''' <param name="stream"></param>
        ''' <returns></returns>
        Public Function ScaleImageImpls(MSI As Image, stream As ggplotPipeline) As Image
            Dim css As CSSEnvirnment = stream.g.LoadEnvironment
            Dim rect As Rectangle = stream.canvas.PlotRegion(css)
            Dim ggplot As ggplotMSI = stream.ggplot

            If hqx <> HqxScales.None AndAlso hqx <> 1 Then
                ' scale size to the plot region
                ' MSI = Drawer.ScaleLayer(CType(MSI, Bitmap), rect.Width, rect.Height, InterpolationMode.HighQualityBicubic)
                MSI = New RasterScaler(New Bitmap(MSI)).Scale(hqx:=hqx)
                MSI = New RasterScaler(New Bitmap(MSI)).Scale(rect.Width, rect.Height)
            End If

            MSI = ApplyGauss(MSI, ggplot)

            Return MSI
        End Function
    End Class
End Namespace
