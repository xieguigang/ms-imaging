#Region "Microsoft.VisualBasic::f896ce30e1949a6164047f1ced0b5ab5, G:/mzkit/Rscript/Library/MSI_app/src//ggplot/layers/MSIChannelLayer.vb"

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

    '   Total Lines: 104
    '    Code Lines: 77
    ' Comment Lines: 12
    '   Blank Lines: 15
    '     File Size: 4.35 KB


    '     Class MSIChannelLayer
    ' 
    ' 
    '         Enum Channels
    ' 
    '             Blue, Green, NA, Red
    ' 
    ' 
    ' 
    '  
    ' 
    '     Properties: channel
    ' 
    '     Function: getIonlayer, Plot, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.Math.Ms1
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports ggplot
Imports ggplot.colors
Imports ggplot.elements.legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports SMRUCC.Rsharp.Runtime.Internal.Object

Namespace layers

    ''' <summary>
    ''' The rgb color channel heatmap plot
    ''' </summary>
    Public Class MSIChannelLayer : Inherits ggplotMSILayer

        Public Enum Channels
            NA
            Red
            Green
            Blue
        End Enum

        ''' <summary>
        ''' check of the red/green/blue channel
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property channel As Channels
            Get
                Dim color As String = DirectCast(colorMap, ggplotColorLiteral).ToColor.ToHtmlColor

                Select Case color.ToLower
                    Case "#ff0000" : Return Channels.Red

                    Case "#00ff00", "#008000" : Return Channels.Green
                    Case "#0000ff" : Return Channels.Blue
                    Case Else
                        Return Channels.NA
                End Select
            End Get
        End Property

        ''' <summary>
        ''' the ion layer has already been knn interpolation at here
        ''' </summary>
        ''' <param name="ggplot"></param>
        ''' <returns></returns>
        Public Overloads Function getIonlayer(ggplot As ggplot.ggplot) As SingleIonLayer
            Dim args As list = reader.args
            Dim mz As Double = args.getValue(Of Double)("mz", ggplot.environment)
            Dim mzdiff As Tolerance = args.getValue(Of Tolerance)("mzdiff", ggplot.environment)

            If mz <= 0 Then
                Throw New InvalidProgramException("invalid ion m/z value!")
            End If
            If mzdiff Is Nothing Then
                mzdiff = Tolerance.DeltaMass(0.1)
                ggplot.environment.AddMessage("missing 'tolerance' parameter, use the default da:0.1 as mzdiff tolerance value!")
            End If

            Return MSIInterpolation(getIonlayer(mz, mzdiff, ggplot), ggplot)
        End Function

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim rect As Rectangle = stream.canvas.PlotRegion
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim ion As SingleIonLayer = getIonlayer(ggplot)
            Dim MSI As Image
            Dim engine As New RectangleRender(ggplot.driver, heatmapRender:=False)
            Dim color As String = DirectCast(colorMap, ggplotColorLiteral).ToColor.ToHtmlColor
            Dim colorSet As String = $"transparent,{color}"
            Dim dims As Size = ggplot.GetDimensionSize(ion.DimensionSize)

            Select Case color.ToLower
                Case "#ff0000"            ' red
                    MSI = engine.ChannelCompositions(ion.MSILayer, {}, {}, dims, background:="transparent").AsGDIImage
                Case "#00ff00", "#008000" ' green
                    MSI = engine.ChannelCompositions({}, ion.MSILayer, {}, dims, background:="transparent").AsGDIImage
                Case "#0000ff"            ' blue
                    MSI = engine.ChannelCompositions({}, {}, ion.MSILayer, dims, background:="transparent").AsGDIImage
                Case Else
                    MSI = engine.RenderPixels(ion.MSILayer, dims, colorSet:=colorSet).AsGDIImage
            End Select

            Call stream.g.DrawImage(ScaleImageImpls(MSI, stream), rect)

            Return New ggplotLegendElement With {
                .legend = New LegendObject With {
                    .color = color,
                    .fontstyle = stream.theme.legendLabelCSS,
                    .style = LegendStyles.Square,
                    .title = SingleIonLayer.ToString(ion)
                }
            }
        End Function

        Public Overrides Function ToString() As String
            Return $"[{DirectCast(colorMap, ggplotColorLiteral).ToColor.ToHtmlColor}] {reader.args!mz}"
        End Function
    End Class

End Namespace
