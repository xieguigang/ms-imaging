#Region "Microsoft.VisualBasic::fb5621c95ed4b4860eb83abef22115d8, G:/mzkit/Rscript/Library/MSI_app/src//ggplot/filterPipelineBuilder.vb"

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

    '   Total Lines: 55
    '    Code Lines: 45
    ' Comment Lines: 0
    '   Blank Lines: 10
    '     File Size: 1.69 KB


    ' Module filterPipelineBuilder
    ' 
    '     Function: BuildPipeline
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine
Imports SMRUCC.Rsharp.Interpreter.ExecuteEngine.ExpressionSymbols.Operators
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components

Public Module filterPipelineBuilder

    <Extension>
    Friend Function BuildPipeline(bin As BinaryExpression, env As Environment, pip As RasterPipeline) As Object
        Dim start As Expression = bin.left
        Dim right As Expression = bin.right
        Dim pipEval As Object

        If TypeOf start Is BinaryExpression Then
            pipEval = DirectCast(start, BinaryExpression).BuildPipeline(env, pip)

            If TypeOf pipEval Is Message Then
                Return pipEval
            Else
                pip = pipEval
            End If
        Else
            pipEval = start.Evaluate(env)

            If TypeOf pipEval Is Message Then
                Return pipEval
            Else
                Call pip.Add(pipEval)
            End If
        End If

        If TypeOf right Is BinaryExpression Then
            pipEval = DirectCast(right, BinaryExpression).BuildPipeline(env, pip)

            If TypeOf pipEval Is Message Then
                Return pipEval
            Else
                pip = pipEval
            End If
        Else
            pipEval = right.Evaluate(env)

            If TypeOf pipEval Is Message Then
                Return pipEval
            Else
                Call pip.Add(pipEval)
            End If
        End If

        Return pip
    End Function

End Module
