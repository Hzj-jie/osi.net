
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public Class scope(Of T As scope(Of T))
    Public NotInheritable Class function_t
        Private ReadOnly s As New unordered_map(Of String, String)()

        Public Function define(ByVal type As String, ByVal name As String) As Boolean
            assert(Not type.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            type = current_accessor().type_alias(type)
            If s.emplace(name, type).second() Then
                Return True
            End If
            raise_error(error_type.user, "Function ", name, " has been defined already with return type ", type)
            Return False
        End Function

        Public Function is_defined(ByVal name As String) As Boolean
            Return s.find(name, Nothing)
        End Function

        Public Function return_type_of(ByVal name As String, ByRef type As String) As Boolean
            If s.find(name, type) Then
                Return True
            End If
            raise_error(error_type.user, "Function ", name, " has not been defined.")
            Return False
        End Function
    End Class
End Class
