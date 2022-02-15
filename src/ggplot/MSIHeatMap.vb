Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Public Class MSIHeatMap

    Public Property R As SingleIonLayer
    Public Property G As SingleIonLayer
    Public Property B As SingleIonLayer

    Public Property dimension As Size

    Public Shared Function CreateLayer(layerName As String, pixels As Point(), heatmap As Vector) As SingleIonLayer
        ' scale to [0,1]
        heatmap = (heatmap - heatmap.Min) / (heatmap.Max - heatmap.Min)

        Return New SingleIonLayer With {
            .IonMz = layerName,
            .MSILayer = pixels _
                .Select(Function(pt, i)
                            Return New PixelData With {
                                .intensity = heatmap(i),
                                .x = pt.X,
                                .y = pt.Y
                            }
                        End Function) _
                .ToArray
        }
    End Function
End Class
