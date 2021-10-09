﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Copy a variable from @source to @target.
    Public NotInheritable Class copy
        Inherits copy_move

        Public Sub New(ByVal types As types, ByVal target As String, ByVal source As String)
            MyBase.New(types, target, source, command.cp)
        End Sub

        Public Overloads Shared Function export(ByVal target As variable,
                                                ByVal source As variable,
                                                ByVal o As vector(Of String)) As Boolean
            Return copy_move.export(command.cp, target, source, o)
        End Function
    End Class
End Namespace
