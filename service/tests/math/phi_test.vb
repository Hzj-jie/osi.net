
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.math

Public Class phi_test
    Inherits [case]

    Private Shared Function stupid_phi(ByVal x As Int32) As Int32
        If x < 0 Then
            Return npos
        Else
            Dim r As Int32 = 0
            For i As Int32 = 1 To x
                If relatively_prime(x, i) Then
                    r += 1
                End If
            Next
            Return r
        End If
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024
            assert_equal(stupid_phi(i), phi(i), i)
        Next
        For i As Int32 = 0 To 32 - 1
            Dim x As Int32 = 0
            x = rnd_int(0, max_int16)
            assert_equal(stupid_phi(x), phi(x), x)
        Next
        For i As Int32 = 0 To 2 - 1
            Dim x As Int32 = 0
            x = rnd_int(0, max_int32)
            assert_equal(stupid_phi(x), phi(x), x)
        Next
        For i As Int32 = 0 To 128 - 1
            Dim x As Int32 = 0
            x = rnd_int(min_int32, 0)
            assert_equal(stupid_phi(x), phi(x), x)
        Next
        Return True
    End Function
End Class
