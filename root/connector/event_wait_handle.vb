
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.constants

Public Module _event_wait_handle
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

    <Extension()> Public Function wait(ByVal i As EventWaitHandle, ByVal ms As Int64) As Boolean
        assert(Not i Is Nothing)
        If ms < 0 Then
            Try
                assert(i.WaitOne())
                Return True
            Catch ex As ThreadAbortException
                Return False
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return False
            End Try
        Else
            Try
                Return i.WaitOne(ms)
            Catch ex As ThreadAbortException
                Return False
            Catch ex As Exception
                log_unhandled_exception(ex)
                Return False
            End Try
        End If
    End Function

    <Extension()> Public Function wait(ByVal i As EventWaitHandle) As Boolean
        Return wait(i, npos)
    End Function
End Module
