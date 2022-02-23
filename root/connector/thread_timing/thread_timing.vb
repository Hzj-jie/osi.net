
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Diagnostics
Imports System.Threading
Imports osi.root.constants

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

    Public Shared ReadOnly realtime As nice = New nice(process_priority.realtime, thread_priority.highest)
    Public Shared ReadOnly boost As nice = New nice(process_priority.high, thread_priority.highest)
    Public Shared ReadOnly moderate As nice = New nice(process_priority.normal, thread_priority.normal)
    Public Shared ReadOnly lazy As nice = New nice(process_priority.idle, thread_priority.lowest)
    Public Shared ReadOnly process_realtime As nice = New nice(process_priority.realtime)
    Public Shared ReadOnly process_boost As nice = New nice(process_priority.high)
    Public Shared ReadOnly process_moderate As nice = New nice(process_priority.normal)
    Public Shared ReadOnly process_lazy As nice = New nice(process_priority.idle)
    Public Shared ReadOnly thread_boost As nice = New nice(thread_priority.highest)
    Public Shared ReadOnly thread_moderate As nice = New nice(thread_priority.normal)
    Public Shared ReadOnly thread_lazy As nice = New nice(thread_priority.lowest)

    <global_init(global_init_level.runtime_checkers)>
    Private NotInheritable Class assertions
        Private Shared Sub init()
            assert(DirectCast(Nothing, process_priority) = process_priority.keep)
            assert(DirectCast(Nothing, thread_priority) = thread_priority.keep)
        End Sub

        Private Sub New()
        End Sub
    End Class

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

<CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
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

    <CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
    Public Sub Dispose() Implements IDisposable.Dispose
        If Not n.keep_process_priority() Then
            assert(p IsNot Nothing)
            If ppc <> n.target_process_priority() Then
                'for mono
                Try
                    p.PriorityClass() = ppc
                Catch
                End Try
            End If
        End If
        If Not n.keep_thread_priority() Then
            assert(t IsNot Nothing)
            If otp <> n.target_thread_priority() Then
                t.Priority() = otp
            End If
        End If
        GC.SuppressFinalize(Me)
    End Sub
End Class
