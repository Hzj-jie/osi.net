
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_udec
    Public Function less(ByVal that As big_udec) As Boolean
        Return CompareTo(that) < 0
    End Function

    Public Function equal(ByVal that As big_udec) As Boolean
        Return CompareTo(that) = 0
    End Function

    Public Function less_or_equal(ByVal that As big_udec) As Boolean
        Return CompareTo(that) <= 0
    End Function

    Public Function compare(ByVal that As big_udec) As Int32
        Return compare(Me, that)
    End Function

    Public Shared Function compare(ByVal this As big_udec, ByVal that As big_udec) As Int32
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
            Return -1
        End If
        If that.is_zero() Then
            Return 1
        End If

        If this.d > that.d AndAlso this.n <= that.n Then
            Return -1
        End If
        If this.d < that.d AndAlso this.n >= that.n Then
            Return 1
        End If
        If this.d >= that.d AndAlso this.n < that.n Then
            Return -1
        End If
        If this.d <= that.d AndAlso this.n > that.n Then
            Return 1
        End If
        If this.d = that.d AndAlso this.n = that.n Then
            Return 0
        End If

        Return (this.n * that.d).compare(this.d * that.n)
    End Function
End Class
