
Imports System.Threading
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.delegates
Imports osi.root.lock
Imports osi.root.connector

Public Class multithreading_case_wrapper
    Inherits case_wrapper

    <ThreadStatic()> Private Shared id As Int32
    <ThreadStatic()> Private Shared this As multithreading_case_wrapper
    Private ReadOnly tc As Int32 = 0
    Private ReadOnly start_are As AutoResetEvent
    Private ReadOnly accept_mre As ManualResetEvent
    Private ReadOnly finish_are As AutoResetEvent
    Private running_thread As Int32 = 0
    Private accepted As Boolean = False
    Private failed As Boolean = False

    Public Sub New(ByVal c As [case], Optional ByVal threadcount As Int32 = 8)
        MyBase.New(c)
        Me.tc = threadcount
        start_are = New AutoResetEvent(False)
        accept_mre = New ManualResetEvent(False)
        finish_are = New AutoResetEvent(False)
    End Sub

    Public Overrides Function preserved_processors() As Int16
        Return max(threadcount(), MyBase.preserved_processors())
    End Function

    Protected Overridable Function threadcount() As Int32
        Return tc
    End Function

    Private Sub workon(ByVal i As Int32)
        Interlocked.Increment(running_thread)
        assert(start_are.Set())
        id = i
        this = Me
        assert(accept_mre.WaitOne())
        If failed Then
            Return
        Else
            assert(accepted)
        End If
        If Not do_(AddressOf MyBase.run, False) Then
            failed = True
        End If
        this = Nothing
        id = npos
        Interlocked.Decrement(running_thread)
        assert(finish_are.Set())
    End Sub

    Public Shared Function current() As multithreading_case_wrapper
        assert(Not this Is Nothing)
        Return this
    End Function

    Public Shared Function running_thread_count() As Int32
        Return current().running_thread
    End Function

    Public Shared Function thread_id() As Int32
        assert(id <> npos)
        Return id
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        assert(Me.threadcount() > 1 OrElse envs.single_cpu)
        accepted = False
        failed = False
        start_are.Reset()
        accept_mre.Reset()
        finish_are.Reset()
        Dim ts() As Thread = Nothing
        ReDim ts(threadcount() - 1)
        For i As Int32 = 0 To threadcount() - 1
            ts(i) = New Thread(AddressOf workon)
            ts(i).Name() = "MULTITHREADING_CASE_WRAPPER_THREAD"
            ts(i).Start(i)
        Next
        Dim wait_round As Int32 = 1000
        While _dec(wait_round) > 0
            start_are.WaitOne(envs.half_timeslice_length_ms)
            assert(running_thread <= threadcount())
            If running_thread = threadcount() Then
                Exit While
            End If
        End While
        If wait_round = 0 Then
            failed = True
        Else
            accepted = True
        End If
        assert(accept_mre.Set())
        While assert(finish_are.WaitOne()) AndAlso running_thread > 0
        End While
        For i As Int32 = 0 To threadcount() - 1
            ts(i).Abort()
            ts(i).Join()
        Next
        Return Not failed
    End Function

    Protected Overrides Sub Finalize()
        start_are.Close()
        accept_mre.Close()
        finish_are.Close()
        MyBase.Finalize()
    End Sub
End Class
