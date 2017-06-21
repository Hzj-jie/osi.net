
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Public Class unordered_set_case(Of T)
    Inherits random_run_case

    Private ReadOnly h As unordered_set(Of T)
    Private ReadOnly s As [set](Of T)

    Public Sub New(ByVal percentags() As Double)
        h = New unordered_set(Of T)()
        s = New [set](Of T)()
        insert_call(percentags(0), AddressOf insert)
        insert_call(percentags(1), AddressOf emplace)
        insert_call(percentags(2), AddressOf find)
        insert_call(percentags(3), AddressOf [erase])
        insert_call(percentags(4), AddressOf clone)
        insert_call(percentags(5), AddressOf clear)
    End Sub

    Private Function random_select_key_from_h() As T
        assert_false(h.empty())
        Dim it As unordered_set(Of T).iterator = Nothing
        it = h.begin()
        Dim c As UInt32 = 0
        c = rnd_uint(uint32_0, h.size())
        While c > uint32_0
            it += 1
            c -= uint32_1
        End While
        assert_not_equal(it, h.end())
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
            k = random_select_key_from_h()
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
                k = random_select_key_from_h()
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
        Dim h2 As unordered_set(Of T) = Nothing
        copy(h2, h)

        assert_equal(h2.size(), s.size())
        Dim it As unordered_set(Of T).iterator = Nothing
        it = h2.begin()
        While it <> h2.end()
            assert_not_equal(s.find(+it), s.end())
            it += 1
        End While
    End Sub
End Class

Public Class unordered_set_test(Of T)
    Inherits repeat_case_wrapper

    Public Sub New(ByVal percentages() As Double)
        MyBase.New(New unordered_set_case(Of T)(percentages), 1024 * 16)
    End Sub

    Protected Shared Function low_item_count_percentages() As Double()
        ' ~229 items
        Return {0.2395, 0.2395, 0.26, 0.25, 0.01, 0.001}
    End Function

    Protected Shared Function high_item_count_percentages() As Double()
        ' ~25000 items
        Return {0.25, 0.25, 0.23999, 0.25, 0.01, 0.00001}
    End Function
End Class

Public Class unordered_set_uint_test
    Inherits unordered_set_test(Of UInt32)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub
End Class

Public Class unordered_set_string_test
    Inherits unordered_set_test(Of String)

    Public Sub New()
        MyBase.New(low_item_count_percentages())
    End Sub
End Class

Public Class unordered_set_more_items_uint_test
    Inherits unordered_set_test(Of UInt32)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub
End Class

Public Class unordered_set_more_items_string_test
    Inherits unordered_set_test(Of String)

    Public Sub New()
        MyBase.New(high_item_count_percentages())
    End Sub
End Class
