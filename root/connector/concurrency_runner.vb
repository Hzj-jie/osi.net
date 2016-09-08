
Imports System.Threading
Imports osi.root.constants

' An executor to concurrently execute processor-count tasks in managed threadpool. To maximize processor usage, one
' should not queue a sleep or wait or io operation in a concurrency_runner.
Public NotInheritable Class concurrency_runner
    Public Shared ReadOnly instance As concurrency_runner
    <ThreadStatic> Private Shared is_concurrency_runner_thread As Boolean

    Shared Sub New()
        instance = New concurrency_runner()
    End Sub

    Public Shared Function in_concurrency_runner_thread() As Boolean
        Return is_concurrency_runner_thread
    End Function

    Private Sub New()
    End Sub

    ' Queues actions into concurrency_runner, and block until all the jobs are finished.
    Public Shared Sub execute(ByVal ParamArray v() As Action)
        instance.e(v)
    End Sub

    Public Shared Sub execute(Of T)(ByVal v As Action(Of T), ByVal ParamArray c() As T)
        assert(Not v Is Nothing)
        If Not isemptyarray(c) Then
            Dim a() As Action = Nothing
            ReDim a(array_size(c) - uint32_1)
            For i As UInt32 = uint32_0 To array_size(c) - uint32_1
                Dim n As T = Nothing
                n = c(i)
                a(i) = Sub()
                           v(n)
                       End Sub
            Next
            execute(a)
        End If
    End Sub

    Private Sub e(ByVal v As Action)
        is_concurrency_runner_thread = True
        void_(v)
        is_concurrency_runner_thread = False
    End Sub

    Private Sub e(ByVal v() As Action)
        If Not v.null_or_empty() Then
            If array_size(v) = uint32_1 Then
                e(v(0))
            ElseIf Environment.ProcessorCount() = 1 Then
                For i As UInt32 = uint32_0 To array_size(v) - uint32_1
                    e(v(i))
                Next
            Else
                Using w As AutoResetEvent = New AutoResetEvent(False)
                    Dim r As Int32 = 0
                    For i As Int32 = uint32_0 To array_size(v) - uint32_1
                        Dim a As Action = Nothing
                        a = v(i)
                        If Interlocked.Increment(r) <= Environment.ProcessorCount() Then
                            queue_in_managed_threadpool(Sub()
                                                            e(a)
                                                            assert(Interlocked.Decrement(r) >= 0)
                                                            assert(w.disposed_or_set())
                                                        End Sub)
                        Else
                            assert(Interlocked.Decrement(r) >= 0)
                            assert(w.WaitOne())
                            i -= uint32_1
                        End If
                    Next
                    While r > 0
                        assert(w.WaitOne())
                    End While
                End Using
            End If
        End If
    End Sub
End Class
