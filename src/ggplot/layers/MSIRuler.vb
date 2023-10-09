#Region "Microsoft.VisualBasic::f0a4922ee7f6c79b75210a0ed0ae2809, mzkit\Rscript\Library\MSI_app\src\ggplot\layers\MSIRuler.vb"

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

    '   Total Lines: 33
    '    Code Lines: 27
    ' Comment Lines: 0
    '   Blank Lines: 6
    '     File Size: 1.09 KB


    '     Class MSIRuler
    ' 
    '         Properties: color
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Reader
Imports ggplot
Imports ggplot.elements.legend
Imports ggplot.layers

Namespace layers

    Public Class MSIRuler : Inherits ggplotLayer

        Public Property color As Color = Color.White
        Public Property width As Double? = 1000

        Public Overrides Function Plot(stream As ggplotPipeline) As IggplotLegendElement
            Dim ggplot As ggplotMSI = stream.ggplot
            Dim base = DirectCast(ggplot.base.reader, MSIReader)
            Dim reader As PixelReader = base.reader
            Dim resolution As Double = reader.resolution
            Dim dimsize As Size = ggplot.GetDimensionSize(reader.dimension)
            Dim rect As Rectangle = stream.canvas.PlotRegion

            Call New Ruler(stream.theme) With {.width = width} _
                .DrawOnCanvas(
                    g:=stream.g,
                    dimsize:=dimsize,
                    rect:=rect,
                    color:=color,
                    resolution:=resolution
                )

            Return Nothing
        End Function
    End Class
End Namespace
