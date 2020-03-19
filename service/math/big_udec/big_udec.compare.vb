
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

        Dim v1 As big_uint = Nothing
        Dim r1 As big_uint = Nothing
        Dim v2 As big_uint = Nothing
        Dim r2 As big_uint = Nothing
        v1 = this.d.CloneT()
        v1.assert_divide(this.n, r1)
        v2 = that.d.CloneT()
        v2.assert_divide(that.n, r2)
        If v1.less(v2) Then
            Return 1
        End If
        If v2.less(v1) Then
            Return -1
        End If
        If r1.less(r2) Then
            Return 1
        End If
        If r2.less(r1) Then
            Return -1
        End If
        Return 0
    End Function
End Class
