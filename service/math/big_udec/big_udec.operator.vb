
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class big_udec
    Public Shared Operator +(ByVal this As big_udec, ByVal that As big_udec) As big_udec
        Return this.CloneT().add(that)
    End Operator

    Public Shared Operator -(ByVal this As big_udec, ByVal that As big_udec) As big_udec
        Return this.CloneT().sub(that)
    End Operator

    Public Shared Operator *(ByVal this As big_udec, ByVal that As big_udec) As big_udec
        Return this.CloneT().multiply(that)
    End Operator

    Public Shared Operator /(ByVal this As big_udec, ByVal that As big_udec) As big_udec
        Return this.CloneT().divide(that)
    End Operator

    Public Shared Operator >(ByVal this As big_udec, ByVal that As big_udec) As Boolean
        Return compare(this, that) > 0
    End Operator

    Public Shared Operator <(ByVal this As big_udec, ByVal that As big_udec) As Boolean
        Return compare(this, that) < 0
    End Operator

    Public Shared Operator =(ByVal this As big_udec, ByVal that As big_udec) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator <=(ByVal this As big_udec, ByVal that As big_udec) As Boolean
        Return compare(this, that) <= 0
    End Operator

    Public Shared Operator >=(ByVal this As big_udec, ByVal that As big_udec) As Boolean
        Return compare(this, that) >= 0
    End Operator

    Public Shared Operator <>(ByVal this As big_udec, ByVal that As big_udec) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Shared Operator ^(ByVal this As big_udec, ByVal that As big_udec) As big_udec
        Return this.CloneT().power(that)
    End Operator
End Class
