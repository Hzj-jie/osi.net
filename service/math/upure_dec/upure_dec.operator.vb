
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class upure_dec
    Public Shared Operator +(ByVal this As upure_dec, ByVal that As upure_dec) As upure_dec
        Return this.CloneT().add(that)
    End Operator

    Public Shared Operator -(ByVal this As upure_dec, ByVal that As upure_dec) As upure_dec
        Return this.CloneT().sub(that)
    End Operator

    Public Shared Operator *(ByVal this As upure_dec, ByVal that As upure_dec) As upure_dec
        Return this.CloneT().multiply(that)
    End Operator

    Public Shared Operator /(ByVal this As upure_dec, ByVal that As upure_dec) As upure_dec
        Return this.CloneT().divide(that)
    End Operator

    Public Shared Operator >(ByVal this As upure_dec, ByVal that As upure_dec) As Boolean
        Return compare(this, that) > 0
    End Operator

    Public Shared Operator <(ByVal this As upure_dec, ByVal that As upure_dec) As Boolean
        Return compare(this, that) < 0
    End Operator

    Public Shared Operator =(ByVal this As upure_dec, ByVal that As upure_dec) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator <=(ByVal this As upure_dec, ByVal that As upure_dec) As Boolean
        Return compare(this, that) <= 0
    End Operator

    Public Shared Operator >=(ByVal this As upure_dec, ByVal that As upure_dec) As Boolean
        Return compare(this, that) >= 0
    End Operator

    Public Shared Operator <>(ByVal this As upure_dec, ByVal that As upure_dec) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Shared Operator ^(ByVal this As upure_dec, ByVal that As upure_dec) As upure_dec
        Return this.CloneT().power(that)
    End Operator
End Class
