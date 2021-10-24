﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Private NotInheritable Class variable_t
            Private ReadOnly s As New unordered_map(Of String, String)()

            Public Function define(ByVal type As String, ByVal name As String) As Boolean
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                ' Types are always resolved during the define / build stage, so scope.current() equals to the scope
                ' where the variable_t instance Is being defined.
                type = scope.current().type_alias()(type)
                If s.emplace(name, type).second() Then
                    Return True
                End If
                raise_error(error_type.user,
                            "Variable ",
                            name,
                            " (new type ",
                            type,
                            ") has been defined already as ",
                            s(name))
                Return False
            End Function

            Public Function resolve(ByVal name As String, ByRef type As String) As Boolean
                Return s.find(name, type)
            End Function
        End Class

        Public NotInheritable Class variable_proxy
            Public ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal type As String, ByVal name As String) As Boolean
                Return s.v.define(type, name)
            End Function

            Public Function define(ByVal vs As vector(Of builders.parameter)) As Boolean
                assert(Not vs Is Nothing)
                Return vs.stream().
                          map(Function(ByVal p As builders.parameter) As Boolean
                                  assert(Not p Is Nothing)
                                  Return define(p.type, p.name)
                              End Function).
                          aggregate(bool_stream.aggregators.all_true, True)  ' If vs is empty, always return true.
            End Function

            Public Function try_resolve(ByVal name As String, ByRef type As String) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.v.resolve(name, type) Then
                        Return True
                    End If
                    s = s.parent
                End While
                Return False
            End Function

            Public Function resolve(ByVal name As String, ByRef type As String) As Boolean
                If try_resolve(name, type) Then
                    Return True
                End If
                raise_error(error_type.user, "Variable ", name, " has not been defined.")
                Return False
            End Function
        End Class

        Public Function variables() As variable_proxy
            Return New variable_proxy(Me)
        End Function
    End Class
End Class