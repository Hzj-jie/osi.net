
Option Explicit On
Option Infer Off
Option Strict On

#Const USE_DIV_REM = False

Imports System.Runtime.CompilerServices
Imports osi.root.connector
#If USE_DIV_REM Then
Imports osi.root.constants
#End If

Public Module _div_rem
    ' max_uint64 / max_uint32 = max_uint32 + 2, so the result needs to be UInt64.
    ' https://stackoverflow.com/questions/447282/why-is-math-divrem-so-inefficient
    <Extension()> Public Function div_rem(ByVal this As UInt64,
                                          ByVal that As UInt32,
                                          ByRef remainder As UInt32) As UInt64
#If USE_DIV_REM Then
        If this <= max_int32 AndAlso that <= max_int32 Then
            Dim iresult As Int32 = 0
            Dim iremainder As Int32 = 0
            iresult = System.Math.DivRem(CInt(this), CInt(that), iremainder)
            remainder = assert_which.of(iremainder).can_cast_to_uint32()
            Return assert_which.of(iresult).can_cast_to_uint32()
        End If
        If this <= max_int64 Then
            Dim iresult As Int64 = 0
            Dim iremainder As Int64 = 0
            iresult = System.Math.DivRem(CLng(this), CLng(that), iremainder)
            remainder = assert_which.of(iremainder).can_cast_to_uint32()
            Return assert_which.of(iresult).can_cast_to_uint64()
        End If
#End If

        ' * and - pair is more efficient than \.
        Dim r As UInt64 = 0
        r = this \ that
        remainder = assert_which.of(this - that * r).can_cast_to_uint32()
        Return r
    End Function
End Module
