﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.lock

Public NotInheritable Class slimqless2(Of T)
    Private Structure value_status
        Private Const nv As Int32 = DirectCast(Nothing, Int32)
        Private Const bw As Int32 = nv + 1
        Private Const aw As Int32 = bw + 1

        Private v As Int32

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function mark_value_writting() As Boolean
            Return atomic.compare_exchange(v, bw, nv)
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Sub mark_value_written()
            assert(v = bw)
            atomic.eva(v, aw)
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function value_written() As Boolean
            assert(not_no_value())
            Return v = aw
        End Function

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function not_no_value() As Boolean
            Return v <> nv
        End Function
    End Structure

    Private NotInheritable Class node
        Public [next] As node
        Public v As T
        Public vs As value_status
    End Class

    Private e As New node()
    Private ReadOnly f As New node() With {.[next] = e}

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        While pop(Nothing)
        End While
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub push(ByVal v As T)
        emplace(copy_no_error(v))
    End Sub

    ' TODO: Why using value_status does not work?
    Private Structure mark_value_writting_d
        Implements ifunc(Of node, Boolean)

        Public Function run(ByRef e As node) As Boolean Implements ifunc(Of node, Boolean).run
#If DEBUG Then
            assert(Not e Is Nothing)
#End If
            Return Not e.vs.mark_value_writting()
        End Function
    End Structure

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub emplace(ByVal v As T)
        Dim ne As New node()
        wait_when(New mark_value_writting_d(), e)
        Dim n As node = e
        atomic.eva(n.next, ne)
        atomic.eva(e, ne)
        n.v = v
        n.vs.mark_value_written()
    End Sub

    Private Structure wait_written_d
        Implements ifunc(Of node, Boolean)

        Public Function run(ByRef n As node) As Boolean Implements ifunc(Of node, Boolean).run
#If DEBUG Then
            assert(Not n Is Nothing)
#End If
            Return Not n.vs.value_written()
        End Function
    End Structure

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Sub wait_written(ByVal nf As node)
        assert(Not nf Is Nothing)
        wait_when(New wait_written_d(), nf)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pop(ByRef o As T) As Boolean
        Dim nf As node = f.next
        While True
            assert(Not nf Is Nothing)
            If nf Is e Then
                Return False
            End If
            assert(Not nf.next Is Nothing)
            If Interlocked.CompareExchange(f.next, nf.next, nf) Is nf Then
                assert(nf.vs.not_no_value())
                wait_written(nf)
                o = nf.v
                Return True
            End If
            nf = f.next
        End While
        Return assert(False)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pop() As T
        Dim o As T = Nothing
        If pop(o) Then
            Return o
        End If
        Return Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pick(ByRef o As T) As Boolean
        Dim nf As node = f.next
        If nf Is e Then
            Return False
        End If
        wait_written(nf)
        o = nf.v
        Return True
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
    Public Function empty() As Boolean
        Return f.next Is e
    End Function
End Class
