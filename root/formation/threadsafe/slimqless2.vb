
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
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

    Private ReadOnly f As node
    Private e As node

    Public Sub New()
        f = New node()
        e = New node()
        f.next = e
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        While pop(Nothing)
        End While
    End Sub

    Private Sub wait_mark_writting()
        'wait_when(Function() Not e.vs.mark_value_writting())
        'almost copy from spinwait, since it will give a better performance
        If should_yield() Then
            While Not e.vs.mark_value_writting()
                yield()
            End While
        Else
            Dim i As Int32 = 0
            While Not e.vs.mark_value_writting()
                i += 1
                If i > loops_per_yield Then
                    If yield() Then
                        i = 0
                    Else
                        i = loops_per_yield
                    End If
                End If
            End While
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub push(ByVal v As T)
        emplace(copy_no_error(v))
    End Sub

    Public Sub emplace(ByVal v As T)
        Dim ne As node = Nothing
        ne = New node()
        wait_mark_writting()
        Dim n As node = Nothing
        n = e
        atomic.eva(n.next, ne)
        atomic.eva(e, ne)
        n.v = v
        n.vs.mark_value_written()
    End Sub

    Private Sub wait_written(ByVal nf As node)
        assert(Not nf Is Nothing)
        'wait_when(Function() Not nf.vs.value_written())
        'almost copy from spinwait, since it will give a better performance
        If should_yield() Then
            While Not nf.vs.value_written()
                yield()
            End While
        Else
            Dim i As Int32 = 0
            While Not nf.vs.value_written()
                i += 1
                If i > loops_per_yield Then
                    If yield() Then
                        i = 0
                    Else
                        i = loops_per_yield
                    End If
                End If
            End While
        End If
    End Sub

    Public Function pop(ByRef o As T) As Boolean
        Dim nf As node = Nothing
        nf = f.next
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

    Public Function pick(ByRef o As T) As Boolean
        Dim nf As node = Nothing
        nf = f.next
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
