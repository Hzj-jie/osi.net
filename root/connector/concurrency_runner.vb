
Imports System.Threading
Imports osi.root.constants
Imports osi.root.template

Public NotInheritable Class concurrency_runner
    Public Shared ReadOnly instance As concurrency_runner(Of _NPOS)

    Shared Sub New()
        instance = New concurrency_runner(Of _NPOS)()
    End Sub

    Public Shared Sub execute(ByVal ParamArray v() As Action)
        instance.execute(v)
    End Sub

    Public Shared Sub execute(Of T)(ByVal v As Action(Of T), ByVal ParamArray c() As T)
        instance.execute(v, c)
    End Sub

    Private Sub New()
    End Sub
End Class

' An executor to concurrently execute processor-count tasks in managed threadpool. To maximize processor usage, one
' should not queue a sleep or wait or io operation in a concurrency_runner.
Public NotInheritable Class concurrency_runner(Of _SIZE As _int64)
    Private Shared ReadOnly size As UInt32
    <ThreadStatic> Private Shared is_concurrency_runner_thread As Boolean

    Shared Sub New()
        Dim c As Int64 = 0
        c = +(alloc(Of _SIZE)())
        If c = npos Then
            size = Environment.ProcessorCount()
        ElseIf c > 0 Then
            If c >= max_uint32 Then
                size = max_uint32
            Else
                size = CUInt(c)
            End If
        Else
            assert(False)
        End If
    End Sub

    Public Shared Function in_concurrency_runner_thread() As Boolean
        Return is_concurrency_runner_thread
    End Function

    ' Queues actions into concurrency_runner, and block until all the jobs are finished.
    Public Sub execute(ByVal ParamArray v() As Action)
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

    Public Sub execute(Of T)(ByVal v As Action(Of T), ByVal ParamArray c() As T)
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
End Class
