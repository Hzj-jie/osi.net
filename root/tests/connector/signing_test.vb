
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.constants

Public Class signing_perf
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim g As Guid = Nothing
        g = Guid.NewGuid()
        For i As Int32 = 1024 * 1024 * 1024 - 1 To 0 Step -1
            signing(g, i)
        Next
        Return True
    End Function
End Class

Public Class signing_test
    Inherits [case]

    Private Shared Function strong_consistant() As Boolean
        Dim g As Guid = Nothing
        g = Guid.NewGuid()
        Dim r As Int32 = 0
        For i As Int32 = 0 To 15
            r = rnd_int(min_int32, max_int32)
            assertion.equal(signing(g, r), signing(g, r))
            assertion.not_equal(signing(g, r), uint32_0)
        Next
        Return True
    End Function

    Private Shared Function unique() As Boolean
        Dim b As UInt32 = 0
        b = signing(guid_str())
        For i As Int32 = 0 To 1
            If b <> signing(guid_str()) Then
                Return True
            End If
        Next
        assertion.is_true(False)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 64 - 1
            If Not strong_consistant() OrElse
               Not unique() Then
                Return False
            End If
        Next
        Return True
    End Function
End Class
