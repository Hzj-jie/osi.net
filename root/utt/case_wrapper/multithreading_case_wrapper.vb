
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants

<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")>
Public Class multithreading_case_wrapper
    Inherits case_wrapper

    <ThreadStatic()> Private Shared id As Int32
    <ThreadStatic()> Private Shared this As multithreading_case_wrapper
    Private ReadOnly tc As UInt32 = 0
    Private ReadOnly start_are As AutoResetEvent
    Private ReadOnly accept_mre As ManualResetEvent
    Private ReadOnly finish_are As AutoResetEvent
    Private running_thread As Int32 = 0
    Private accepted As Boolean = False
    Private failed As Boolean = False

    Public Shared Function valid_thread_count(ByVal tc As UInt32) As Boolean
        Return tc > 1 OrElse envs.single_cpu
    End Function

    Public Sub New(ByVal c As [case], Optional ByVal threadcount As UInt32 = 8)
        MyBase.New(c)
        Me.tc = threadcount
        start_are = New AutoResetEvent(False)
        accept_mre = New ManualResetEvent(False)
        finish_are = New AutoResetEvent(False)
    End Sub

    Public Sub New(ByVal c As [case], ByVal threadcount As Int32)
        Me.New(c, assert_which.of(threadcount).can_cast_to_uint32())
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return CShort(min(max(threadcount(), MyBase.reserved_processors()), max_int16))
    End Function

    Protected Overridable Function threadcount() As UInt32
        Return tc
    End Function

    Private Sub workon(ByVal input As Object)
        Interlocked.Increment(running_thread)
        assert(start_are.Set())
        id = direct_cast(Of Int32)(input)
        this = Me
        Using defer.to(Sub()
                           id = npos
                           this = Nothing
                           Interlocked.Decrement(running_thread)
                           assert(finish_are.Set())
                       End Sub)
            assert(accept_mre.WaitOne())
            If failed Then
                Return
            Else
                assert(accepted)
            End If
            If Not do_(AddressOf MyBase.run, False) Then
                failed = True
            End If
        End Using
    End Sub

    Public Shared Function current() As multithreading_case_wrapper
        assert(Not this Is Nothing)
        Return this
    End Function

    Public Shared Function running_thread_count() As UInt32
        Dim r As Int32 = 0
        r = current().running_thread
        assert(r >= 0)
        Return CUInt(r)
    End Function

    Public Shared Function thread_id() As UInt32
        Dim r As Int32 = 0
        r = id
        assert(r >= 0)
        Return CUInt(r)
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        assert(valid_thread_count(threadcount()))
        accepted = False
        failed = False
        start_are.Reset()
        accept_mre.Reset()
        finish_are.Reset()
        Dim ts() As Thread = Nothing
        ReDim ts(CInt(threadcount()) - 1)
        For i As Int32 = 0 To CInt(threadcount()) - 1
            ts(i) = New Thread(AddressOf workon)
            ts(i).Name() = "MULTITHREADING_CASE_WRAPPER_THREAD"
            ts(i).Start(i)
        Next
        Dim wait_round As Int32 = 1000
        While _dec(wait_round) > 0
            start_are.wait(max(envs.timeslice_length_ms \ 2, 1))
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
        For i As Int32 = 0 To CInt(threadcount()) - 1
            ts(i).Abort()
            ts(i).Join()
        Next
        Return Not failed
    End Function

    Protected Overrides Sub Finalize()
        start_are.Close()
        start_are.Dispose()
        accept_mre.Close()
        accept_mre.Dispose()
        finish_are.Close()
        finish_are.Dispose()
        MyBase.Finalize()
    End Sub
End Class
