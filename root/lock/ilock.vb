
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.lock.slimlock

Public Interface ilock
    Inherits islimlock
    Function held_in_thread() As Boolean
    Function held() As Boolean
End Interface

Public NotInheritable Class lock_tracer
    Public Shared Function wait_too_long(ByVal n As Int64) As Boolean
        Return Now().milliseconds() - n > max(timeslice_length_ms \ 2, 1)
    End Function

    Private Sub New()
    End Sub
End Class
