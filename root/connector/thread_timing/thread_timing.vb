
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics
Imports System.Threading

Public Structure nice
    Public Enum process_priority
        keep = 0
        realtime
        high
        above_normal
        normal
        below_normal
        idle
    End Enum

    Public Enum thread_priority
        keep = 0
        highest
        above_normal
        normal
        below_normal
        lowest
    End Enum

    Public Shared ReadOnly realtime As nice
    Public Shared ReadOnly boost As nice
    Public Shared ReadOnly moderate As nice
    Public Shared ReadOnly lazy As nice
    Public Shared ReadOnly process_realtime As nice
    Public Shared ReadOnly process_boost As nice
    Public Shared ReadOnly process_moderate As nice
    Public Shared ReadOnly process_lazy As nice
    Public Shared ReadOnly thread_boost As nice
    Public Shared ReadOnly thread_moderate As nice
    Public Shared ReadOnly thread_lazy As nice

    Shared Sub New()
        assert(DirectCast(Nothing, process_priority) = process_priority.keep)
        assert(DirectCast(Nothing, thread_priority) = thread_priority.keep)

        realtime = New nice(process_priority.realtime, thread_priority.highest)
        boost = New nice(process_priority.high, thread_priority.highest)
        moderate = New nice(process_priority.normal, thread_priority.normal)
        lazy = New nice(process_priority.idle, thread_priority.lowest)

        process_realtime = New nice(process_priority.realtime)
        process_boost = New nice(process_priority.high)
        process_moderate = New nice(process_priority.normal)
        process_lazy = New nice(process_priority.idle)

        thread_boost = New nice(thread_priority.highest)
        thread_moderate = New nice(thread_priority.normal)
        thread_lazy = New nice(thread_priority.lowest)
    End Sub

    Private ReadOnly pp As process_priority
    Private ReadOnly tp As thread_priority

    Public Sub New(ByVal process_priority As process_priority,
                   ByVal thread_priority As thread_priority)
        pp = process_priority
        tp = thread_priority
    End Sub

    Public Sub New(ByVal process_priority As process_priority)
        Me.New(process_priority, thread_priority.keep)
    End Sub

    Public Sub New(ByVal thread_priority As thread_priority)
        Me.New(process_priority.keep, thread_priority)
    End Sub

    Public Function keep_process_priority() As Boolean
        Return pp = process_priority.keep
    End Function

    Public Function target_process_priority() As ProcessPriorityClass
        assert(Not keep_process_priority())
        Select Case pp
            Case process_priority.realtime
                Return ProcessPriorityClass.RealTime
            Case process_priority.high
                Return ProcessPriorityClass.High
            Case process_priority.above_normal
                Return ProcessPriorityClass.AboveNormal
            Case process_priority.normal
                Return ProcessPriorityClass.Normal
            Case process_priority.below_normal
                Return ProcessPriorityClass.BelowNormal
            Case process_priority.idle
                Return ProcessPriorityClass.Idle
            Case Else
                assert(False)
                Return ProcessPriorityClass.Normal
        End Select
    End Function

    Public Function keep_thread_priority() As Boolean
        Return tp = thread_priority.keep
    End Function

    Public Function target_thread_priority() As ThreadPriority
        assert(Not keep_thread_priority())
        Select Case tp
            Case thread_priority.highest
                Return ThreadPriority.Highest
            Case thread_priority.above_normal
                Return ThreadPriority.AboveNormal
            Case thread_priority.normal
                Return ThreadPriority.Normal
            Case thread_priority.below_normal
                Return ThreadPriority.BelowNormal
            Case thread_priority.lowest
                Return ThreadPriority.Lowest
            Case Else
                assert(False)
                Return ThreadPriority.Normal
        End Select
    End Function
End Structure

Public Class priority
    Implements IDisposable

    Private ReadOnly t As Thread
    Private ReadOnly p As Process
    Private ReadOnly n As nice
    Private ReadOnly ppc As ProcessPriorityClass
    Private ReadOnly otp As ThreadPriority

    Public Sub New(Optional ByVal n As nice = Nothing)
        Me.n = n
        If Not n.keep_process_priority() Then
            p = Process.GetCurrentProcess()
            'for mono
            Try
                ppc = p.PriorityClass()
            Catch
            End Try
            If ppc <> n.target_process_priority() Then
                Try
                    p.PriorityClass() = n.target_process_priority()
                Catch
                End Try
            End If
        End If
        If Not n.keep_thread_priority() Then
            t = Thread.CurrentThread()
            otp = t.Priority()
            If otp <> n.target_thread_priority() Then
                t.Priority() = n.target_thread_priority()
            End If
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not n.keep_process_priority() Then
            assert(Not p Is Nothing)
            If ppc <> n.target_process_priority() Then
                'for mono
                Try
                    p.PriorityClass() = ppc
                Catch
                End Try
            End If
        End If
        If Not n.keep_thread_priority() Then
            assert(Not t Is Nothing)
            If otp <> n.target_thread_priority() Then
                t.Priority() = otp
            End If
        End If
        GC.SuppressFinalize(Me)
    End Sub
End Class
