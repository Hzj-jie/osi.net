
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class big_int
    Public Shared Widening Operator CType(ByVal this As Int32) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As UInt32) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As Int64) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As UInt64) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As big_uint) As big_int
        Return New big_int(this)
    End Operator

    Public Shared Widening Operator CType(ByVal this As Boolean) As big_int
        Dim r As big_int = Nothing
        r = New big_int()
        r.replace_by(this)
        Return r
    End Operator

    Public Shared Widening Operator CType(ByVal this As big_int) As Boolean
        Return Not this Is Nothing AndAlso this.true()
    End Operator

    Public Shared Operator Not(ByVal this As big_int) As Boolean
        Return this Is Nothing OrElse this.false()
    End Operator

    Public Function as_uint64(ByRef overflow As Boolean) As UInt64
        If negative() Then
            overflow = True
            Return 0
        Else
            Return d.as_uint64(overflow)
        End If
    End Function

    Public Function as_int32(ByRef overflow As Boolean) As Int32
        Dim v As Int32 = 0
        v = d.as_int32(overflow)
        Return If(negative(), -v, v)
    End Function

    Public Function as_bytes(Optional ByRef neg As Boolean = Nothing) As Byte()
        neg = negative()
        Return d.as_bytes()
    End Function
End Class
