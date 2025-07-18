﻿#Region "Microsoft.VisualBasic::9b6179196492c16e47b2f9d7e05eae81, Rscript\Library\MSI_app\src\builders\BuildFilters.vb"

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

    '   Total Lines: 54
    '    Code Lines: 44 (81.48%)
    ' Comment Lines: 0 (0.00%)
    '    - Xml Docs: 0.00%
    ' 
    '   Blank Lines: 10 (18.52%)
    '     File Size: 1.84 KB


    ' Module BuildFilters
    ' 
    '     Function: FromArray, FromFile, FromVector
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports Microsoft.VisualBasic.Language
Imports SMRUCC.Rsharp.Interpreter
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.DataSets
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Vectorization
Imports REnv = SMRUCC.Rsharp.Runtime

Module BuildFilters

    <Extension>
    Public Function FromVector(vec As VectorLiteral, env As Environment) As [Variant](Of Message, RasterPipeline)
        Dim vals As Object = vec.Evaluate(env)

        If Program.isException(vals) Then
            Return DirectCast(vals, Message)
        Else
            Return FromArray(REnv.asVector(Of Object)(vals))
        End If
    End Function

    Public Function FromArray(v As Array) As RasterPipeline
        Dim scales As New List(Of Scaler)

        If v IsNot Nothing AndAlso v.Length = 1 Then
            If TypeOf v(0) Is RasterPipeline Then
                Return DirectCast(v(0), RasterPipeline)
            End If
        End If

        If REnv.isVector(Of String)(v) Then
            Call scales.AddRange(CLRVector.asCharacter(v).Select(Function(si) Scaler.Parse(si)))
        Else
            Call scales.AddRange(DirectCast(REnv.asVector(Of Scaler)(v), Scaler()))
        End If

        Dim pip As New RasterPipeline

        For Each opt As Scaler In scales
            pip = pip.Then(opt)
        Next

        Return pip
    End Function

    Public Function FromFile(file As Stream) As RasterPipeline
        Dim lines As String() = New StreamReader(file).ReadToEnd.LineTokens
        Dim pip As RasterPipeline = RasterPipeline.Parse(lines)
        Return pip
    End Function
End Module
