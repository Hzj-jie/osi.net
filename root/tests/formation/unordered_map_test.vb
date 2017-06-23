﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Public Class unordered_map_case(Of KEY_T, VALUE_T)
    Inherits random_run_case

    Private ReadOnly u As unordered_map(Of KEY_T, VALUE_T)
    Private ReadOnly m As map(Of KEY_T, VALUE_T)

    Public Sub New()
        u = New unordered_map(Of KEY_T, VALUE_T)()
        m = New map(Of KEY_T, VALUE_T)()
        insert_call(0.25, AddressOf insert)
        insert_call(0.25, AddressOf emplace)
        insert_call(0.23999, AddressOf find)
        insert_call(0.25, AddressOf [erase])
        insert_call(0.01, AddressOf clone)
        insert_call(0.00001, AddressOf clear)
    End Sub

    Private Function random_select_key_from_u() As KEY_T
        assert_false(u.empty())
        Dim it As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
        it = u.begin()
        Dim c As UInt32 = 0
        c = rnd_uint(uint32_0, u.size())
        While c > uint32_0
            it += 1
            c -= uint32_1
        End While
        assert_not_equal(it, u.end())
        Return (+it).first
    End Function

    Private Sub insert_or_emplace(ByVal insert As Boolean)
        Dim n As KEY_T = Nothing
        n = rnd(Of KEY_T)()
        Dim v As VALUE_T = Nothing
        v = rnd(Of VALUE_T)()
        If If(insert, u.insert(n, v).second, u.emplace(n, v).second) Then
            assert_equal(m.find(n), m.end())
            assert_true(m.emplace(n, v).second)
        Else
            assert_not_equal(m.find(n), m.end())
            assert_false(m.emplace(n, v).second)
        End If
        assert_equal(u.size(), m.size())
        assert_equal(u.empty(), m.empty())
    End Sub

    Private Sub insert()
        insert_or_emplace(True)
    End Sub

    Private Sub emplace()
        insert_or_emplace(False)
    End Sub

    Private Sub [erase]()
        If rnd_bool() AndAlso Not m.empty() Then
            Dim k As KEY_T = Nothing
            k = random_select_key_from_u()
            assert_true(m.erase(k))
            assert_true(u.erase(k))
        Else
            Dim k As KEY_T = Nothing
            k = rnd(Of KEY_T)()
            assert_equal(m.erase(k), u.erase(k))
        End If
    End Sub

    Private Sub find()
        If rnd_bool() Then
            If m.empty() Then
                assert_true(u.empty())
                assert_equal(u.find(rnd(Of KEY_T)()), u.end())
            Else
                Dim k As KEY_T = Nothing
                k = random_select_key_from_u()
                assert_not_equal(m.find(k), m.end())
                assert_not_equal(u.find(k), u.end())
            End If
        Else
            Dim k As KEY_T = Nothing
            k = rnd(Of KEY_T)()
            If m.find(k) = m.end() Then
                assert_equal(u.find(k), u.end())
            Else
                assert_not_equal(u.find(k), u.end())
            End If
        End If
    End Sub

    Private Sub clear()
        m.clear()
        u.clear()
        assert_true(u.empty())
    End Sub

    Private Sub clone()
        Dim u2 As unordered_map(Of KEY_T, VALUE_T) = Nothing
        copy(u2, u)

        assert_equal(u2.size(), m.size())
        Dim it As unordered_map(Of KEY_T, VALUE_T).iterator = Nothing
        it = u2.begin()
        While it <> u2.end()
            assert_not_equal(m.find((+it).first), m.end())
            assert_equal(m((+it).first), (+it).second)
            it += 1
        End While
    End Sub
End Class

Public Class unordered_map_test(Of KEY_T, VALUE_T)
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New unordered_map_case(Of KEY_T, VALUE_T), 1024 * 16)
    End Sub
End Class

Public Class unordered_map_uint_test
    Inherits unordered_map_test(Of UInt32, UInt32)
End Class

Public Class unordered_map_string_test
    Inherits unordered_map_test(Of String, String)
End Class
