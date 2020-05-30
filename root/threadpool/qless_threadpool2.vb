
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless_threadpool2.vbp ----------
'so change qless_threadpool2.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with slimqless2_threadpool2.vbp ----------
'so change slimqless2_threadpool2.vbp instead of this file



Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Public NotInheritable Class qless_threadpool2
    Implements IDisposable

    Private ReadOnly q As waitable_qless(Of Action)
    Private ReadOnly rs() As qless_runner

    Public Sub New()
        q = New waitable_qless(Of Action)()
        ReDim rs(CInt(threadpool.default_thread_count - uint32_1))
        For i As Int32 = 0 To array_size_i(rs) - 1
            rs(i) = New qless_runner(q)
        Next
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function current_thread_is_managed() As Boolean
        Return qless_runner.current_thread_is_managed()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function in_managed_thread() As Boolean
        Return qless_runner.current_thread_is_managed()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function running_in_current_thread() As Boolean
        For i As Int32 = 0 To array_size_i(rs) - 1
            If rs(i).running_in_current_thread() Then
                Return True
            End If
        Next
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function thread_count() As UInt32
        Return threadpool.default_thread_count
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function stopping() As Boolean
        For i As Int32 = 0 To array_size_i(rs) - 1
            If rs(i).stopping() Then
                Return True
            End If
        Next
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [stop]() As Boolean
        If object_compare(Me, newable_global_instance(Of qless_threadpool2).ref()) = 0 Then
            Return False
        End If
        Dim r As Boolean = False
        r = True
        For i As Int32 = 0 To array_size_i(rs) - 1
            If Not rs(i).stop() Then
                r = False
            End If
        Next
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function idle() As Boolean
        Return q.empty()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function wait(ByVal ms As Int64) As Boolean
        Return q.wait(ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function wait_job(ByVal ms As Int64) As Boolean
        Return wait(ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait()
        q.wait()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait_job()
        wait()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function execute() As Boolean
        Return rs(0).execute()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function execute_job() As Boolean
        Return execute()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function push(ByVal v As Action) As Boolean
        Return rs(0).push(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function queue_job(ByVal v As Action) As Boolean
        Return push(v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function join(ByVal ms As Int64) As Boolean
        For i As Int32 = 0 To array_size_i(rs) - 1
            If Not rs(i).join(ms) Then
                Return False
            End If
        Next
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub join()
        For i As Int32 = 0 To array_size_i(rs) - 1
            rs(i).join()
        Next
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        [stop]()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal this As qless_threadpool2, ByVal that As Action) As qless_threadpool2
        assert(Not this Is Nothing)
        this.push(that)
        Return this
    End Operator
End Class
'finish slimqless2_threadpool2.vbp --------
'finish qless_threadpool2.vbp --------
