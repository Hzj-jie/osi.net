
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Define a variable with @name.
    Public Class define
        Implements exportable

        Private ReadOnly name As String
        Private ReadOnly type As String

        Public Sub New(ByVal name As String, ByVal type As String)
            assert(Not String.IsNullOrEmpty(name))
            assert(Not String.IsNullOrEmpty(type))
            Me.name = name
            Me.type = type
        End Sub

        Public Shared Function export(ByVal name As String,
                                      ByVal type As String,
                                      ByVal scope As scope,
                                      ByVal o As vector(Of String)) As Boolean
            Dim d As define = Nothing
            d = New define(name, type)
            Return d.export(scope, o)
        End Function

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            If scope_define(scope, name, type) Then
                o.emplace_back(command_str(command.push))
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
