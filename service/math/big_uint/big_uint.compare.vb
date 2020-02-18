
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class big_uint
    Public Function less(ByVal that As big_uint) As Boolean
        Return compare(that) < 0
    End Function

    Public Function equal(ByVal that As big_uint) As Boolean
        Return compare(that) = 0
    End Function

    Public Function less_or_equal(ByVal that As big_uint) As Boolean
        Return compare(that) <= 0
    End Function

    Public Function compare(ByVal that As big_uint) As Int32
        Return compare(Me, that)
    End Function

    Public Shared Function compare(ByVal this As big_uint, ByVal that As big_uint) As Int32
        Dim c As Int32 = 0
        c = object_compare(this, that)
        If c <> object_compare_undetermined Then
            Return c
        End If
        assert(Not this Is Nothing)
        assert(Not that Is Nothing)
        If (this.is_zero() AndAlso that.is_zero()) OrElse
           (this.is_one() AndAlso that.is_one()) Then
            Return 0
        End If
        If this.v.size() <> that.v.size() Then
            Return If(this.v.size() > that.v.size(), 1, -1)
        End If
        assert(this.v.size() > 0)
        Dim i As UInt32 = 0
        i = this.v.size() - uint32_1
        While True
            If this.v(i) <> that.v(i) Then
                Return If(this.v(i) > that.v(i), 1, -1)
            End If
            If i = 0 Then
                Exit While
            End If
            i -= uint32_1
        End While
        Return 0
    End Function
End Class
