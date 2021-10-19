
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Class function_t
            Private ReadOnly s As New unordered_map(Of String, String)()

            Public Function define(ByVal type As String, ByVal name As String) As Boolean
                assert(Not type.null_or_whitespace())
                assert(Not name.null_or_whitespace())
                type = scope.current().type_alias()(type)
                If s.emplace(name, type).second() Then
                    Return True
                End If
                raise_error(error_type.user, "Function ", name, " has been defined already with return type ", type)
                Return False
            End Function

            Public Function return_type_of(ByVal name As String, ByRef type As String) As Boolean
                If s.find(name, type) Then
                    Return True
                End If
                raise_error(error_type.user, "Function ", name, " has not been defined.")
                Return False
            End Function
        End Class

        Public Function functions() As function_t
            If is_root() Then
                assert(Not f Is Nothing)
                Return f
            End If
            assert(f Is Nothing)
            Return (+root).f
        End Function
    End Class
End Class
