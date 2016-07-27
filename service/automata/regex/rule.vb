
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils
Imports configuration = osi.service.configuration

Partial Public Class rlexer
    Partial Public Class rule
        Inherits configuration.rule

        Private ReadOnly m As map(Of String, Func(Of String, Boolean))
        Private ReadOnly macros As vector(Of pair(Of String, String))
        Private ReadOnly words As vector(Of pair(Of String, String))
        Private type_choice As String
        Private word_choice As String

        Public Sub New()
            m = New map(Of String, Func(Of String, Boolean))()
            macros = rlexer.macros.default.export()
            words = New vector(Of pair(Of String, String))()
            type_choice = Nothing
            word_choice = Nothing
            m(command_clear_define) = AddressOf clear_define
            m(command_define) = AddressOf define
            m(command_mode) = AddressOf mode
            m(command_clear_word) = AddressOf clear_word
        End Sub

        Protected Overrides Function command_mapping() As map(Of String, Func(Of String, Boolean))
            Return m
        End Function

        Protected Overrides Function [default](ByVal s As String, ByVal f As String) As Boolean
            If s.null_or_whitespace() OrElse f.null_or_whitespace() Then
                raise_error(error_type.user, "word ", f, " has empty definition")
                Return False
            Else
                words.emplace_back(emplace_make_pair(f, s))
                Return True
            End If
        End Function

        Private Function clear_define() As Boolean
            macros.clear()
            Return True
        End Function

        Private Function define(ByVal s As String) As Boolean
            Dim k As String = Nothing
            Dim v As String = Nothing
            If strsep(s, k, v, define_separator) Then
                macros.emplace_back(emplace_make_pair(k, v))
                Return True
            Else
                Return False
            End If
        End Function

        Private Function mode(ByVal s As String) As Boolean
            Dim k As String = Nothing
            Dim v As String = Nothing
            If strsep(s, k, v, mode_separator) Then
                If enum_cast(Of match_choice)(v, Nothing) Then
                    If strsame(k, mode_type_choice) Then
                        type_choice = v
                        Return True
                    ElseIf strsame(k, mode_word_choice) Then
                        word_choice = v
                        Return True
                    End If
                End If
            End If
            Return False
        End Function

        Private Function clear_word(ByVal s As String) As Boolean
            words.clear()
            Return True
        End Function
    End Class
End Class
