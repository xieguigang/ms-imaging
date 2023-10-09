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

        If REnv.isVector(Of String)(v) Then
            scales.AddRange(CLRVector.asCharacter(v).Select(Function(si) Scaler.Parse(si)))
        Else
            scales.AddRange(REnv.asVector(Of Scaler)(v))
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
