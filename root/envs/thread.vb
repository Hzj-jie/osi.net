
Imports System.Diagnostics
Imports System.Diagnostics.CodeAnalysis
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants

Public Module _thread
    Private Class _thread
        <ThreadStatic()> Private Shared _current_thread As Thread
        <ThreadStatic()> Private Shared _current_thread_id As Int32
        <ThreadStatic()> Private Shared _current_process_thread As ProcessThread
        <ThreadStatic()> Private Shared _current_process_thread_id As Int32

        Shared Sub New()
            assert(Nothing Is DirectCast(Nothing, Thread))
            assert(INVALID_THREAD_ID = DirectCast(Nothing, Int32))
        End Sub

        Public Shared Function current_thread() As Thread
            Dim rtn As Thread = Nothing
            rtn = _current_thread
            If rtn Is Nothing Then
                rtn = Thread.CurrentThread()
                _current_thread = rtn
            End If

            Return rtn
        End Function

        Public Shared Function current_thread_id() As Int32
            Dim rtn As Int32 = 0
            rtn = _current_thread_id
            If rtn = INVALID_THREAD_ID Then
                rtn = current_thread().ManagedThreadId()
                _current_thread_id = rtn
            End If

            Return rtn
        End Function

        Public Shared Function current_process_thread() As ProcessThread
            Dim r As ProcessThread = Nothing
            r = _current_process_thread
            If r Is Nothing Then
                Thread.BeginThreadAffinity()
                Dim tps As ProcessThreadCollection = Nothing
                tps = current_process.Threads()
                assert(Not tps.null_or_empty())
                For i As Int32 = 0 To tps.Count() - 1
                    assert(Not tps(i) Is Nothing)
                    If tps(i).Id() = current_process_thread_id() Then
                        r = tps(i)
                        Exit For
                    End If
                Next
                assert(Not r Is Nothing)
                _current_process_thread = r
                For i As Int32 = 0 To tps.Count() - 1
                    Dim c As Int32 = 0
                    c = object_compare(r, tps(i))
                    If c = object_compare_undetermined Then
                        tps(i).Dispose()
                    Else
                        assert(c = 0)
                    End If
                Next
            End If

            Return r
        End Function

        Public Shared Function current_process_thread_id() As Int32
            Dim r As Int32 = 0
            r = _current_process_thread_id
            If r = INVALID_THREAD_ID Then
                Thread.BeginThreadAffinity()
                r = AppDomain.GetCurrentThreadId()
                _current_process_thread_id = r
            End If

            Return r
        End Function
    End Class

    Public Function current_thread() As Thread
        Return _thread.current_thread()
    End Function

    Public Function current_thread_id() As Int32
        Return _thread.current_thread_id()
    End Function

    Public Function current_process_thread() As ProcessThread
        Return _thread.current_process_thread()
    End Function

    Public Function current_process_thread_id() As Int32
        Return _thread.current_process_thread_id()
    End Function
End Module
