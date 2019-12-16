﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class nlexer
    Public NotInheritable Class parser
        Inherits configuration.rule

        Private ReadOnly commands As map(Of String, Func(Of String, Boolean))
        Private ReadOnly rules As vector(Of pair(Of String, rule))

        Public Sub New()
            ' TODO: Implement macros
            commands = New map(Of String, Func(Of String, Boolean))()
            rules = New vector(Of pair(Of String, rule))()
        End Sub

        Protected Overrides Function command_mapping() As map(Of String, Func(Of String, Boolean))
            Return commands
        End Function

        Protected Overrides Function [default](ByVal s As String, ByVal f As String) As Boolean
            If s.null_or_whitespace() OrElse f.null_or_whitespace() Then
                raise_error(error_type.user, "word ", s, " and ", f, " pair has empty definition")
                Return False
            End If
            Dim r As rule = Nothing
            If Not rule.of(s, r) Then
                raise_error(error_type.user, "failed to parse rule " + s)
            End If
            rules.emplace_back(emplace_make_pair(f, r))
            Return True
        End Function
    End Class
End Class
