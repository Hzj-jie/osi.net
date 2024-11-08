
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with waitable_slimqless2.vbp ----------
'so change waitable_slimqless2.vbp instead of this file



Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
' count_reset_event is in osi.root.event, which is not included in osi.root.formation.

Public NotInheritable Class waitable_slimqless2(Of T)
    Implements IDisposable

    Private ReadOnly q As New slimqless2(Of T)()
    Private ReadOnly are As New AutoResetEvent(False)

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub push(ByVal v As T)
        emplace(copy_no_error(v))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub emplace(ByVal v As T)
        q.emplace(v)
        assert(are.force_set())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pop(ByRef o As T) As Boolean
        Return q.pop(o)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pop() As T
        Return q.pop()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pick(ByRef o As T) As Boolean
        Return q.pick(o)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pick() As T
        Return q.pick()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return q.empty()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait()
        assert(are.wait())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function wait(ByVal ms As Int64) As Boolean
        If ms < 0 Then
            wait()
            Return True
        End If
        Return are.wait(ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        ' If one push happens concurrently, are may be released one more time.
        assert(are.force_reset())
        q.clear()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        are.Close()
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        are.Close()
        MyBase.Finalize()
    End Sub
End Class

'finish waitable_slimqless2.vbp --------
