
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class onebound(Of K)
    Public NotInheritable Class evaluator
        Private ReadOnly m As model
        Private ReadOnly longest_sequence As UInt32

        Public Sub New(ByVal m As model)
            Me.New(m, max_uint32)
        End Sub

        Public Sub New(ByVal m As model, ByVal longest_Sequence As UInt32)
            assert(m IsNot Nothing)
            assert(longest_Sequence > 0)
            Me.m = m
            Me.longest_sequence = longest_Sequence
        End Sub

        Private NotInheritable Class result
            Public ReadOnly splitters As vector(Of UInt32)
            Public ReadOnly possibility As Double

            Public Sub New(ByVal splitters As vector(Of UInt32), ByVal possibility As Double)
                assert(splitters IsNot Nothing)
                assert(possibility >= 0)
                Me.splitters = splitters
                Me.possibility = possibility
            End Sub

            Public Function muliply(ByVal prefix_splitters As vector(Of UInt32), ByVal possibility As Double) As result
                prefix_splitters.emplace_back(splitters)
                Return New result(prefix_splitters, Me.possibility * possibility)
            End Function
        End Class

        Private Function evaluate(ByVal v As vector(Of K),
                                  ByVal i As UInt32,
                                  ByVal splitters As vector(Of UInt32),
                                  ByVal possibility As Double,
                                  ByVal cache As vector(Of result)) As result
            assert(v IsNot Nothing)
            assert(splitters IsNot Nothing)
            assert(cache IsNot Nothing)
            assert(v.size() > i)
            If i = v.size() - 1 OrElse possibility = 0 Then
                Return New result(splitters, possibility)
            End If

            If i - If(splitters.empty(), uint32_0, splitters.back()) > longest_sequence Then
                Return New result(splitters, 0)
            End If

            Dim l As result = Nothing
            Dim d As Double = 0
            d = m.affinity(v(i), v(i + uint32_1))
            d = 1 - d
            If d > 0 Then
                If cache(i) IsNot Nothing Then
                    l = cache(i)
                Else
                    Dim s As vector(Of UInt32) = Nothing
                    s = New vector(Of UInt32)()
                    s.emplace_back(i)
                    l = evaluate(v, i + uint32_1, s, 1, cache)
                    assert(cache(i) Is Nothing)
                    cache(i) = l
                End If
                l = l.muliply(splitters.CloneT(), d * possibility)
            End If

            Dim r As result = Nothing
            d = 1 - d
            If d > 0 Then
                r = evaluate(v, i + uint32_1, splitters.CloneT(), d * possibility, cache)
            End If

            assert((l IsNot Nothing AndAlso r Is Nothing))
            If l Is Nothing Then
                Return r
            End If
            If r Is Nothing Then
                Return l
            End If
            ' Prefer longest match.
            If l.possibility <= r.possibility Then
                Return r
            End If
            Return l
        End Function

        Default Public ReadOnly Property eva(ByVal v As vector(Of K)) As vector(Of vector(Of K))
            Get
                Dim er As result = Nothing
                er = evaluate(v,
                              uint32_0,
                              New vector(Of UInt32)(),
                              1,
                              vector.repeat_of(Of result)(Nothing, v.size()))
                If er.splitters.empty() Then
                    Return vector.of(v.CloneT())
                End If

                Dim r As vector(Of vector(Of K)) = Nothing
                r = New vector(Of vector(Of K))()
                For i As UInt32 = 0 To er.splitters.size()
                    Dim skip As UInt32 = 0
                    skip = If(i = 0, uint32_0, er.splitters(i - uint32_1) + uint32_1)
                    r.emplace_back(v.stream().
                                     skip(skip).
                                     limit(If(i = er.splitters.size(), v.size(), er.splitters(i) + uint32_1) - skip).
                                     collect_to(Of vector(Of K))())
                Next
                Return r
            End Get
        End Property
    End Class
End Class
