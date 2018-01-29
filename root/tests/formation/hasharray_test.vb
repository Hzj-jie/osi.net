
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Public Class hasharray_case(Of T)
    Inherits random_run_case

    Private ReadOnly h As hasharray(Of T)
    Private ReadOnly s As [set](Of T)

    ' ~229 items
    Public Sub New()
        h = New hasharray(Of T)()
        s = New [set](Of T)()
        insert_call(0.2395, AddressOf insert)
        insert_call(0.2395, AddressOf emplace)
        insert_call(0.26, AddressOf find)
        insert_call(0.25, AddressOf [erase])
        insert_call(0.01, AddressOf clone)
        insert_call(0.0009, AddressOf clear)
        insert_call(0.0001, AddressOf traversal)
    End Sub

    Private Function random_select_key_from_s() As T
        assert_false(h.empty())
        Dim it As [set](Of T).iterator = Nothing
        it = s.begin()
        Dim c As UInt32 = 0
        c = rnd_uint(uint32_0, s.size())
        While c > uint32_0
            it += 1
            c -= uint32_1
        End While
        assert_not_equal(it, s.end())
        Return +it
    End Function

    Private Sub insert_or_emplace(ByVal insert As Boolean)
        Dim n As T = Nothing
        n = rnd(Of T)()
        If If(insert, h.insert(n).second, h.emplace(n).second) Then
            assert_equal(s.find(n), s.end())
            s.insert(n)
        Else
            assert_not_equal(s.find(n), s.end())
        End If
        assert_equal(h.size(), s.size())
        assert_equal(h.empty(), s.empty())
    End Sub

    Private Sub insert()
        insert_or_emplace(True)
    End Sub

    Private Sub emplace()
        insert_or_emplace(False)
    End Sub

    Private Sub [erase]()
        If rnd_bool() AndAlso Not s.empty() Then
            Dim k As T = Nothing
            k = random_select_key_from_s()
            assert_true(s.erase(k))
            assert_equal(h.erase(k), uint32_1)
        Else
            Dim k As T = Nothing
            k = rnd(Of T)()
            If s.erase(k) Then
                assert_equal(h.erase(k), uint32_1)
            Else
                assert_equal(h.erase(k), uint32_0)
            End If
        End If
    End Sub

    Private Sub find()
        If rnd_bool() Then
            If s.empty() Then
                assert_true(h.empty())
                assert_equal(h.find(rnd(Of T)()), h.end())
            Else
                Dim k As T = Nothing
                k = random_select_key_from_s()
                assert_not_equal(h.find(k), h.end())
                assert_not_equal(s.find(k), s.end())
            End If
        Else
            Dim k As T = Nothing
            k = rnd(Of T)()
            If s.find(k) = s.end() Then
                assert_equal(h.find(k), h.end())
            Else
                assert_not_equal(h.find(k), h.end())
            End If
        End If
    End Sub

    Private Sub clear()
        s.clear()
        h.clear()
        assert_true(h.empty())
    End Sub

    Private Sub clone()
        Dim h2 As hasharray(Of T) = Nothing
        copy(h2, h)

        assert_equal(h2.size(), s.size())
        Dim it As [set](Of T).iterator = Nothing
        it = s.begin()
        While it <> s.end()
            assert_not_equal(h.find(+it), h.end())
            assert_not_equal(h2.find(+it), h2.end())
            it += 1
        End While
    End Sub

    Private Sub traversal()
        Dim it As hasharray(Of T).iterator = Nothing
        it = h.begin()
        Dim c As UInt32 = 0
        While it <> h.end()
            c += uint32_1
            it += 1
        End While

        Dim c2 As UInt32 = 0
        it = h.rbegin()
        While it <> h.rend()
            c2 += uint32_1
            it -= 1
        End While

        assert_equal(c, h.size())
        assert_equal(c2, h.size())
    End Sub
End Class

Public Class hasharray_test(Of T)
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New hasharray_case(Of T)(), 1024 * 64))
    End Sub
End Class

Public Class hasharray_uint_test
    Inherits hasharray_test(Of UInt32)
End Class

Public Class hasharray_string_test
    Inherits hasharray_test(Of String)
End Class
