
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless_runner.vbp ----------
'so change qless_runner.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with slimqless2_runner.vbp ----------
'so change slimqless2_runner.vbp instead of this file


Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock

Public NotInheritable Class qless_runner
    <ThreadStatic()> Private Shared current As qless_runner
    Private ReadOnly q As waitable_qless(Of Action)
    Private ReadOnly t As Thread
    Private s As singleentry

    Public Sub New()
        Me.New(New waitable_qless(Of Action)())
    End Sub

    Public Sub New(ByVal q As waitable_qless(Of Action))
        assert(Not q Is Nothing)
        Me.q = q
        Me.t = New Thread(Sub()
                              current = Me
                              Try
                                  While Not stopping()
                                      If Not execute() Then
                                          wait()
                                      End If
                                  End While
                              Finally
                                  current = Nothing
                              End Try
                          End Sub)
        Me.t.Name() = "qless_runner_thread"
        t.Start()
        assert(Not stopping())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function current_thread_is_managed() As Boolean
        Return Not current Is Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function running_in_current_thread() As Boolean
        Return object_compare(current, Me) = 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function stopping() As Boolean
        Return s.in_use() AndAlso Not stopped()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function stopped() As Boolean
        Return (t.ThreadState() = ThreadState.Aborted OrElse
                t.ThreadState() = ThreadState.Stopped) AndAlso
               assert(s.in_use())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [stop]() As Boolean
        If Not s.mark_in_use() Then
            Return False
        End If
        t.Abort()
        GC.SuppressFinalize(Me)
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function join(ByVal timeout_ms As Int64) As Boolean
        assert(Not running_in_current_thread())
        If timeout_ms > max_int32 Then
            Return t.Join(max_int32)
        End If
        If timeout_ms < 0 Then
            Return t.Join(Timeout.Infinite)
        End If
        Return t.Join(CInt(timeout_ms))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub join()
        t.Join()
    End Sub

    ' Execute one pending job in current thread.
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function execute() As Boolean
        Dim c As Action = Nothing
        If q.pop(c) Then
            void_(c)
            Return True
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function run_until_idle() As UInt32
        Dim r As UInt32 = uint32_0
        While execute()
            r += uint32_1
        End While
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function wait(ByVal ms As Int64) As Boolean
        Return q.wait(ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait()
        q.wait()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function idle() As Boolean
        Return q.empty()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function push(ByVal job As Action) As Boolean
        If job Is Nothing Then
            Return False
        End If
        q.emplace(job)
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal this As qless_runner, ByVal that As Action) As qless_runner
        If this Is Nothing Then
            Return Nothing
        End If
        this.push(that)
        Return this
    End Operator

    Protected Overrides Sub Finalize()
        [stop]()
        MyBase.Finalize()
    End Sub
End Class
'finish slimqless2_runner.vbp --------
'finish qless_runner.vbp --------
