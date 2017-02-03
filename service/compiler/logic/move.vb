
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.interpreter.primitive

Namespace logic
    ' Move a variable from @source to @target.
    Public Class move
        Inherits copy_move

        Public Sub New(ByVal types As types, ByVal target As String, ByVal source As String)
            MyBase.New(types, target, source)
        End Sub

        Protected Overrides Function instruction() As command
            Return command.mov
        End Function
    End Class
End Namespace
