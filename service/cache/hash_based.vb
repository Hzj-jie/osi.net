
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.template

Public Class hash_based(Of KEY_T As IComparable(Of KEY_T), _HASH_SIZE As _int64, CT)
    Private Shared ReadOnly hs As Int64

    Shared Sub New()
        hs = +(alloc(Of _HASH_SIZE)())
        assert(hs > 1)
    End Sub

    Protected Shared Function hash_size() As Int64
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
        Return [select](signing(k) Mod hs)
    End Function

    Protected Function [select](ByVal i As UInt32) As CT
        Return cc(i)
    End Function
End Class
