
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public Class cast_test
    Inherits [case]

    Public Shared Function failed_case(Of T, T2)(ByVal i As T) As Boolean
        Dim j As T2 = Nothing
        assert_false(cast(Of T2)(i, j))
        Return True
    End Function

    Private Shared Function caster_case() As Boolean
        Dim i As String = Nothing
        Dim j As StringBuilder = Nothing

        assert_true(cast(i, j))
        assert_nothing(j)
        assert_true(cast(j, i))
        assert_nothing(i)

        i = "ABC"
        assert_true(cast(i, j))
        assert_not_nothing(j)
        assert_equal(Convert.ToString(j), i)

        j = New StringBuilder("DEF")
        assert_true(cast(j, i))
        assert_not_nothing(i)
        assert_equal(i, Convert.ToString(j))

        Return True
    End Function

    Private Shared Function implicit_case() As Boolean
        Dim a() As Byte = Nothing
        a = rnd_bytes(rnd_uint(10, 20))
        Dim r As array_pointer(Of Byte) = Nothing
        assert_true(cast(a, r))
        assert_array_equal(+r, a)

        Dim r2 As pointer(Of Byte) = Nothing
        assert_true(cast(a(0), r2))
        assert_equal(+r2, a(0))

        Return True
    End Function

    Private Shared Function from_objects() As Boolean
        assert_equal(cast(Of String)("abc"), "abc")
        assert_equal(cast(Of Int32)(100), 100)
        assert_equal(cast(Of Double)(100.0), 100)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Using scoped_atomic_bool(suppress.compare_error)
            Return value_types() AndAlso
                   reference_types() AndAlso
                   caster_case() AndAlso
                   implicit_case() AndAlso
                   from_objects()
        End Using
    End Function
End Class
