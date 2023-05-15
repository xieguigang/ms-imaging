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
