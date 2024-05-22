#Region "Microsoft.VisualBasic::d02af0ada1b107e54740e183f65bd0de, Rscript\Library\MSI_app\src\ggplot\layers\ggplotMSILayer.vb"

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

    '   Total Lines: 112
    '    Code Lines: 69 (61.61%)
    ' Comment Lines: 25 (22.32%)
    '    - Xml Docs: 84.00%
    ' 
    '   Blank Lines: 18 (16.07%)
    '     File Size: 4.51 KB


    '     Class ggplotMSILayer
    ' 
    '         Properties: colorLevels, pixelDrawer, threshold
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
                    layer = ggplot.filter(layer)
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

        Public Shared Function ScaleImageImpls(MSI As Image, stream As ggplotPipeline) As Image
            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim ggplot As ggplotMSI = stream.ggplot

            ' scale size to the plot region
            ' MSI = Drawer.ScaleLayer(CType(MSI, Bitmap), rect.Width, rect.Height, InterpolationMode.HighQualityBicubic)
            MSI = New RasterScaler(CType(MSI, Bitmap)).Scale(hqx:=HqxScales.Hqx_4x)
            MSI = New RasterScaler(CType(MSI, Bitmap)).Scale(rect.Width, rect.Height)
            MSI = ApplyGauss(MSI, ggplot)

            Return MSI
        End Function
    End Class
End Namespace
