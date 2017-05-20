
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Collections.Generic
Imports System.IO
Imports System.Threading
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.delegates

Public NotInheritable Class disposable
    Private Class type_disposer
        Public ReadOnly t As Type
        Public ReadOnly d As Action(Of Object)

        Public Sub New(ByVal t As Type, ByVal d As Action(Of Object))
            assert(Not t Is Nothing)
            assert(Not d Is Nothing)
            Me.t = t
            Me.d = d
        End Sub

        Public Shared Function [New](Of T)(ByVal d As Action(Of T)) As type_disposer
            assert(Not d Is Nothing)
            Return New type_disposer(GetType(T), d.type_restore())
        End Function
    End Class

    Private Shared ReadOnly td As List(Of type_disposer)

    ' The order is important.
    Shared Sub New()
        td = New List(Of type_disposer)()
        register_base_type(Sub(ByVal s As Stream)
                               If Not s Is Nothing Then
                                   s.Flush()
                                   s.Close()
                                   s.Dispose()
                               End If
                           End Sub)
        register_base_type(Sub(ByVal s As WaitHandle)
                               If Not s Is Nothing Then
                                   s.Close()
                                   s.Dispose()
                               End If
                           End Sub)
        register_base_type(Sub(ByVal s As TcpClient)
                               If Not s Is Nothing Then
                                   s.Close()
                               End If
                           End Sub)
        register_base_type(Sub(ByVal s As UdpClient)
                               If Not s Is Nothing Then
                                   s.Close()
                               End If
                           End Sub)
        register_base_type(Sub(ByVal s As Socket)
                               If Not s Is Nothing Then
                                   s.Close()
                                   s.Dispose()
                               End If
                           End Sub)
        register_base_type(Sub(ByVal s As TextWriter)
                               close_writer(s)
                           End Sub)
        register_base_type(Sub(ByVal s As IDisposable)
                               s.not_null_and_dispose()
                           End Sub)
    End Sub

    ' By registering with a Type, some disposer(Of T) may be initialized with the d, so unregister usually won't take
    ' effect.
    Public Shared Sub register(ByVal t As Type, ByVal d As Action(Of Object))
        td.Add(New type_disposer(t, d))
    End Sub

    Public Shared Sub register_base_type(Of T)(ByVal d As Action(Of T))
        td.Add(type_disposer.[New](d))
    End Sub

    Public Shared Function find(Of T)(ByVal type As Type, ByRef o As Action(Of T)) As Boolean
        For Each p As type_disposer In td
            If type.is(p.t) Then
                o = p.d.type_erasure(Of T)()
                Return True
            End If
        Next
        Return False
    End Function

    Public Shared Function find(Of T)(ByRef o As Action(Of T)) As Boolean
        Return find(GetType(T), o)
    End Function

    Public Shared Function find(Of T)() As Action(Of T)
        Dim o As Action(Of T) = Nothing
        If find(o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    Public Shared Sub register(Of T)(ByVal d As Action(Of T))
        disposable(Of T).register(d)
    End Sub

    Public Shared Sub dispose(Of T)(ByVal v As T)
        disposable(Of T).dispose(v)
    End Sub

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class disposable(Of T)
    Private Shared ReadOnly [default] As Action(Of T)

    Shared Sub New()
        If type_info(Of T).is_object Then
            raise_error(error_type.performance, "disposable(Of Object) is definitely a performance destroyer.")
            [default] = Sub(ByVal i As T)
                            If Not i Is Nothing Then
                                Dim a As Action(Of T) = Nothing
                                If disposable.find(i.GetType(), a) Then
                                    assert(Not a Is Nothing)
                                    a(i)
                                End If
                            End If
                        End Sub
        Else
            [default] = disposable.find(Of T)()
        End If
    End Sub

    Public Shared Sub register(ByVal d As Action(Of T))
        assert(Not binder(Of Action(Of T), disposer_binder_protector).has_global_value())
        binder(Of Action(Of T), disposer_binder_protector).set_global(d)
    End Sub

    Public Shared Sub unregister()
        assert(binder(Of Action(Of T), disposer_binder_protector).has_global_value())
        binder(Of Action(Of T), disposer_binder_protector).set_global(Nothing)
    End Sub

    Public Shared Function D() As Action(Of T)
        If binder(Of Action(Of T), disposer_binder_protector).has_global_value() Then
            Return binder(Of Action(Of T), disposer_binder_protector).global()
        Else
            Return [default]
        End If
    End Function

    Public Shared Sub dispose(ByVal v As T)
        Dim a As Action(Of T) = Nothing
        a = D()
        If Not a Is Nothing Then
            a(v)
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
