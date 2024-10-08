
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless2.vbp ----------
'so change qless2.vbp instead of this file



Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock

Public NotInheritable Class qless2(Of T)
    Private ReadOnly q As slimqless2(Of T)
    Private _size As Int32

    Public Sub New()
        q = New slimqless2(Of T)()
        _size = 0
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        '< 0 is in partial of pop operation
        Dim v As Int32 = 0
        v = _size
        Return If(v < 0, uint32_0, CUInt(v))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        While pop(Nothing)
        End While
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pop() As T
        Dim o As T = Nothing
        If pop(o) Then
            Return o
        End If
        Return Nothing
    End Function

    ' For a single thread reader,
    ' if (!q.empty()) assert(q.pop(r))
    ' should never fail.
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pop(ByRef d As T) As Boolean
        If atomic.decrement(_size, 0) Then
            Return assert(q.pop(d))
        End If
        Return False
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub push(ByVal d As T)
        emplace(copy_no_error(d))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub emplace(ByVal d As T)
        q.emplace(d)
        Interlocked.Increment(_size)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pick(ByRef d As T) As Boolean
        Return q.pick(d)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pick() As T
        Return q.pick()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function push(ByVal d As T, ByVal max_size As UInt32) As Boolean
        Return emplace(copy_no_error(d), max_size)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace(ByVal d As T, ByVal max_size As UInt32) As Boolean
        If atomic.increment(_size, max_size) Then
            q.emplace(d)
            Return True
        End If
        Return False
    End Function
End Class

'finish qless2.vbp --------
