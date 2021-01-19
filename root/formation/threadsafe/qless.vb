
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.lock.slimlock

'a thread-safe queue
Public Class qless(Of T, lock_t As islimlock)
    Private f As ref_node(Of T) = Nothing
    Private e As ref_node(Of T) = Nothing
    Private _size As UInt32 = uint32_0
    Private l As lock_t

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        Return _size
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return size() = uint32_0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        l.wait()
        f = Nothing
        e = Nothing
        _size = uint32_0
        l.release()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub push(ByVal d As T)
        emplace(copy_no_error(d))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub emplace(ByVal d As T)
        Dim n As ref_node(Of T) = Nothing
        n = New ref_node(Of T)(1)
        n.emplace(d)
        l.wait()
        If e Is Nothing Then
#If DEBUG Then
            assert(f Is Nothing)
            assert(_size = 0)
#End If
            f = n
        Else
            e.ref(0) = n
        End If
        e = n
        _size += uint32_1
        l.release()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pop() As T
        Dim o As T = Nothing
        If pop(o) Then
            Return o
        End If
        Return Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pop(ByRef o As T) As Boolean
        Dim rtn As Boolean = False
        l.wait()
        If f Is Nothing Then
#If DEBUG Then
            assert(e Is Nothing)
            assert(_size = 0)
#End If
            rtn = False
        Else
            o = +f
            f = f.ref(0)
            If f Is Nothing Then
                e = Nothing
            End If
#If DEBUG Then
            assert(_size > 0)
#End If
            _size -= uint32_1
            rtn = True
        End If
        l.release()
        Return rtn
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pick() As T
        Dim o As T = Nothing
        If pick(o) Then
            Return o
        End If
        Return Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pick(ByRef o As T) As Boolean
        Dim rtn As Boolean = False
        l.wait()
        If f Is Nothing Then
            rtn = False
        Else
            rtn = True
            o = f.data()
        End If
        l.release()
        Return rtn
    End Function
End Class

Public NotInheritable Class qless(Of T)
    Inherits qless(Of T, slimlock.monitorlock)
End Class

