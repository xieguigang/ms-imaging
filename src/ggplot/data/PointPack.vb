Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports Microsoft.VisualBasic.Imaging.Math2D

Namespace data

    ''' <summary>
    ''' A collection of the <see cref="PixelData"/>
    ''' </summary>
    Public Class PointPack

        ''' <summary>
        ''' A collection of the <see cref="PixelData"/> for create heatmap rendering
        ''' </summary>
        ''' <returns></returns>
        Public Property pixels As PixelData()
        ''' <summary>
        ''' the canvas size of the ms-imaging
        ''' </summary>
        ''' <returns></returns>
        Public Property dimension As Size

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