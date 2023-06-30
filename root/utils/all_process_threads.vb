
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation

Public NotInheritable Class all_process_threads
    Public Shared Function suspend(ByVal tps As ProcessThreadCollection) As Boolean
        If tps.null_or_empty() Then
            Return False
        End If
        Dim r As Boolean = True
        For i As Int32 = 0 To tps.Count() - 1
            If Not tps(i) Is Nothing Then
                If Not tps(i).suspend_thread() Then
                    r = False
                End If
            End If
        Next
        Return r
    End Function

    Public Shared Function suspend() As Boolean
        Dim tps As ProcessThreadCollection = this_process.ref.Threads()
        If tps.null_or_empty() Then
            Return False
        End If
        For Each t As ProcessThread In tps
            If Not t Is Nothing AndAlso t.Id() = current_process_thread_id() Then
                tps.Remove(t)
                t.Dispose()
                Exit For
            End If
        Next
        Dim r As Boolean = suspend(tps)
        tps.dispose()
        Return r
    End Function

    Public Shared Function [resume](ByVal tps As ProcessThreadCollection) As Boolean
        If tps.null_or_empty() Then
            Return False
        End If
        Dim r As Boolean = True
        For i As Int32 = 0 To tps.Count() - 1
            If Not tps(i) Is Nothing Then
                If Not tps(i).resume_thread() Then
                    r = False
                End If
            End If
        Next
        Return r
    End Function

    Public Shared Function [resume]() As Boolean
        Dim tps As ProcessThreadCollection = this_process.ref.Threads()
        Dim r As Boolean = [resume](tps)
        tps.dispose()
        Return r
    End Function
End Class
