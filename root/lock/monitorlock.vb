
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants

Public Structure monitorlock
    Implements ilock

    Private obj As Object
    Private in_use As Int32

    Public Sub New(ByVal i As Object)
        assert(i IsNot Nothing)
        Me.obj = i
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait() Implements ilock.wait
        atomic.create_if_nothing(obj)
        Monitor.Enter(obj)
        in_use += 1
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub release() Implements ilock.release
        in_use -= 1
#If DEBUG Then
        assert(in_use >= 0)
#End If
        Monitor.Exit(obj)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function can_thread_owned() As Boolean Implements ilock.can_thread_owned
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function can_cross_thread() As Boolean Implements ilock.can_cross_thread
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function held() As Boolean Implements ilock.held
        Return in_use > 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function held_in_thread() As Boolean Implements ilock.held_in_thread
        If Not held() Then
            Return False
        End If
        If Not Monitor.TryEnter(obj) Then
            Return False
        End If
        Dim rtn As Boolean = False
        rtn = held()
        Monitor.Exit(obj)
        Return rtn
    End Function
End Structure
