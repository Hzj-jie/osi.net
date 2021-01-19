
Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.lock

Public Module _threadpool
    Public Sub evaluate_in_managed_threadpool(Of T)(ByVal d As Func(Of T), ByVal r As ref(Of T))
        assert(Not d Is Nothing)
        queue_in_managed_threadpool(Sub()
                                        Dim i As T = Nothing
                                        i = d()
                                        eva(r, i)
                                    End Sub)
    End Sub

    Public Sub evaluate_in_managed_threadpool(Of T, RT)(ByVal d As _do(Of T, RT),
                                                        ByVal a As T,
                                                        ByVal r As ref(Of RT))
        assert(Not d Is Nothing)
        queue_in_managed_threadpool(Sub()
                                        Dim i As RT = Nothing
                                        i = d(a)
                                        eva(r, i)
                                    End Sub)
    End Sub

    Public Sub evaluate_in_managed_threadpool(Of T, RT)(ByVal d As Func(Of T, RT),
                                                        ByVal a As T,
                                                        ByVal r As ref(Of RT))
        assert(Not d Is Nothing)
        queue_in_managed_threadpool(Sub()
                                        Dim i As RT = Nothing
                                        i = d(a)
                                        eva(r, i)
                                    End Sub)
    End Sub

    Public Function execute_in_managed_threadpool(ByVal d As Action, ByVal timeout_ms As Int64) As Boolean
        If d Is Nothing Then
            Return False
        ElseIf timeout_ms < 0 OrElse timeout_ms > max_int32 Then
            d()
            Return True
        Else
            Using are As AutoResetEvent = New AutoResetEvent(False)
                Dim t As Thread = Nothing
                queue_in_managed_threadpool(Sub()
                                                Try
                                                    t = current_thread()
                                                    d()
                                                    assert(are.Set())
                                                Catch ex As ThreadAbortException
                                                    If application_lifetime.running() Then
                                                        Thread.ResetAbort()
                                                    End If
                                                End Try
                                            End Sub)
                If are.WaitOne(timeout_ms) Then
                    Return True
                Else
                    timeslice_sleep_wait_when(Function() t Is Nothing)
                    assert(t.ManagedThreadId() <> current_thread_id())
                    t.Abort()
                    Return False
                End If
            End Using
        End If
    End Function

    Public Function execute_in_managed_threadpool(Of T)(ByVal d As Func(Of T),
                                                        ByRef r As T,
                                                        ByVal timeout_ms As Int64) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Dim o As T = Nothing
            If execute_in_managed_threadpool(Sub()
                                                 o = d()
                                             End Sub,
                                             timeout_ms) Then
                r = o
                Return True
            Else
                Return False
            End If
        End If
    End Function

    <Extension()> Public Function suspend_all_process_threads(ByVal tps As ProcessThreadCollection) As Boolean
        If tps.null_or_empty() Then
            Return False
        Else
            Dim r As Boolean = False
            r = True
            For i As Int32 = 0 To tps.Count() - 1
                If Not tps(i) Is Nothing Then
                    If Not tps(i).suspend_thread() Then
                        r = False
                    End If
                End If
            Next
            Return r
        End If
    End Function

    Public Function suspend_all_current_process_threads() As Boolean
        Dim tps As ProcessThreadCollection = Nothing
        tps = this_process.ref.Threads()
        If tps.null_or_empty() Then
            Return False
        Else
            For Each t As ProcessThread In tps
                If Not t Is Nothing AndAlso
                   t.Id() = current_process_thread_id() Then
                    tps.Remove(t)
                    t.Dispose()
                    Exit For
                End If
            Next
            Dim r As Boolean = False
            r = suspend_all_process_threads(tps)
            tps.dispose()
            Return r
        End If
    End Function

    <Extension()> Public Function resume_all_process_threads(ByVal tps As ProcessThreadCollection) As Boolean
        If tps.null_or_empty() Then
            Return False
        Else
            Dim r As Boolean = False
            r = True
            For i As Int32 = 0 To tps.Count() - 1
                If Not tps(i) Is Nothing Then
                    If Not tps(i).resume_thread() Then
                        r = False
                    End If
                End If
            Next
            Return r
        End If
    End Function

    Public Function resume_all_current_process_threads() As Boolean
        Dim tps As ProcessThreadCollection = Nothing
        tps = this_process.ref.Threads()
        Dim r As Boolean = False
        r = resume_all_process_threads(tps)
        tps.dispose()
        Return r
    End Function
End Module
