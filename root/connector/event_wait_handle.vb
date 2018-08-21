
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.constants

Public Module _event_wait_handle
    <Extension()> Public Function disposed_or_set(ByVal i As EventWaitHandle) As Boolean
        Try
            assert(i.Set())
            Return True
        Catch ex As ObjectDisposedException
            Return True
        Catch ex As ThreadAbortException
            Return False
        Catch ex As Exception
            log_unhandled_exception(ex)
            Return False
        End Try
    End Function

    <Extension()> Public Function force_set(ByVal i As EventWaitHandle) As Boolean
        assert(Not i Is Nothing)
        Try
            assert(i.Set())
            Return True
        Catch ex As ThreadAbortException
            Return False
        Catch ex As Exception
            log_unhandled_exception(ex)
            Return False
        End Try
    End Function

    <Extension()> Public Function disposed_or_reset(ByVal i As EventWaitHandle) As Boolean
        Try
            assert(i.Reset())
            Return True
        Catch ex As ObjectDisposedException
            Return True
        Catch ex As ThreadAbortException
            Return False
        Catch ex As Exception
            log_unhandled_exception(ex)
            Return False
        End Try
    End Function

    <Extension()> Public Function force_reset(ByVal i As EventWaitHandle) As Boolean
        assert(Not i Is Nothing)
        Try
            assert(i.Reset())
            Return True
        Catch ex As ThreadAbortException
            Return False
        Catch ex As Exception
            log_unhandled_exception(ex)
            Return False
        End Try
    End Function

    <Extension()> Public Function wait(ByVal i As WaitHandle, ByVal ms As Int64) As Boolean
        assert(Not i Is Nothing)
        Try
            If ms < 0 OrElse ms > max_int32 Then
                Return assert(i.WaitOne())
            End If
            Return i.WaitOne(CInt(ms))
        Catch ex As ThreadAbortException
            Return False
        Catch ex As Exception
            log_unhandled_exception(ex)
            Return False
        End Try
    End Function

    <Extension()> Public Function wait(ByVal i As WaitHandle) As Boolean
        Return wait(i, npos)
    End Function

    <Extension()> Public Sub wait_close(ByVal i As WaitHandle)
        assert(Not i Is Nothing)
        assert(wait(i))
        i.Close()
    End Sub
End Module
