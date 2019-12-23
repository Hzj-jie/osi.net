
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

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
            m.emplace(command_clear_define, Function(ByVal i As String) As Boolean
                                                Return clear_define()
                                            End Function)
            m.emplace(command_define, AddressOf define)
            m.emplace(command_mode, AddressOf mode)
            m.emplace(command_clear_word, AddressOf clear_word)
        End Sub

        Protected Overrides Function command_mapping() As map(Of String, Func(Of String, Boolean))
            Return m
        End Function

        Protected Overrides Function [default](ByVal s As String, ByVal f As String) As Boolean
            If s.null_or_whitespace() OrElse f.null_or_whitespace() Then
                raise_error(error_type.user, "word ", f, " has empty definition")
                Return False
            End If
            words.emplace_back(emplace_make_pair(f, s))
            Return True
        End Function

        Private Function clear_define() As Boolean
            macros.clear()
            Return True
        End Function

        Private Function define(ByVal s As String) As Boolean
            Dim k As String = Nothing
            Dim v As String = Nothing
            If Not strsep(s, k, v, define_separator) Then
                Return False
            End If
            macros.emplace_back(emplace_make_pair(k, v))
            Return True
        End Function

        Private Function mode(ByVal s As String) As Boolean
            Dim k As String = Nothing
            Dim v As String = Nothing
            If Not strsep(s, k, v, mode_separator) Then
                Return False
            End If
            If Not enum_def(Of match_choice).has(v) Then
                Return False
            End If
            If strsame(k, mode_type_choice) Then
                type_choice = v
                Return True
            End If
            If strsame(k, mode_word_choice) Then
                word_choice = v
                Return True
            End If
            Return False
        End Function

        Private Function clear_word(ByVal s As String) As Boolean
            words.clear()
            Return True
        End Function
    End Class
End Class
