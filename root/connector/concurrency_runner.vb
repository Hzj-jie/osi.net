
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.constants
Imports osi.root.template

Public NotInheritable Class concurrency_runner
    Public Shared ReadOnly instance As New concurrency_runner(Of _NPOS)()

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
    Private Shared ReadOnly size As UInt32 = calculate_size()
    <ThreadStatic> Private Shared is_concurrency_runner_thread As Boolean

    Private Shared Function calculate_size() As UInt32
        Dim size As UInt32 = 0
        Dim c As Int64 = +(alloc(Of _SIZE)())
        If c = npos Then
            size = CUInt(Environment.ProcessorCount())
        ElseIf c >= max_uint32 Then
            size = max_uint32
        ElseIf c > 0 Then
            size = CUInt(c)
        Else
            assert(False)
        End If
        Return size
    End Function

    Public Shared Function in_concurrency_runner_thread() As Boolean
        Return is_concurrency_runner_thread
    End Function

    ' Queues actions into concurrency_runner, and block until all the jobs are finished.
    Public Sub execute(ByVal ParamArray v() As Action)
        If v.null_or_empty() Then
            Return
        End If
        If array_size(v) = uint32_1 Then
            e(v(0))
            Return
        End If
        If size = 1 Then
            For i As Int32 = 0 To array_size_i(v) - 1
                e(v(i))
            Next
            Return
        End If
        Using w As AutoResetEvent = New AutoResetEvent(False)
            Dim r As Int32 = 0
            For i As Int32 = 0 To array_size_i(v) - 1
                Dim a As Action = Nothing
                a = v(i)
                If Interlocked.Increment(r) <= size Then
                    managed_thread_pool.push(Sub()
                                                 e(a)
                                                 assert(Interlocked.Decrement(r) >= 0)
                                                 assert(w.disposed_or_set())
                                             End Sub)
                Else
                    assert(Interlocked.Decrement(r) >= 0)
                    assert(w.WaitOne())
                    i -= 1
                End If
            Next
            While r > 0
                assert(w.WaitOne())
            End While
        End Using
    End Sub

    Public Sub execute(Of T)(ByVal v As Action(Of T), ByVal ParamArray c() As T)
        assert(Not v Is Nothing)
        If isemptyarray(c) Then
            Return
        End If
        Dim a(array_size_i(c) - 1) As Action
        For i As Int32 = 0 To array_size_i(c) - 1
            Dim n As T = c(i)
            a(i) = Sub()
                       v(n)
                   End Sub
        Next
        execute(a)
    End Sub

    Private Sub e(ByVal v As Action)
        is_concurrency_runner_thread = True
        void_(v)
        is_concurrency_runner_thread = False
    End Sub
End Class
