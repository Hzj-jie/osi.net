﻿
#Const USE_EXIT_SIGNAL = False

Option Explicit On
Option Infer Off
Option Strict On

Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
#If USE_EXIT_SIGNAL Then
Imports System.Threading
#End If
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.formation
Imports osi.root.lock

Public NotInheritable Class shell_less_process
    Inherits disposer

    Private ReadOnly p As disposer(Of Process)
    Private proc_started As singleentry
    Private proc_exited As singleentry
    Public Event receive_output(ByVal s As String)
    Public Event receive_error(ByVal s As String)
    Public Event process_exit()
    Private ReadOnly enable_raise_event As Boolean
    ' This does not resolve the issue of unreliable output event, but only to reduce the possibility.
    Private ReadOnly ce As count_event
#If USE_EXIT_SIGNAL Then
    Private ReadOnly exit_signal As ManualResetEvent
#End If

    Public Sub New(Optional ByVal enable_raise_event As Boolean = False,
                   Optional ByVal synchronize_invoke As binder(Of ISynchronizeInvoke) = Nothing)
        p = make_disposer(New Process(),
                          disposer:=Sub(p As Process)
                                        trace("disposer")
                                        assert(Not p Is Nothing)
                                        p.quit()
                                        If Not enable_raise_event Then
                                            p.StandardOutput().Close()
                                            p.StandardOutput().Dispose()
                                            p.StandardError().Close()
                                            p.StandardError().Dispose()
                                        End If
                                        p.StandardInput().Close()
                                        p.StandardInput().Dispose()
                                        p.Dispose()
                                        trace("finish disposer")
                                    End Sub)
        Me.enable_raise_event = enable_raise_event
#If USE_EXIT_SIGNAL Then
        Me.exit_signal = New ManualResetEvent(False)
#End If
        Me.ce = New count_event()
        proc().EnableRaisingEvents() = True
        proc().SynchronizingObject() = +synchronize_invoke
        AddHandler proc().OutputDataReceived, AddressOf output_received
        AddHandler proc().ErrorDataReceived, AddressOf error_received
        AddHandler proc().Exited, AddressOf process_exited
    End Sub

    Private Sub received(ByVal e As DataReceivedEventArgs, ByVal output As Boolean)
        If Not e Is Nothing AndAlso Not e.Data() Is Nothing Then
            If output Then
                RaiseEvent receive_output(e.Data())
            Else
                RaiseEvent receive_error(e.Data())
            End If
        End If
    End Sub

    Private Sub received_delegate(ByVal e As DataReceivedEventArgs, ByVal output As Boolean)
        ce.increment()
        trace("received_delegate")
        If proc().SynchronizingObject() Is Nothing Then
            received(e, output)
        Else
            ' This is definitely not the correct behavior, but Process.Exited() may be raised on a random ThreadPool
            ' thread.
            ' TODO: Why Process.Exited() won't respect SynchronizingObject().
            proc().SynchronizingObject().Invoke(Sub()
                                                    received(e, output)
                                                End Sub,
                                                Nothing)
        End If
        trace("finish received_delegate")
        ce.decrement()
    End Sub

    Private Sub output_received(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        received_delegate(e, True)
    End Sub

    Private Sub error_received(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        received_delegate(e, False)
    End Sub

    Private Sub process_exited()
        If proc_exited.mark_in_use() Then
            assert(ce.wait())
            proc_started.release()
            RaiseEvent process_exit()
            If enable_raise_event Then
                proc().CancelOutputRead()
                proc().CancelErrorRead()
            End If
#If USE_EXIT_SIGNAL Then
            exit_signal.force_set()
#End If
        End If
    End Sub

    Private Sub process_exited(ByVal sender As Object, ByVal e As EventArgs)
        trace("start process_exited")
        If proc().SynchronizingObject() Is Nothing Then
            process_exited()
        Else
            ' This is definitely not the correct behavior, but Process.Exited() may be raised on a random ThreadPool
            ' thread.
            ' TODO: Why Process.Exited() won't respect SynchronizingObject().
            proc().SynchronizingObject().Invoke(Sub()
                                                    process_exited()
                                                End Sub,
                                                Nothing)
        End If
        trace("finish process_exited")
    End Sub

    Public Function exited() As Boolean
        Return proc_exited.in_use()
    End Function

    Public Function exit_code() As Int32
        Return proc().ExitCode()
    End Function

    Public Function exit_time() As DateTime
        Return proc().ExitTime()
    End Function

    Private Function proc() As Process
        Return p.[get]()
    End Function

    Public Function start_info() As ProcessStartInfo
        Return proc().StartInfo()
    End Function

    Public Function start(Optional ByRef ex As Exception = Nothing) As Boolean
        If proc_started.mark_in_use() Then
            ce.increment()
            Using defer(Sub() ce.decrement())
                If proc_exited.in_use() Then
                    proc_exited.release()
                End If
                start_info().UseShellExecute() = False
                start_info().RedirectStandardOutput() = True
                start_info().RedirectStandardError() = True
                start_info().RedirectStandardInput() = True
                Try
                    If Not proc().Start() Then
                        Return False
                    End If
                Catch e As Exception
                    ex = e
                    Return False
                End Try
                If enable_raise_event Then
                    proc().BeginOutputReadLine()
                    proc().BeginErrorReadLine()
                End If
            End Using
            Return True
        Else
            Return False
        End If
    End Function

    Public Function quit(ByVal wait_ms As Int64) As Boolean
        Return proc().quit(wait_ms)
    End Function

    Public Function quit() As Boolean
        Return proc().quit()
    End Function

    Public Function queue_quit(ByVal wait_ms As Int64,
                               Optional ByVal quit_result As pointer(Of Boolean) = Nothing) As Boolean
        Return proc().queue_quit(wait_ms, quit_result)
    End Function

    Public Function queue_quit(Optional ByVal quit_result As pointer(Of Boolean) = Nothing) As Boolean
        Return proc().queue_quit(quit_result:=quit_result)
    End Function

    Public Function wait_for_exit(ByVal wait_ms As Int32) As Boolean
#If USE_EXIT_SIGNAL Then
        Return exit_signal.wait(wait_ms)
#Else
        Return proc().WaitForExit(If(wait_ms < 0, max_int32, wait_ms))
#End If
    End Function

    Public Function wait_for_exit(ByVal wait_ms As Int64) As Boolean
        Return wait_for_exit(If(wait_ms >= max_int32, max_int32, CInt(wait_ms)))
    End Function

    Public Sub wait_for_exit()
        proc().WaitForExit()
    End Sub

    Public Function stdout() As StreamReader
        assert(Not enable_raise_event)
        Return proc().StandardOutput()
    End Function

    Public Function stderr() As StreamReader
        assert(Not enable_raise_event)
        Return proc().StandardError()
    End Function

    Public Function stdin() As StreamWriter
        Return proc().StandardInput()
    End Function

    Public Function stdout_str() As String
        Return stdout().ReadToEnd()
    End Function

    Public Function stderr_str() As String
        Return stderr().ReadToEnd()
    End Function

    Protected Overrides Sub disposer()
        p.dispose()
#If USE_EXIT_SIGNAL Then
        exit_signal.Close()
        exit_signal.Dispose()
#End If
    End Sub

    Public Shared Operator +(ByVal this As shell_less_process) As Process
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.p.get()
        End If
    End Operator
End Class
