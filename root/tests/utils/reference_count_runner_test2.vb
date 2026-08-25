
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class reference_count_runner_test2
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim are As AutoResetEvent = Nothing
        are = New AutoResetEvent(False)
        Dim r As reference_count_runner = Nothing
        r = New reference_count_runner(Sub(ByVal this As reference_count_runner)
                                           assert(are.force_set())
                                           this.mark_started()
                                           sleep(10)
                                       End Sub)
#If NET8_0_OR_GREATER Then
        ' Thread.Abort() is not supported on .NET 8+ / CoreCLR and unconditionally throws
        ' PlatformNotSupportedException. Terminate the background thread cooperatively.
        Dim stop_thread As Boolean = False
#End If
        Dim t As Thread = Nothing
        t = New Thread(Sub()
#If NET8_0_OR_GREATER Then
                           While Not stop_thread
                               If are.wait(50) Then
                                   r.mark_stopped()
                               End If
                           End While
#Else
                           While True
                               assert(are.wait())
                               r.mark_stopped()
                           End While
#End If
                       End Sub)
        t.Start()
        For i As Int32 = 0 To 1000
            assertion.is_true(r.bind())
            assertion.is_true(r.release())
        Next
#If NET8_0_OR_GREATER Then
        stop_thread = True
#Else
        t.Abort()
#End If
        t.Join()
        are.Close()
        Return True
    End Function
End Class
