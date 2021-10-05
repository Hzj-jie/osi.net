
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class copy_array_in
        Inherits copy_array

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal target As String,
                       ByVal target_index As String,
                       ByVal source As String)
            MyBase.New(anchors,
                       types,
                       target,
                       target_index,
                       source,
                       Function(ByVal array_ptr As String, ByVal stack_ptr As String, ByRef o As String) As Boolean
                           assert(Not array_ptr.null_or_whitespace())
                           assert(Not stack_ptr.null_or_whitespace())
                           o = instruction_builder.str(command.hcpin, array_ptr, stack_ptr)
                           Return True
                       End Function)
        End Sub
    End Class
End Namespace
