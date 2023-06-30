
#If 0 Then
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.lock
Imports osi.root.envs
Imports System.Threading
Imports osi.root.utils
Imports osi.root.delegates

Public Class auto_duallock_test
    Inherits multithreading_case_wrapper

    Private Shared Shadows Function threadcount() As Int32
        Return (8 << (If(isreleasebuild(), 2, 0)))
    End Function

    Private Shared Function round() As Int64
        Return (1024 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(repeat(New auto_duallock_case(), round()), threadcount())
    End Sub

    Private Class auto_duallock_case
        Inherits random_run_case

        Private l As duallock
        Private test_string As String = Nothing

        Public Sub New()
            MyBase.New()
            insert_call(0.5, AddressOf read)
            insert_call(0.5, AddressOf write)
        End Sub

        Private Shared Sub _sleep()
            sleep(rnd_int(0, two_timeslice_length_ms))
        End Sub

        Private Function write() As Boolean
            If rnd(0, 1) < 0.2 Then
                Using New auto_dual_write_lock(l)
                    _sleep()
                    test_string = guid_str()
                    _sleep()
                End Using
            Else
                _sleep()
                _sleep()
            End If
            Return True
        End Function

        Private Function read() As Boolean
            Using New auto_dual_read_lock(l)
                Dim s As String = Nothing
                copy(s, test_string)
                _sleep()
                assertion.equal(s, test_string)
                _sleep()
                assertion.equal(s, test_string)
            End Using
            Return True
        End Function
    End Class
End Class
#End If
