
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace logic
    Public Module _scope_define
        Public Function scope_define(ByVal scope As scope, ByVal name As String, ByVal type As String) As Boolean
            assert(Not scope Is Nothing)
            If scope.define(name, type) Then
                Return True
            Else
                errors.redefine(name, type, scope.type(name))
                Return False
            End If
        End Function
    End Module
End Namespace
