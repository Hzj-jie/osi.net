
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports tp = System.Threading.ThreadPool

Public NotInheritable Class managed_threadpool
    Private Sub New(Optional ByVal thread_count As UInt32 = uint32_0)
#If 0 Then
        If thread_count = 0 Then
            thread_count = default_thread_count
        End If
        Me.thread_count() = thread_count
#End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function in_managed_thread() As Boolean
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function running_in_current_thread() As Boolean
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function thread_count() As UInt32
        Dim ft As Int32 = 0
        Dim bt As Int32 = 0
        tp.GetMaxThreads(ft, bt)

        Dim aft As Int32 = 0
        Dim abt As Int32 = 0
        tp.GetAvailableThreads(aft, abt)

        Dim mft As Int32 = 0
        Dim mbt As Int32 = 0
        tp.GetMinThreads(mft, mbt)

        Dim r As Int32 = 0
        r = ft + bt - aft - abt
        r = max(r, mft + mbt)
        If r <= 0 Then
            Return 0
        End If
        Return CUInt(r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function stopping() As Boolean
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [stop]() As Boolean
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function idle() As Boolean
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function wait(ByVal ms As Int64) As Boolean
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function execute() As Boolean
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function push(ByVal v As Action) As Boolean
        queue_in_managed_threadpool(v)
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function join(ByVal ms As Int64) As Boolean
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub join()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal this As managed_threadpool, ByVal that As Action) As managed_threadpool
        assert(Not this Is Nothing)
        this.push(that)
        Return this
    End Operator

    Public Shared ReadOnly [global] As managed_threadpool = New managed_threadpool()
End Class
