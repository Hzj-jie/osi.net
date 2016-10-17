
Imports System.IO
Imports System.Threading
Imports System.Net.Sockets
Imports osi.root.constants

Public NotInheritable Class disposable
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
        If GetType(T).is(GetType(Stream)) Then
            [default] = Sub(x As T)
                            If Not x Is Nothing Then
                                Dim s As Stream = Nothing
                                s = direct_cast(Of Stream)(x)
                                assert(Not s Is Nothing)
                                s.Flush()
                                s.Close()
                                s.Dispose()
                            End If
                        End Sub
        ElseIf GetType(T).is(GetType(WaitHandle)) Then
            [default] = Sub(x As T)
                            If Not x Is Nothing Then
                                direct_cast(Of WaitHandle)(x).Close()
                            End If
                        End Sub
        ElseIf GetType(T).is(GetType(TcpClient)) Then
            [default] = Sub(x As T)
                            If Not x Is Nothing Then
                                direct_cast(Of TcpClient)(x).Close()
                            End If
                        End Sub
        ElseIf GetType(T).is(GetType(UdpClient)) Then
            [default] = Sub(x As T)
                            If Not x Is Nothing Then
                                direct_cast(Of UdpClient)(x).Close()
                            End If
                        End Sub
        ElseIf GetType(T).is(GetType(Socket)) Then
            [default] = Sub(x As T)
                            If Not x Is Nothing Then
                                direct_cast(Of Socket)(x).Close()
                            End If
                        End Sub
        ElseIf GetType(T).is(GetType(TextWriter)) Then
            [default] = Sub(x As T)
                            close_writer(direct_cast(Of TextWriter)(x))
                        End Sub
        ElseIf GetType(T).is(GetType(IDisposable)) Then
            [default] = Sub(x As T)
                            If Not x Is Nothing Then
                                direct_cast(Of IDisposable)(x).Dispose()
                            End If
                        End Sub
        Else
            [default] = Nothing
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
        assert(Not a Is Nothing)
        a(v)
    End Sub

    Private Sub New()
    End Sub
End Class
