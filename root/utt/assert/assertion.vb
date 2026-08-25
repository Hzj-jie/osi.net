
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock
Imports osi.root.template

Public NotInheritable Class assertion
    Inherits check(Of is_true_func)

    Private Shared failure As Int64 = 0

    Public NotInheritable Class is_true_func
        Inherits __void(Of Boolean, Object())

        Public Overrides Sub at(ByRef v As Boolean, ByRef msg() As Object)
            If v Then
                Return
            End If
            utt_raise_error("assertion failure, ",
                            msg,
                            " @ ",
                            backtrace(Of assertion, check(Of is_true_func))(),
                            ", stacktrace ",
                            callstack())
            If Not envs.utt_no_assert Then
                Interlocked.Increment(failure)
                assert(atomic.read(failure) < 1000, "too many assertion failures")
            End If
        End Sub
    End Class

    Public Shared Function failure_count() As Int64
        Return atomic.read(failure)
    End Function

    Public Shared Sub clear_failure()
        atomic.eva(failure, 0)
    End Sub

    Public Shared Sub disable(Optional ByVal msg As String = "test is disabled")
        Throw New utt_test_disabled(msg)
    End Sub

    Public Shared Sub disable_on_nix(Optional ByVal msg As String = "disabled on *nix")
        If os.is_nix Then
            disable(msg)
        End If
    End Sub

    Public Shared Sub disable_not_on_nix(Optional ByVal msg As String = "disabled not on *nix")
        If Not os.is_nix Then
            disable(msg)
        End If
    End Sub

    Public Shared Sub disable_on_windows(Optional ByVal msg As String = "disabled on Windows")
        If os.is_windows Then
            disable(msg)
        End If
    End Sub

    Public Shared Sub disable_not_on_windows(Optional ByVal msg As String = "disabled not on Windows")
        If Not os.is_windows Then
            disable(msg)
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
