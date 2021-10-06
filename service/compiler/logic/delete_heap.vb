
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Delete an array with @name
    Public NotInheritable Class delete_heap
        Implements exportable

        Private ReadOnly name As String

        Public Sub New(ByVal name As String)
            assert(Not name.null_or_whitespace())
            Me.name = name
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim v As variable = Nothing
            If Not variable.of_stack(scope, name, v) Then
                Return False
            End If
            o.emplace_back(instruction_builder.str(command.dealloc, v))
            Return True
        End Function
    End Class
End Namespace
