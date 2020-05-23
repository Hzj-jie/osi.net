
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.template

Public Class hash_based(Of KEY_T As IComparable(Of KEY_T), _HASH_SIZE As _int64, CT)
    Private Shared ReadOnly hs As Int32

    Shared Sub New()
        hs = assert_which.of(+(alloc(Of _HASH_SIZE)())).can_cast_to_int32()
        assert(hs > 1)
    End Sub

    Protected Shared Function hash_size() As Int32
        Return hs
    End Function

    Private ReadOnly cc() As CT

    Protected Sub New(ByVal d As Func(Of CT))
        assert(Not d Is Nothing)
        ReDim cc(hs - 1)
        For i As Int32 = 0 To hs - 1
            cc(i) = d()
        Next
    End Sub

    Protected Function [select](ByVal k As KEY_T) As CT
        Return [select](signing(k) Mod CUInt(hs))
    End Function

    Protected Function [select](ByVal i As Int32) As CT
        Return cc(i)
    End Function

    Protected Function [select](ByVal i As UInt32) As CT
        Return [select](CInt(i))
    End Function
End Class
