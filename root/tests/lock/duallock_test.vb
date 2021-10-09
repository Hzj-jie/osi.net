
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock
Imports osi.root.utt

Public NotInheritable Class duallock_test
    Inherits multithreading_case_wrapper

    Private Shared Shadows Function threadcount() As Int32
        Return (4 << (If(isreleasebuild(), 1, 0)))
    End Function

    Private Shared Function round() As Int64
        Return (128 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(repeat(New duallock_case(), round()), threadcount())
    End Sub

    Private NotInheritable Class duallock_case
        Inherits random_run_case

        Private l As duallock
        Private test_string As String = Nothing

        Public Sub New()
            MyBase.New()
            insert_call(0.5, AddressOf read)
            insert_call(0.5, AddressOf write)
        End Sub

        Private Shared Sub _sleep()
            sleep(rnd_int(0, CInt(2 * timeslice_length_ms)))
        End Sub

        Private Function write() As Boolean
            If rnd(0, 1) < 0.2 Then
                l.writer_wait()
                _sleep()
                test_string = guid_str()
                _sleep()
                l.writer_release()
            Else
                _sleep()
                _sleep()
            End If
            Return True
        End Function

        Private Function read() As Boolean
            l.reader_wait()
            Dim s As String = Nothing
            copy(s, test_string)
            _sleep()
            assertion.equal(s, test_string)
            _sleep()
            assertion.equal(s, test_string)
            l.reader_release()
            Return True
        End Function
    End Class
End Class
