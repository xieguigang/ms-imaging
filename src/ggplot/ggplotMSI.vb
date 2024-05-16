#Region "Microsoft.VisualBasic::ad25e7e9cf5b521d872332e2d599a816, Rscript\Library\MSI_app\src\ggplot\ggplotMSI.vb"

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

    '   Total Lines: 125
    '    Code Lines: 87
    ' Comment Lines: 24
    '   Blank Lines: 14
    '     File Size: 4.58 KB


    ' Class ggplotMSI
    ' 
    '     Properties: dimension_size, filter
    ' 
    '     Constructor: (+1 Overloads) Sub New
    '     Function: createHeatmap, createMzPackReader, createPixelReader, createPointReader, CreateReader
    '               GetDimensionSize
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Assembly
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports ggplot
Imports ggplotMSImaging.data
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports SMRUCC.Rsharp.Runtime

''' <summary>
''' ggplot for ms-imaging
''' </summary>
Public Class ggplotMSI : Inherits ggplot.ggplot

    Public Property filter As RasterPipeline
    ''' <summary>
    ''' the dimension size of the ms-imaging file scan
    ''' </summary>
    ''' <returns></returns>
    Public Property dimension_size As Size

    Public Sub New(theme As Theme)
        MyBase.New(theme)

        titleOffset = 1.125
    End Sub

    ''' <summary>
    ''' returns the given <paramref name="dims"/> size if the 
    ''' <see cref="dimension_size"/> is empty or else returns 
    ''' the <see cref="dimension_size"/>.
    ''' </summary>
    ''' <param name="dims">
    ''' use this canvas size value as default if the raw data
    ''' file <see cref="dimension_size"/> value is empty.
    ''' </param>
    ''' <returns></returns>
    Public Function GetDimensionSize(dims As Size) As Size
        If dimension_size.IsEmpty Then
            Return dims
        Else
            Return dimension_size
        End If
    End Function

    ''' <summary>
    ''' create the reader from a given ggplot base <see cref="data"/> object
    ''' </summary>
    ''' <param name="mapping"></param>
    ''' <returns></returns>
    Public Overrides Function CreateReader(mapping As ggplot.ggplotReader) As ggplot.ggplotBase
        Select Case template
            Case GetType(mzPack) : Return createMzPackReader(mapping)
            Case GetType(MSIHeatMap) : Return createHeatmap()
            Case GetType(PixelData()) : Return createPixelReader()
            Case GetType(PointPack) : Return createPointReader()

            Case Else
                Throw New NotImplementedException(template.FullName)
        End Select
    End Function

    Private Function createPointReader() As ggplotBase
        Return New ggplotBase With {
            .reader = New MSIReader(DirectCast(data, PointPack), Me)
        }
    End Function

    Private Function createPixelReader() As ggplotBase
        Return New ggplotBase() With {
            .reader = New MSIReader(New PointPack With {
                .pixels = DirectCast(data, PixelData())
            }, Me)
        }
    End Function

    Private Function createHeatmap() As ggplotBase
        Return New ggplotBase() With {
            .reader = New HeatMapReader(DirectCast(data, MSIHeatMap))
        }
    End Function

    Private Function createMzPackReader(mapping As ggplot.ggplotReader) As ggplotBase
        Dim mzpack As mzPack = DirectCast(data, mzPack)
        Dim metadata = mzpack.metadata
        Dim scan_x = Val(metadata.TryGetValue("width", [default]:=0))
        Dim scan_y = Val(metadata.TryGetValue("height", [default]:=0))
        Dim println As Action(Of Object) = environment.WriteLineHandler
        Dim verbose As Boolean = environment.globalEnvironment.verboseOption(opt:=False)

        If dimension_size.IsEmpty Then
            If scan_x > 0 AndAlso scan_y > 0 Then
                dimension_size = New Size(scan_x, scan_y)

                If verbose Then
                    ' show information data about the msimaging dimension
                    ' size parameter
                    Call println({
                        $"use the ms-imaging canvas size from the internal metadata!",
                        $"internal_canvas_size: [{scan_x}x{scan_y}]"
                    })
                End If
            Else
                With New Polygon2D(mzpack.MS.Select(Function(scan) scan.GetMSIPixel))
                    dimension_size = New Size(.width, .height)
                End With
            End If
        End If

        Return New ggplotBase With {
            .reader = New MSIReader(mzpack, Me) With {
                .args = mapping.args,
                .[class] = mapping.class,
                .color = mapping.color,
                .label = mapping.label,
                .shape = mapping.shape,
                .title = mapping.title,
                .x = mapping.x,
                .y = mapping.y,
                .z = mapping.z
            }
        }
    End Function
End Class
