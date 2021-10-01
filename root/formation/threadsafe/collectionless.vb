
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public NotInheritable Class collectionless(Of T)
    Private ReadOnly f As New free_list(Of T)()
    Private l As lock_t

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        l.wait()
        f.clear()
        l.release()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function free_pool_size() As UInt32
        Dim r As UInt32 = 0
        l.wait()
        r = f.free_pool_size()
        l.release()
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pool_size() As UInt32
        Dim r As UInt32 = 0
        l.wait()
        r = f.pool_size()
        l.release()
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace(ByVal v As T) As UInt32
        Dim r As UInt32 = uint32_0
        l.wait()
        r = f.emplace(v)
        l.release()
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function push(ByVal v As T) As UInt32
        Dim r As UInt32 = uint32_0
        l.wait()
        r = f.push(v)
        l.release()
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub [erase](ByVal p As UInt32)
        l.wait()
        f.erase(p)
        l.release()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        Dim r As UInt32 = 0
        l.wait()
        r = f.size()
        l.release()
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Dim r As Boolean = False
        l.wait()
        r = f.empty()
        l.release()
        Return r
    End Function

    Default Public ReadOnly Property at(ByVal p As UInt32) As T
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
            Dim r As T = Nothing
            l.wait()
            r = f(p)
            l.release()
            Return r
        End Get
    End Property

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function has(ByVal p As UInt32) As Boolean
        Dim r As Boolean = False
        l.wait()
        r = f.has(p)
        l.release()
        Return r
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function optionally(ByVal p As UInt32) As [optional](Of T)
        Dim r As [optional](Of T) = Nothing
        l.wait()
        r = f.optionally(p)
        l.release()
        Return r
    End Function
End Class
