
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class cast_test
    Inherits [case]

    Public Shared Function failed_case(Of T, T2)(ByVal i As T) As Boolean
        Dim j As T2 = Nothing
        assertion.is_false(cast(Of T2)(i, j))
        Return True
    End Function

    Private Shared Function caster_case() As Boolean
        Dim i As String = Nothing
        Dim j As StringBuilder = Nothing

        assertion.is_true(cast(i, j))
        assertion.is_null(j)
        assertion.is_true(cast(j, i))
        assertion.is_null(i)

        i = "ABC"
        assertion.is_true(cast(i, j))
        assertion.is_not_null(j)
        assertion.equal(Convert.ToString(j), i)

        j = New StringBuilder("DEF")
        assertion.is_true(cast(j, i))
        assertion.is_not_null(i)
        assertion.equal(i, Convert.ToString(j))

        Return True
    End Function

    Private Shared Function implicit_case() As Boolean
        Dim a() As Byte = Nothing
        a = rnd_bytes(rnd_uint(10, 20))
        Dim r As array_pointer(Of Byte) = Nothing
        assertion.is_true(cast(a, r))
        assertion.array_equal(+r, a)

        Dim r2 As pointer(Of Byte) = Nothing
        assertion.is_true(cast(a(0), r2))
        assertion.equal(+r2, a(0))

        Return True
    End Function

    Private Shared Function from_objects() As Boolean
        assertion.equal(cast(Of String)("abc"), "abc")
        assertion.equal(cast(Of Int32)(100), 100)
        assertion.equal(cast(Of Double)(100.0), 100)
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
