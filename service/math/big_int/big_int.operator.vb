
Imports osi.root.formation

Partial Public Class big_int
    Public Shared Operator +(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.add(that)
    End Operator

    Public Shared Operator -(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.sub(that)
    End Operator

    Public Shared Operator *(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.multiply(that)
    End Operator

    Public Shared Operator \(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.divide(that)
    End Operator

    Public Shared Operator /(ByVal this As big_int, ByVal that As big_int) As pair(Of big_int, big_int)
        Dim q As big_int = Nothing
        Dim r As big_int = Nothing
        q = New big_int(this)
        q.divide(that, r)
        Return pair.emplace_of(q, r)
    End Operator

    Public Shared Operator Mod(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim q As big_int = Nothing
        Dim r As big_int = Nothing
        q = New big_int(this)
        q.divide(that, r)
        Return r
    End Operator

    Public Shared Operator >(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) > 0
    End Operator

    Public Shared Operator <(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) < 0
    End Operator

    Public Shared Operator =(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator <>(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Shared Operator <=(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) <= 0
    End Operator

    Public Shared Operator >=(ByVal this As big_int, ByVal that As big_int) As Boolean
        Return compare(this, that) >= 0
    End Operator

    Public Shared Operator >>(ByVal this As big_int, ByVal that As Int32) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.right_shift(that)
    End Operator

    Public Shared Operator <<(ByVal this As big_int, ByVal that As Int32) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.left_shift(that)
    End Operator

    Public Shared Operator ^(ByVal this As big_int, ByVal that As big_int) As big_int
        Dim r As big_int = Nothing
        r = New big_int(this)
        Return r.power(that)
    End Operator
End Class
