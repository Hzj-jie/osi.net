
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Private NotInheritable Class current_function_t
            Private ReadOnly raw_name As String
            Public ReadOnly return_type As String
            Private ReadOnly params As vector(Of builders.parameter)
            Public ReadOnly name As lazier(Of String)

            Public Sub New(ByVal raw_name As String,
                           ByVal return_type As String,
                           ByVal params As vector(Of builders.parameter))
                assert(Not raw_name.null_or_whitespace())
                assert(Not return_type.null_or_whitespace())
                assert(Not params Is Nothing)
                Me.raw_name = raw_name
                Me.return_type = scope.current().type_alias()(return_type)
                Me.params = params
                Me.name = lazier.of(Function() As String
                                        Return logic_name.of_function(raw_name, params)
                                    End Function)
            End Sub

            Public Function allow_return_value() As Boolean
                Return Not return_type.Equals(types.zero_type)
            End Function
        End Class

        Public Structure current_function_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Sub define(ByVal raw_name As String,
                              ByVal return_type As String,
                              ByVal params As vector(Of builders.parameter))
                assert(s.cf Is Nothing)
                s.cf = New current_function_t(raw_name, return_type, params)
            End Sub

            Private Function current_function() As current_function_t
                Dim s As scope = Me.s
                While s.cf Is Nothing
                    s = s.parent
                    assert(Not s Is Nothing)
                End While
                Return s.cf
            End Function

            Public Function allow_return_value() As Boolean
                Return current_function().allow_return_value()
            End Function

            Public Function name() As String
                Return +current_function().name
            End Function

            Public Function return_struct() As Boolean
                Return s.structs().defined(return_type())
            End Function

            Public Function return_type() As String
                assert(allow_return_value())
                Return current_function().return_type
            End Function
        End Structure

        Public Function current_function() As current_function_proxy
            Return New current_function_proxy(Me)
        End Function
    End Class
End Class
