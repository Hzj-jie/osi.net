
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_ufloat
    Public Function less(ByVal that As big_ufloat) As Boolean
        Return CompareTo(that) < 0
    End Function

    Public Function equal(ByVal that As big_ufloat) As Boolean
        Return CompareTo(that) = 0
    End Function

    Public Function less_or_equal(ByVal that As big_ufloat) As Boolean
        Return CompareTo(that) <= 0
    End Function

    Public Shared Function compare(ByVal this As big_ufloat, ByVal that As big_ufloat) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c <> object_compare_undetermined Then
            Return c
        End If

        assert(Not this Is Nothing)
        assert(Not that Is Nothing)

        If this.is_zero() AndAlso that.is_zero() Then
            Return 0
        End If
        If this.is_zero() Then
            Return 1
        End If
        If that.is_zero() Then
            Return -1
        End If

        Dim int_part As Int32 = 0
        int_part = this.i.CompareTo(that.i)
        If int_part = 0 Then
            Return this.d.CompareTo(that.d)
        End If
        Return int_part
    End Function
End Class
