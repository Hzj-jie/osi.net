
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public NotInheritable Class request_path
    Inherits context_filter

    Private ReadOnly path As String
    Private ReadOnly exact_match As Boolean
    Private ReadOnly prefix As Boolean

    Public Sub New(ByVal path As String,
                   Optional ByVal exact_match As Boolean = False,
                   Optional ByVal prefix As Boolean = False)
        Me.path = path
        Me.exact_match = exact_match
        Me.prefix = prefix
    End Sub

    Public Overrides Function [select](ByVal context As server.context) As Boolean
        If context Is Nothing Then
            Return False
        End If

        If exact_match Then
            If prefix Then
                Return strsame(context.context.Request().Url().AbsolutePath(), path, strlen(path), False)
            Else
                Return strsame(context.context.Request().Url().AbsolutePath(), path, False)
            End If
        Else
            Dim parsed_path As vector(Of String) = Nothing
            Dim request_path As vector(Of String) = Nothing
            If Not url_path.parse(path, parsed_path, context.encoder) OrElse
               Not url_path.parse(context.context.Request(), request_path, context.encoder) Then
                Return False
            End If

            If prefix Then
                If parsed_path.empty() Then
                    Return True
                ElseIf parsed_path.size() > request_path.size() Then
                    Return False
                Else
                    For i As UInt32 = 0 To parsed_path.size() - uint32_1
                        If Not strsame(parsed_path(i), request_path(i), False) Then
                            Return False
                        End If
                    Next
                    Return True
                End If
            Else
                If parsed_path.size() <> request_path.size() Then
                    Return False
                ElseIf parsed_path.empty() Then
                    Return True
                Else
                    For i As UInt32 = 0 To parsed_path.size() - uint32_1
                        If Not strsame(parsed_path(i), request_path(i), False) Then
                            Return False
                        End If
                    Next
                    Return True
                End If
            End If
        End If
    End Function
End Class
