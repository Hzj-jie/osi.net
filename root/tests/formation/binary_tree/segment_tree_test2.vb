
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Friend Class segment_tree_case2
    Inherits [case]

    Protected ReadOnly verify As Boolean
    Private ReadOnly emp As Boolean

    Protected Sub New(ByVal verify As Boolean, ByVal emp As Boolean)
        Me.verify = verify
        Me.emp = emp
    End Sub

    Public Sub New(ByVal verify As Boolean)
        Me.New(verify, True)
    End Sub

    Protected Shared Function rnd_position() As Int64
        Return If(isreleasebuild(), rnd_int16(), rnd_int8())
    End Function

    Private Shared Function rnd_segment(ByRef min As Int64,
                                        ByRef max As Int64) As pair(Of Int64, Int64)
        Dim i As Int64 = 0
        i = rnd_position()
        Dim j As Int64 = 0
        j = rnd_position()
        If i > j Then
            swap(i, j)
        End If
        If i < min Then
            min = i
        End If
        If j > max Then
            max = j
        End If
        Return If(i <= j, pair.emplace_of(i, j), pair.emplace_of(j, i))
    End Function

    Private Shared Function rnd_small_segment() As pair(Of Int64, Int64)
        Dim i As Int64 = 0
        i = rnd_position()
        Return pair.emplace_of(i, i + rnd_uint8())
    End Function

    Protected Function prepare_segments(ByRef min As Int64,
                                        ByRef max As Int64) As vector(Of pair(Of Int64, pair(Of Int64, Int64)))
        min = max_int64
        max = min_int64
        Dim r As vector(Of pair(Of Int64, pair(Of Int64, Int64))) = Nothing
        r = New vector(Of pair(Of Int64, pair(Of Int64, Int64)))()
        For i As Int32 = 0 To rnd_int(10 * If(isreleasebuild(), 200, 1),
                                      20 * If(isreleasebuild(), 200, 1)) - 1
            r.emplace_back(pair.emplace_of(If(isreleasebuild(),
                                                If(emp, rnd_int64(), rnd_int()),
                                                rnd_int8()),
                                             rnd_segment(min, max)))
        Next
        Return r
    End Function

    Protected Sub stupid_find(ByVal segments As vector(Of pair(Of Int64, pair(Of Int64, Int64))),
                              ByVal p As Int64,
                              ByRef has_value As Boolean,
                              ByRef value As Int64)
        assert(Not segments Is Nothing)
        has_value = False
        value = 0
        assert(Not segments.empty())
        For j As UInt32 = 0 To segments.size() - uint32_1
            If segments(j).second.first <= p AndAlso
               segments(j).second.second >= p Then
                has_value = True
                If emp Then
                    value = segments(j).first
                Else
                    value += segments(j).first
                End If
            End If
        Next
    End Sub

    Private Function single_find_case(ByVal t As segment_tree(Of Int64),
                                      ByVal segments As vector(Of pair(Of Int64, pair(Of Int64, Int64)))) As Boolean
        assert(Not t Is Nothing)
        For i As Int32 = 0 To 1024 * If(isreleasebuild(), 10, 1) - 1
            Dim p As Int64 = 0
            p = rnd_position()
            Dim has_value As Boolean = False
            Dim value As Int64 = 0
            stupid_find(segments, p, has_value, value)
            Dim it As segment_tree(Of Int64).iterator = Nothing
            it = t.find(p)
            If has_value Then
                If assertion.is_not_null(it) Then
                    assertion.is_true((+it).has_value())
                    assertion.equal((+it).value(), value)
                End If
            Else
                assertion.is_true(it.is_end() OrElse Not (+it).has_value())
            End If
        Next
        Return True
    End Function

    Private Function range_find_case(ByVal t As segment_tree(Of Int64),
                                     ByVal segments As vector(Of pair(Of Int64, pair(Of Int64, Int64)))) As Boolean
        assert(Not t Is Nothing)
        For i As Int32 = 0 To 16 * If(isreleasebuild(), 8, 1) - 1
            Dim r As pair(Of Int64, Int64) = Nothing
            r = rnd_small_segment()
            Dim its As vector(Of pair(Of segment_tree(Of Int64).iterator, pair(Of Int64, Int64))) = Nothing
            its = t.find(r.first, r.second)
            If assertion.is_not_null(its) Then
                Dim v1() As pair(Of Boolean, Int64) = Nothing
                ReDim v1(CInt(r.second - r.first))
                For j As Int32 = 0 To array_size_i(v1) - 1
                    v1(j) = pair.emplace_of(False, int64_0)
                Next
                assert(Not its.empty())
                For j As UInt32 = 0 To its.size() - uint32_1
                    If assertion.is_not_null(its(j)) Then
                        If Not its(j).first.is_end() AndAlso
                           (+its(j).first).has_value() Then
                            For k As Int64 = max(its(j).second.first, r.first) To min(its(j).second.second, r.second)
                                v1(CInt(k - r.first)).first = True
                                v1(CInt(k - r.first)).second = (+its(j).first).value()
                            Next
                        End If
                    End If
                Next

                Dim v2() As pair(Of Boolean, Int64) = Nothing
                ReDim v2(CInt(r.second - r.first))
                For j As Int32 = 0 To array_size_i(v2) - 1
                    v2(j) = pair.emplace_of(False, int64_0)
                Next
                For j As Int64 = r.first To r.second
                    Dim hv As Boolean = False
                    Dim v As Int64 = 0
                    stupid_find(segments, j, hv, v)
                    v2(CInt(j - r.first)).first = hv
                    v2(CInt(j - r.first)).second = v
                Next

                assertion.array_equal(v1, v2)
            End If
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Dim min As Int64 = 0
        Dim max As Int64 = 0
        Dim s As vector(Of pair(Of Int64, pair(Of Int64, Int64))) = Nothing
        s = prepare_segments(min, max)
        Dim t As segment_tree(Of Int64) = Nothing
        t = New segment_tree(Of Int64)(min, max)
        assert(Not s.empty())
        For i As UInt32 = 0 To s.size() - uint32_1
            assertion.is_true(t.emplace(s(i).second.first, s(i).second.second, s(i).first))
        Next
        Return Not verify OrElse
               (single_find_case(t, s) AndAlso
                range_find_case(t, s))
    End Function
End Class

Public NotInheritable Class segment_tree_test2
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New segment_tree_case2(True), 32 * If(isreleasebuild(), 2, 1))
    End Sub
End Class

Public NotInheritable Class segment_tree_perf2
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New segment_tree_case2(False), 32 * If(isreleasebuild(), 2, 1)))
    End Sub
End Class
