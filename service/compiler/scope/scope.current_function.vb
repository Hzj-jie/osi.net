
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.compiler.logic

Partial Public Class scope(Of T As scope(Of T))
    Public NotInheritable Class current_function_t
        Inherits function_signature(Of builders.parameter)

        Public Sub New(ByVal name As String,
                       ByVal return_type As String,
                       ByVal params As vector(Of builders.parameter))
            MyBase.New(name, scope(Of T).current().type_alias(return_type), params)
        End Sub

        Public Function allow_return_value() As Boolean
            Return Not return_type.Equals(compiler.logic.scope.type_t.zero_type)
        End Function
    End Class

    Public Structure current_function_proxy
        Public Sub define(ByVal name As String,
                          ByVal return_type As String,
                          ByVal params As vector(Of builders.parameter))
            assert(scope(Of T).current().myself().current_function() Is Nothing)
            scope(Of T).current().myself().current_function(New current_function_t(name, return_type, params))
        End Sub

        Private Function current_function() As current_function_t
            Dim s As scope(Of T) = scope(Of T).current()
            While s.myself().current_function() Is Nothing
                s = s.parent
                assert(Not s Is Nothing)
            End While
            Return s.myself().current_function()
        End Function

        Private Function current_function_opt() As [optional](Of current_function_t)
            Dim s As scope(Of T) = scope(Of T).current()
            While s.myself().current_function() Is Nothing
                s = s.parent
                If s Is Nothing Then
                    Return [optional].empty(Of current_function_t)()
                End If
            End While
            Return [optional].of(s.myself().current_function())
        End Function

        Public Function allow_return_value() As Boolean
            Return current_function().allow_return_value()
        End Function

        Public Function name() As String
            Return current_function().name
        End Function

        Public Function name_opt() As [optional](Of String)
            Return current_function_opt().map(Function(ByVal x As current_function_t) As String
                                                  assert(Not x Is Nothing)
                                                  Return x.name
                                              End Function)
        End Function

        Public Function return_struct() As Boolean
            Return scope(Of T).current().structs().types().defined(return_type())
        End Function

        Public Function return_type() As String
            assert(allow_return_value())
            Return current_function().return_type
        End Function

        Public Function signature() As String
            Return current_function_opt().map(Function(ByVal x As current_function_t) As String
                                                  Return x.ToString()
                                              End Function).
                                              or_else("unknown_function")
        End Function
    End Structure
End Class
