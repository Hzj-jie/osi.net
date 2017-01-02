
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.connector

Public NotInheritable Class shell_less_process
    Inherits disposer

    Private ReadOnly p As disposer(Of Process)
    Private proc_started As singleentry
    Private proc_exited As singleentry
    Public Event receive_output(ByVal s As String)
    Public Event receive_error(ByVal s As String)
    Public Event process_exit()
    Private ReadOnly enable_raise_event As Boolean

    Public Sub New(Optional ByVal enable_raise_event As Boolean = False,
                   Optional ByVal synchronize_invoke As binder(Of ISynchronizeInvoke) = Nothing)
        p = make_disposer(New Process(),
                          disposer:=Sub(p As Process)
                                        assert(Not p Is Nothing)
                                        If enable_raise_event Then
                                            p.CancelOutputRead()
                                            p.CancelErrorRead()
                                        End If
                                        p.quit()
                                        p.StandardOutput().Close()
                                        p.StandardOutput().Dispose()
                                        p.StandardError().Close()
                                        p.StandardError().Dispose()
                                        p.StandardInput().Close()
                                        p.StandardInput().Dispose()
                                        p.Dispose()
                                    End Sub)
        Me.enable_raise_event = enable_raise_event
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

    Private Sub output_received(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        received(e, True)
    End Sub

    Private Sub error_received(ByVal sender As Object, ByVal e As DataReceivedEventArgs)
        received(e, False)
    End Sub

    Private Sub process_exited()
        If proc_exited.mark_in_use() Then
            proc_started.release()
            RaiseEvent process_exit()
        End If
    End Sub

    Private Sub process_exited(ByVal sender As Object, ByVal e As EventArgs)
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
        Return proc().WaitForExit(If(wait_ms < 0, max_int32, wait_ms))
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
    End Sub

    Public Shared Operator +(ByVal this As shell_less_process) As Process
        If this Is Nothing Then
            Return Nothing
        Else
            Return this.p.get()
        End If
    End Operator
End Class
