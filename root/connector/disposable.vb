
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
            assert(t IsNot Nothing)
            assert(d IsNot Nothing)
            Me.t = t
            Me.d = d
        End Sub

        Public Shared Function [New](Of T)(ByVal d As Action(Of T)) As type_disposer
            assert(d IsNot Nothing)
            Return New type_disposer(GetType(T), d.type_restore())
        End Function
    End Class

    Private Shared ReadOnly td As List(Of type_disposer) = New List(Of type_disposer)()

    ' The order is important.
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")>
    Shared Sub New()
        register_base_type(Sub(ByVal s As Stream)
                               If s IsNot Nothing Then
                                   s.Flush()
                                   s.Close()
                                   s.Dispose()
                               End If
                           End Sub)
        register_base_type(Sub(ByVal s As WaitHandle)
                               If s IsNot Nothing Then
                                   s.Close()
                                   s.Dispose()
                               End If
                           End Sub)
        register_base_type(Sub(ByVal s As TcpClient)
                               If s IsNot Nothing Then
                                   s.Close()
                               End If
                           End Sub)
        register_base_type(Sub(ByVal s As UdpClient)
                               If s IsNot Nothing Then
                                   s.Close()
                               End If
                           End Sub)
        register_base_type(Sub(ByVal s As Socket)
                               If s IsNot Nothing Then
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
    Private Shared ReadOnly [default] As Action(Of T) = create_default()

    Private Shared Function create_default() As Action(Of T)
        If type_info(Of T).is_object Then
            raise_error(error_type.performance, "disposable(Of Object) is definitely a performance destroyer.")
            Return Sub(ByVal i As T)
                       If i IsNot Nothing Then
                           Dim a As Action(Of T) = Nothing
                           If disposable.find(i.GetType(), a) Then
                               assert(a IsNot Nothing)
                               a(i)
                           End If
                       End If
                   End Sub
        End If
        Return disposable.find(Of T)()
    End Function

    Public Shared Sub register(ByVal d As Action(Of T))
        global_resolver(Of Action(Of T), disposable(Of T)).assert_first_register(d)
    End Sub

    Public Shared Sub unregister()
        global_resolver(Of Action(Of T), disposable(Of T)).assert_unregister()
    End Sub

    Public Shared Function D() As Action(Of T)
        Return global_resolver(Of Action(Of T), disposable(Of T)).resolve_or_default([default])
    End Function

    Public Shared Sub dispose(ByVal v As T)
        Dim a As Action(Of T) = Nothing
        a = D()
        If a IsNot Nothing Then
            a(v)
        End If
    End Sub

    Private Sub New()
    End Sub
End Class
