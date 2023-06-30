
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    ' Move a variable from @source to @target.
    Private NotInheritable Class _move
        Inherits copy_move

        Public Sub New(ByVal target As String, ByVal source As String)
            MyBase.New(target, source, command.mov)
        End Sub

        Public Overloads Shared Function export(ByVal target As variable,
                                                ByVal source As variable,
                                                ByVal o As vector(Of String)) As Boolean
            Return copy_move.export(command.mov, target, source, o)
        End Function
    End Class
End Class
