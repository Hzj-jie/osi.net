
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Private NotInheritable Class template_t
            Private NotInheritable Class definition
                Private ReadOnly name As String
                Private ReadOnly format As String
                Private ReadOnly param_count As UInt32

                Public Sub New(ByVal name As String, ByVal format As String, ByVal param_count As UInt32)
                    assert(Not name.null_or_whitespace())
                    assert(Not format.null_or_whitespace())
                    assert(param_count > 0)
                    Me.name = name
                    Me.format = format
                    Me.param_count = param_count
                End Sub

                Public Function apply(ByVal types() As String, ByRef o As String) As Boolean
                    If types.array_size() <> param_count Then
                        raise_error(error_type.user,
                                    "Template ",
                                    name,
                                    " expectes ",
                                    param_count,
                                    " template types, but received [",
                                    types,
                                    "].")
                        Return False
                    End If
                    o = String.Format(format, types)
                    Return True
                End Function
            End Class

            Private ReadOnly m As New unordered_map(Of String, definition)()

            Public Function define(ByVal name As String,
                                   ByVal format As String,
                                   ByVal param_count As UInt32) As Boolean
                assert(Not name.null_or_whitespace())
                assert(Not format.null_or_whitespace())
                assert(param_count > 0)
                If m.emplace(name, New definition(name, format, param_count)).second() Then
                    Return True
                End If
                raise_error(error_type.user, "Template ", name, " has been defined already.")
                Return False
            End Function

            Public Function resolve(ByVal name As String, ByVal types() As String, ByRef o As String) As Boolean
                assert(Not name.null_or_whitespace())
                assert(Not types.null_or_empty())
                ' TODO: Should resolve type-aliases.
                Dim it As unordered_map(Of String, definition).iterator = m.find(name)
                If it = m.end() Then
                    Return False
                End If
                Return (+it).second.apply(types, o)
            End Function
        End Class

        Public Structure template_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal name As String,
                                   ByVal format As String,
                                   ByVal param_count As UInt32) As Boolean
                Return s.t.define(name, format, param_count)
            End Function

            Public Function resolve(ByVal name As String, ByVal types() As String, ByRef o As String) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.t.resolve(name, types, o) Then
                        Return True
                    End If
                    s = s.parent
                End While
                raise_error(error_type.user, "Template ", name, " has not been defined.")
                Return False
            End Function
        End Structure

        Public Function template() As template_proxy
            Return New template_proxy(Me)
        End Function
    End Class
End Class
