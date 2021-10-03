
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace logic
    ' Move a variable from @source to @target.
    Public NotInheritable Class move
        Inherits copy_move

        Public Sub New(ByVal types As types, ByVal target As String, ByVal source As String)
            MyBase.New(types,
                       target,
                       source,
                       Function(ByVal x As variable, ByVal y As variable, ByRef o As String) As Boolean
                           assert(Not x Is Nothing)
                           Return x.move_from(y, o)
                       End Function)
        End Sub
    End Class
End Namespace
