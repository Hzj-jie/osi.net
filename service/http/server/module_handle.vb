
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports counter = osi.root.utils.counter

Partial Public NotInheritable Class module_handle
    Private ReadOnly v As vector(Of [module])
    Private ReadOnly c As vector(Of Int64)
    Private ReadOnly handle As server.context_receivedEventHandler

    Public Sub New()
        v = New vector(Of [module])()
        c = New vector(Of Int64)()
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

    Public Function module_counter(ByVal i As UInt32) As Int64
        Return c(i)
    End Function

    Private Sub context_received(ByVal ctx As server.context)
        Dim i As UInt32 = 0
        While i < v.size()
            If v(i).context_received(ctx) Then
                assert(i < c.size())
                counter.increase(c(i))
                Return
            End If
            i += uint32_1
        End While

        ctx.not_implemented()
    End Sub
End Class
