﻿
#Const USE_FAST_THREAD_POOL = False
Imports System.Threading
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.lock
Imports osi.root.delegates
Imports osi.root.threadpool

Friend Class threadpool_case
    Private ReadOnly size As Int64
    Private ReadOnly w As Action
    Private ReadOnly before As Action
    Private ReadOnly after As Action

    Public Sub New(ByVal size As Int64,
                   ByVal w As Action,
                   Optional ByVal before As Action = Nothing,
                   Optional ByVal after As Action = Nothing)
        assert(size > 0)
        assert(Not w Is Nothing)
        Me.size = size
        Me.w = w
        Me.before = before
        Me.after = after
    End Sub

    Private Function run_case() As Boolean
        Dim p As pointer(Of Int64) = Nothing
        p = New pointer(Of Int64)()
#If USE_FAST_THREAD_POOL Then
        Dim tp As fast_threadpool = Nothing
#Else
        Dim tp As ithreadpool = Nothing
#End If
        tp = thread_pool()
        Using c = New ms_timing_counter(p)
            assert(Not tp Is Nothing)
            void_(before)
            Dim size As Int64 = 0
            size = Me.size
            For i As Int32 = 0 To min((Environment.ProcessorCount() >> 1) - 1, 0)
                Dim t As Thread = Nothing
                t = New Thread(Sub()
                                   While Interlocked.Decrement(size) >= 0
                                       tp.queue_job(w)
                                   End While
                               End Sub)
                t.Start()
            Next
            timeslice_sleep_wait_until(Function() size <= 0)
            Using New thread_lazy()
                timeslice_sleep_wait_until(Function() tp.idle())
            End Using
            void_(after)
        End Using
        If Not tp Is Nothing Then
            raise_error("finished threadpool ", tp.GetType().FullName(), " test, used time in milliseconds ", +p)
        End If
        Return True
    End Function

    'make sure the latest one is the default threadpool used in utt
    Public Function run() As Boolean
        register_managed_threadpool()
        run_case()
        register_qless_threadpool()
        run_case()
        register_slimheapless_threadpool()
        run_case()
        register_slimqless2_threadpool()
        run_case()
        Return True
    End Function
End Class
