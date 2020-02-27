
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class udec
    Public Shared Operator +(ByVal this As udec, ByVal that As udec) As udec
        Return this.CloneT().add(that)
    End Operator

    Public Shared Operator -(ByVal this As udec, ByVal that As udec) As udec
        Return this.CloneT().sub(that)
    End Operator

    Public Shared Operator *(ByVal this As udec, ByVal that As udec) As udec
        Return this.CloneT().multiply(that)
    End Operator

    Public Shared Operator /(ByVal this As udec, ByVal that As udec) As udec
        Return this.CloneT().divide(that)
    End Operator

    Public Shared Operator >(ByVal this As udec, ByVal that As udec) As Boolean
        Return compare(this, that) > 0
    End Operator

    Public Shared Operator <(ByVal this As udec, ByVal that As udec) As Boolean
        Return compare(this, that) < 0
    End Operator

    Public Shared Operator =(ByVal this As udec, ByVal that As udec) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator <=(ByVal this As udec, ByVal that As udec) As Boolean
        Return compare(this, that) <= 0
    End Operator

    Public Shared Operator >=(ByVal this As udec, ByVal that As udec) As Boolean
        Return compare(this, that) >= 0
    End Operator

    Public Shared Operator <>(ByVal this As udec, ByVal that As udec) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Shared Operator ^(ByVal this As udec, ByVal that As udec) As udec
        Return this.CloneT().power(that)
    End Operator
End Class
