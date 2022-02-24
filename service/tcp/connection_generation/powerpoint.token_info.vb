
Imports System.Net.Sockets
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.envs
Imports osi.root.template
Imports osi.service.commander

Partial Public Class powerpoint
    Public NotInheritable Class token_info
        Inherits token_info(Of powerpoint, TcpClient)

        Public Shared ReadOnly instance As token_info

        Shared Sub New()
            instance = New token_info()
        End Sub

        Private Sub New()
            MyBase.New()
        End Sub

        Public Overrides Function identity(ByVal p As powerpoint) As String
            assert(Not p Is Nothing)
            Return p.identity
        End Function

        Public Overrides Function identity(ByVal c As TcpClient) As String
            assert(Not c Is Nothing)
            Return c.identity()
        End Function

        Public Overrides Sub shutdown(ByVal c As TcpClient)
            assert(Not c Is Nothing)
            c.shutdown()
        End Sub

        Protected Overrides Function token_str(ByVal p As powerpoint) As String
            assert(Not p Is Nothing)
            Return p.token
        End Function

        Public Overrides Function trace() As Boolean
            Return tcp_trace
        End Function

        Protected Overrides Function create_herald(ByVal p As powerpoint, ByVal c As TcpClient) As herald
            assert(Not p Is Nothing)
            Return p.as_herald(c)
        End Function

        Public Overrides Function response_timeout_ms(ByVal p As powerpoint) As Int64
            assert(Not p Is Nothing)
            Return p.response_timeout_ms
        End Function
    End Class

    Public NotInheritable Class challenger
        Public Shared Function [New](ByVal p As powerpoint, ByVal c As TcpClient) _
                                    As itoken_challenger
            assert(Not p Is Nothing)
            If strsame(p.tokener, constants.bypass_tokener) Then
                Return bypass_token_challenger.[New](token_info.instance, p, c)
            ElseIf strsame(p.tokener, constants.token1) OrElse String.IsNullOrEmpty(p.token) Then
                Return token_challenger.[New](token_info.instance, p, c)
            Else
                Return token2_challenger.[New](token_info.instance, p, c)
            End If
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class defender
        Public Shared Function [New](ByVal p As powerpoint) As itoken_defender(Of powerpoint, TcpClient)
            assert(Not p Is Nothing)
            If strsame(p.tokener, constants.bypass_tokener) Then
                Return bypass_token_defender.[New](token_info.instance)
            ElseIf strsame(p.tokener, constants.token1) OrElse String.IsNullOrEmpty(p.token) Then
                Return token_defender.[New](token_info.instance)
            Else
                Return token2_defender.[New](token_info.instance)
            End If
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
