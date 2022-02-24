
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports counter = osi.root.utils.counter

' A replacement of http_listener_context_handler to forward server.context_received event to multiple "module"
' instances. Each module can decide to handle the context_received event or not according to its own configuration or
' implementation.
Partial Public NotInheritable Class module_handle
    Private ReadOnly v As vector(Of ref)
    Private ReadOnly handle As server.context_receivedEventHandler

    Public Sub New()
        v = New vector(Of ref)()
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

    Public Function module_count() As UInt32
        Return v.size()
    End Function

    Public Function module_counter_snapshot(ByVal i As UInt32) As counter.snapshot
        If i >= v.size() Then
            Return Nothing
        Else
            Dim r As counter.snapshot = Nothing
            r = counter.snapshot.[New](v(i).counter_index)
            assert(Not r Is Nothing)
            Return r
        End If
    End Function

    Private Sub context_received(ByVal ctx As server.context)
        Dim i As UInt32 = 0
        While i < v.size()
            If v(i).module.context_received(ctx) Then
                assert(counter.increase(v(i).counter_index))
                Return
            End If
            i += uint32_1
        End While

        ctx.not_implemented()
    End Sub
End Class
