
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Friend NotInheritable Class segment_tree_case
    Inherits [case]

    Private ReadOnly verify As Boolean

    Public Sub New(ByVal verify As Boolean)
        Me.verify = verify
    End Sub

    Private Shared Function rnd_position() As Int64
        Return If(isreleasebuild(), rnd_int16(), rnd_int8())
    End Function

    Private Shared Function prepare_segments() As vector(Of pair(Of Int64, pair(Of Int64, Int64)))
        Dim s As [set](Of Int64) = Nothing
        s = New [set](Of Int64)()
        For i As Int32 = 0 To rnd_int(100 * If(isreleasebuild(), 100, 1),
                                      200 * If(isreleasebuild(), 100, 1)) - 1
            s.emplace(rnd_position())
        Next
        While (s.size() And 1) = 1
            s.emplace(rnd_position())
        End While

        Dim v As vector(Of Int64) = Nothing
        v = New vector(Of Int64)()
        Dim it As [set](Of Int64).iterator = Nothing
        it = s.begin()
        While it <> s.end()
            v.emplace_back(+it)
            it += 1
        End While

        assert(Not v.empty())
        Dim v2 As vector(Of pair(Of Int64, pair(Of Int64, Int64))) = Nothing
        v2 = New vector(Of pair(Of Int64, pair(Of Int64, Int64)))()
        For i As UInt32 = 0 To v.size() - uint32_1 Step 2
            v2.emplace_back(pair.emplace_of(rnd_int64(), pair.emplace_of(v(i), v(i + uint32_1))))
        Next
        Return v2
    End Function

    Public Overrides Function run() As Boolean
        Dim v As vector(Of pair(Of Int64, pair(Of Int64, Int64))) = Nothing
        v = prepare_segments()
        assert(Not v.empty())
        Dim t As segment_tree(Of Int64) = Nothing
        t = New segment_tree(Of Int64)(v.front().second.first, v.back().second.second)
        For i As UInt32 = 0 To v.size() - uint32_1
            If Not verify OrElse
               (v(i).second.second - v(i).second.first <= 1) OrElse
               rnd_bool() Then
                assertion.is_true(t.emplace(v(i).second.first, v(i).second.second, v(i).first))
            Else
                Dim tmp As Int64 = 0
                tmp = rnd_int64(v(i).second.first, v(i).second.second)
                assertion.is_true(t.emplace(v(i).second.first,
                                      If(rnd_bool(), tmp, rnd_int64(tmp, v(i).second.second)),
                                      v(i).first))
                assertion.is_true(t.emplace(If(rnd_bool(), tmp + 1, rnd_int64(v(i).second.first + 1, tmp + 1)),
                                      v(i).second.second,
                                      v(i).first))
            End If
            If verify Then
                For j As Int64 = v(i).second.first To v(i).second.second
                    Dim it As segment_tree(Of Int64).iterator = Nothing
                    it = t.find(j)
                    If assertion.is_not_null(it) Then
                        assertion.is_false(it.is_end())
                        assertion.is_true((+it).has_value())
                        assertion.equal((+it).value(), v(i).first)
                    End If
                Next
            End If
        Next
        If verify Then
            Dim vi As UInt32 = 0
            vi = 0
            For i As Int64 = v.front().second.first To v.back().second.second
                assert(i >= v(vi).second.first)
                Dim it As segment_tree(Of Int64).iterator = Nothing
                it = t.find(i)
                If assertion.is_not_null(it) Then
                    assertion.is_false(it.is_end())
                    If i <= v(vi).second.second Then
                        assertion.is_true((+it).has_value())
                        assertion.equal((+it).value(), v(vi).first)
                    Else
                        assert(vi < v.size() - 1)
                        assert(i <= v(vi + uint32_1).second.first)
                        If i < v(vi + uint32_1).second.first Then
                            assertion.is_false((+it).has_value())
                        Else
                            vi += uint32_1
                            i -= 1
                        End If
                    End If
                End If
            Next
            assert(v.front().second.first >= min_int64 + 100)
            For i As Int64 = v.front().second.first - 100 To v.front().second.first - 1
                Dim it As segment_tree(Of Int64).iterator = Nothing
                it = t.find(i)
                assertion.is_true(it.is_end() OrElse Not (+it).has_value())
            Next
            assert(v.back().second.second <= max_int64 - 100)
            For i As Int64 = v.back().second.second + 1 To v.back().second.second + 100
                Dim it As segment_tree(Of Int64).iterator = Nothing
                it = t.find(i)
                assertion.is_true(it.is_end() OrElse Not (+it).has_value())
            Next
        End If
        Return True
    End Function
End Class

Public NotInheritable Class segment_tree_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New segment_tree_case(True), 32 * If(isreleasebuild(), 2, 1))
    End Sub
End Class

Public NotInheritable Class segment_tree_perf_test
    Inherits performance_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New segment_tree_case(False), 32 * If(isreleasebuild(), 2, 1)))
    End Sub
End Class