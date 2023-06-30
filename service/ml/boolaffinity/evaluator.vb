
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class boolaffinity(Of K)
    Public NotInheritable Class evaluator
        Private ReadOnly m As model

        Public Sub New(ByVal m As model)
            assert(Not m Is Nothing)
            Me.m = m
        End Sub

        Public Structure result
            Public ReadOnly true_possibility As Double
            Public ReadOnly false_possibility As Double

            Public Sub New(ByVal true_possibility As Double, ByVal false_possibility As Double)
                Me.true_possibility = true_possibility
                Me.false_possibility = false_possibility
            End Sub
        End Structure

        Default Public ReadOnly Property e(ByVal v As vector(Of const_pair(Of K, Boolean))) As result
            Get
                If v.null_or_empty Then
                    Return New result(1, 1)
                End If
                Dim tp As Double = 0
                Dim fp As Double = 0
                tp = 1
                fp = 1
                For i As UInt32 = 0 To v.size() - uint32_1
                    Dim p As [optional](Of model.affinity) = Nothing
                    p = m(v(i).first)
                    If p.empty() Then
                        Continue For
                    End If
                    Dim ttp As Double = 0
                    Dim tfp As Double = 0
                    ttp = (+p).true_affinity
                    tfp = (+p).false_affinity
                    If Not v(i).second Then
                        ttp = (+p).false_affinity
                        tfp = (+p).true_affinity
                    End If
                    tp *= ttp
                    fp *= tfp
                Next
                Return New result(tp, fp)
            End Get
        End Property

        Public Function evaluate(ByVal ParamArray v() As const_pair(Of K, Boolean)) As result
            Return Me(vector.of(v))
        End Function

        Public Function significant(ByVal v As vector(Of const_pair(Of K, Boolean)),
                                    ByVal threshold As Double) As ternary
            Dim r As result = Nothing
            r = e(v)
            If r.true_possibility / r.false_possibility >= threshold Then
                Return ternary.true
            End If
            If r.false_possibility / r.true_possibility >= threshold Then
                Return ternary.false
            End If
            Return ternary.unknown
        End Function
    End Class
End Class
