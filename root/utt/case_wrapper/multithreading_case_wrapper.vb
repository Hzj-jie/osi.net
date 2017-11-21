﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils

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

    Public Shared Function valid_thread_count(ByVal tc As Int32) As Boolean
        Return tc > 1 OrElse envs.single_cpu
    End Function

    Public Sub New(ByVal c As [case], Optional ByVal threadcount As Int32 = 8)
        MyBase.New(c)
        Me.tc = threadcount
        start_are = New AutoResetEvent(False)
        accept_mre = New ManualResetEvent(False)
        finish_are = New AutoResetEvent(False)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return CShort(min(max(threadcount(), MyBase.reserved_processors()), max_int16))
    End Function

    Protected Overridable Function threadcount() As Int32
        Return tc
    End Function

    Private Sub workon(ByVal input As Object)
        Dim i As Int32 = 0
        i = direct_cast(Of Int32)(input)
        Interlocked.Increment(running_thread)
        assert(start_are.Set())
        id = i
        this = Me
        Using defer(Sub()
                        id = npos
                        this = Nothing
                        assert(finish_are.Set())
                        Interlocked.Decrement(running_thread)
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

    Public Shared Function running_thread_count() As Int32
        Return current().running_thread
    End Function

    Public Shared Function thread_id() As Int32
        assert(id <> npos)
        Return id
    End Function

    Public NotOverridable Overrides Function run() As Boolean
        assert(valid_thread_count(threadcount()))
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
            start_are.wait(envs.half_timeslice_length_ms)
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
