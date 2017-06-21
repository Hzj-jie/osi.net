
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
End Class
