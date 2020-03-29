
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Numerics
Imports osi.root.connector

Partial Public NotInheritable Class big_int
    Public Function as_BigInteger() As BigInteger
        If is_zero() Then
            Return BigInteger.Zero()
        End If
        If positive() Then
            Return New BigInteger(d.as_bytes())
        End If
        assert(negative())
        Return -(New BigInteger(d.as_bytes()))
    End Function

    Public Shared Function from_BigInteger(ByVal bi As BigInteger) As big_int
        If bi.Sign() > 0 Then
            Return New big_int(bi.ToByteArray())
        End If
        If bi.Sign() = 0 Then
            Return zero()
        End If
        assert(bi.Sign() < 0)
        bi = -bi
        Dim r As big_int = Nothing
        r = New big_int(bi.ToByteArray())
        r.set_negative()
        Return r
    End Function
End Class
