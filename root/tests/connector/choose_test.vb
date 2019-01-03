
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class choose_test
    Inherits [case]

    Private Shared Function init_array() As Int32()
        Dim v() As Int32 = Nothing
        ReDim v(rnd_int(5, 30) - 1)
        For i As UInt32 = 0 To CUInt(array_size(v) - 1)
            v(i) = i
        Next
        Return v
    End Function

    Private Shared Function success_case() As Boolean
        Dim v() As Int32 = Nothing
        v = init_array()
        Dim s As [set](Of String) = Nothing
        s = New [set](Of String)()
        Dim cl As Int32 = 0
        cl = rnd_int(0, 5 + 1)
        assertion.is_true(choose(Sub(a() As Int32)
                               assertion.equal(CUInt(cl), array_size(a))
                               If cl > 1 Then
                                   For i As UInt32 = 0 To CUInt(array_size(a) - 2)
                                       assertion.less(a(i), a(i + 1))
                                   Next
                               End If
                               Dim t As String = Nothing
                               t = strcat(a)
                               assertion.is_true(s.find(t) = s.end())
                               s.insert(t)
                           End Sub,
                           v,
                           cl))
        assertion.equal(CInt(s.size()), choose(cl, array_size(v)))
        Return True
    End Function

    Private Shared Function failure_case() As Boolean
        Dim v() As Int32 = Nothing
        v = init_array()
        assertion.is_false(choose(Sub(a() As Int32)
                            End Sub,
                            v,
                            array_size(v) + 1))
        assertion.is_false(choose(Sub(a() As Int32)
                            End Sub,
                            v,
                            -1))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 - 1
            If Not success_case() Then
                Return False
            End If
        Next
        Return failure_case()
    End Function
End Class
