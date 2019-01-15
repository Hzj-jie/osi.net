
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.template

Public NotInheritable Class expectation
    Inherits check(Of is_true_func)

    Private Shared ReadOnly backtrace_ignores() As String = {
        GetType(expectation).FullName(),
        GetType(check(Of is_true_func)).FullName()
    }

    Private Shared failure As Int64 = 0

    Public NotInheritable Class is_true_func
        Inherits __void(Of Boolean, Object())

        Public Overrides Sub at(ByRef v As Boolean, ByRef msg() As Object)
            If v Then
                Return
            End If
            utt_raise_error("unsatisfied expectation, ",
                            msg,
                            " @ ",
                            backtrace(backtrace_ignores),
                            ", stacktrace ",
                            callstack())
            Interlocked.Increment(failure)
        End Sub
    End Class

    Public Shared Function failure_count() As Int64
        Return atomic.read(failure)
    End Function

    Public Shared Sub clear_failure()
        atomic.eva(failure, 0)
    End Sub

    Private Sub New()
    End Sub
End Class
