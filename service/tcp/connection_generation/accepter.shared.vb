
Imports System.Net
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector

Partial Public Class accepter
    Private Shared Function create_listener(ByVal add As IPAddress, ByVal port As Int32) As TcpListener
        Try
            Return New TcpListener(add, port)
        Catch ex As Exception
            raise_error(error_type.exclamation, "failed to create listener on ", add, " with port ", port)
            Return Nothing
        End Try
    End Function

    Private Shared Function start(ByVal l As TcpListener) As Boolean
        If l Is Nothing Then
            Return False
        Else
            Try
                l.Start()
                Return l.active()
            Catch ex As Exception
                raise_error(error_type.exclamation,
                            "failed to start listener on ",
                            l.LocalEndpoint(),
                            ", ex ",
                            ex.Message())
                Return False
            End Try
        End If
    End Function

    Private Shared Sub [stop](ByVal l As TcpListener)
        If Not l Is Nothing Then
            l.Stop()
        End If
    End Sub
End Class
