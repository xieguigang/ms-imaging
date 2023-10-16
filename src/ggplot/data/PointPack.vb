Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports Microsoft.VisualBasic.Imaging.Math2D

Public Class PointPack

    Public Property pixels As PixelData()
    Public Property dimension As Size

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

End Class