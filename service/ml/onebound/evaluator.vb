
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class evaluator
        Private ReadOnly m As model

        Public Sub New(ByVal m As model)
            assert(Not m Is Nothing)
            Me.m = m
        End Sub

        Private Function evaluate(ByVal v As vector(Of K),
                                  ByVal i As UInt32,
                                  ByVal splitters As vector(Of UInt32),
                                  ByVal possibility As Double) As const_pair(Of vector(Of UInt32), Double)
            assert(Not v Is Nothing)
            assert(Not splitters Is Nothing)
            assert(v.size() > i)
            If i = v.size() - 1 OrElse possibility = 0 Then
                Return const_pair.of(splitters, possibility)
            End If

            Dim l As const_pair(Of vector(Of UInt32), Double) = Nothing
            If m.independence(v(i)) > 0 Then
                Dim s As vector(Of UInt32) = Nothing
                s = splitters.CloneT()
                s.emplace_back(i)
                l = evaluate(v, i + uint32_1, s, possibility * m.independence(v(i)))
            End If
            Dim r As const_pair(Of vector(Of UInt32), Double) = Nothing
            If m.affinity(v(i), v(i + uint32_1)) > 0 Then
                r = evaluate(v, i + uint32_1, splitters.CloneT(), possibility * m.affinity(v(i), v(i + uint32_1)))
            End If
            If l Is Nothing AndAlso r Is Nothing Then
                Return const_pair.of(splitters, double_0)
            End If
            If l Is Nothing Then
                Return r
            End If
            If r Is Nothing Then
                Return l
            End If
            ' Prefer longest match.
            If l.second <= r.second Then
                Return r
            End If
            Return l
        End Function

        Default Public ReadOnly Property eva(ByVal v As vector(Of K)) As vector(Of vector(Of K))
            Get
                Dim er As const_pair(Of vector(Of UInt32), Double) = Nothing
                er = evaluate(v, uint32_0, New vector(Of UInt32)(), 1)
                If er.first.empty() Then
                    Return vector.of(v.CloneT())
                End If

                Dim r As vector(Of vector(Of K)) = Nothing
                r = New vector(Of vector(Of K))()
                For i As UInt32 = 0 To er.first.size()
                    Dim skip As UInt32 = 0
                    skip = If(i = 0, uint32_0, er.first(i - uint32_1) + uint32_1)
                    r.emplace_back(v.stream().
                                     skip(skip).
                                     limit(If(i = er.first.size(), v.size(), er.first(i)) - skip + uint32_1).
                                     collect(Of vector(Of K))())
                Next
                Return r
            End Get
        End Property
    End Class
End Class
