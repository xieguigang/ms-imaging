#Region "Microsoft.VisualBasic::116b56066142f7fdb9e1b1c3d897b42a, Rscript\Library\MSI_app\src\ggplot\data\PointPack.vb"

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

    '   Total Lines: 57
    '    Code Lines: 30 (52.63%)
    ' Comment Lines: 19 (33.33%)
    '    - Xml Docs: 100.00%
    ' 
    '   Blank Lines: 8 (14.04%)
    '     File Size: 2.05 KB


    '     Class PointPack
    ' 
    '         Properties: dimension, pixels, size
    ' 
    '         Function: GetDimensionSize, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports Microsoft.VisualBasic.Imaging.Math2D

Namespace data

    ''' <summary>
    ''' A collection of the <see cref="PixelData"/>
    ''' </summary>
    Public Class PointPack : Implements IMSILayer

        ''' <summary>
        ''' A collection of the <see cref="PixelData"/> for create heatmap rendering
        ''' </summary>
        ''' <returns></returns>
        Public Property pixels As PixelData() Implements IMSILayer.MSILayer
        ''' <summary>
        ''' the canvas size of the ms-imaging
        ''' </summary>
        ''' <returns></returns>
        Public Property dimension As Size Implements IMSILayer.DimensionSize

        ''' <summary>
        ''' get the element number of the <see cref="PixelData"/> collection
        ''' </summary>
        ''' <returns>this property will returns ZERO if the array is null</returns>
        Public ReadOnly Property size As Integer
            Get
                Return pixels.TryCount
            End Get
        End Property

        ''' <summary>
        ''' 这个函数会自动校准位置，尽量将目标多边形区域放置在中间
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDimensionSize() As Size
            If dimension.IsEmpty Then
                Dim rect As RectangleF = New Polygon2D(pixels).GetRectangle
                Dim offset As PointF = rect.Location
                Dim right As Double = offset.X + rect.Width + offset.X
                Dim height As Double = offset.Y + rect.Height + offset.Y

                Return New Size(right, height)
            Else
                Return dimension
            End If
        End Function

        Public Overrides Function ToString() As String
            With GetDimensionSize()
                Return $"canvas(w={ .Width},h={ .Height}) contains {pixels.Length} spots"
            End With
        End Function

    End Class
End Namespace
