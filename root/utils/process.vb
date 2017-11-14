
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector

Public Module _process
    <Extension()> Public Function quit(ByVal p As Process,
                                       Optional ByVal wait_ms As Int64 = default_close_process_wait_milliseconds) _
                                      As Boolean
        If p Is Nothing Then
            Return False
        Else
            Try
                If p.HasExited() Then
                    Return True
                End If
            Catch ex As Exception
                raise_error(error_type.warning,
                            "failed to retrieve process information of ",
                            p.StartInfo().FileName(),
                            ", ex ",
                            ex.Message())
                Return False
            End Try

            If wait_ms > max_int32 Then
                wait_ms = max_int32
            ElseIf wait_ms < min_int32 Then
                wait_ms = min_int32
            End If

            Try
                If Not p.CloseMainWindow() OrElse
                   Not p.WaitForExit(CInt(wait_ms)) Then
                    p.Kill()
                End If
            Catch ex As Exception
                raise_error(error_type.warning,
                            "failed to send quit commands to process of ",
                            p.StartInfo().FileName(),
                            ", ex ",
                            ex.Message())
                Return False
            End Try
            Return True
        End If
    End Function

    <Extension()> Public Function queue_quit(ByVal p As Process,
                                             Optional ByVal wait_ms As Int64 = default_close_process_wait_milliseconds,
                                             Optional ByVal quit_result As pointer(Of Boolean) = Nothing) As Boolean
        If p Is Nothing Then
            Return False
        Else
            queue_in_managed_threadpool(Sub()
                                            eva(quit_result, quit(p, wait_ms))
                                        End Sub)
            Return True
        End If
    End Function
End Module
