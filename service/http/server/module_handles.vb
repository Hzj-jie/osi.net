
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument

Public NotInheritable Class module_handles
    Private ReadOnly v As vector(Of module_handle)
    Private ReadOnly handle As server.context_receivedEventHandler

    Public Sub New()
        v = New vector(Of module_handle)()
        handle = AddressOf context_received
    End Sub

    Public Sub New(ByVal server As server)
        Me.New()
        attach(server)
    End Sub

    Public Shared Function [New](ByVal server As server) As http_listener_context_handle
        Return New http_listener_context_handle(server)
    End Function

    Public Sub attach(ByVal server As server)
        assert(Not server Is Nothing)
        AddHandler server.context_received, handle
    End Sub

    Public Sub detach(ByVal server As server)
        assert(Not server Is Nothing)
        RemoveHandler server.context_received, handle
    End Sub

    Private Sub context_received(ByVal ctx As server.context)
        Dim i As UInt32 = 0
        While i < v.size()
            If v(i).context_received(ctx) Then
                Return
            End If
            i += uint32_1
        End While

        ctx.not_implemented()
    End Sub

    Public Function add(ByVal h As module_handle) As Boolean
        If h Is Nothing Then
            Return False
        Else
            v.emplace_back(h)
            Return True
        End If
    End Function

    Public Function add(ByVal invoker As invoker(Of _do_val_ref(Of server.context, event_comb, Boolean))) As Boolean
        Return add(module_handle.[New](invoker))
    End Function

    Public Function add(ByVal type As String,
                        ByVal assembly As String,
                        ByVal binding_flags As BindingFlags,
                        ByVal function_name As String) As Boolean
        Return add(module_handle.[New](type, assembly, binding_flags, function_name))
    End Function

    Public Function add(ByVal v As var) As Boolean
        Return add(module_handle.[New](v))
    End Function
End Class
