
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class copy_heap_in
        Inherits copy_heap

        Public Sub New(ByVal types As types,
                       ByVal target As String,
                       ByVal target_index As String,
                       ByVal source As String)
            MyBase.New(types,
                       target,
                       target_index,
                       source,
                       Function(ByVal stack_var As variable, ByVal heap_var As variable) As Boolean
                           Return heap_var.is_assignable_from(stack_var)
                       End Function,
                       Function(ByVal array_ptr As String, ByVal stack_ptr As String, ByRef o As String) As Boolean
                           assert(Not array_ptr.null_or_whitespace())
                           assert(Not stack_ptr.null_or_whitespace())
                           o = instruction_builder.str(command.hcpin, array_ptr, stack_ptr)
                           Return True
                       End Function)
        End Sub
    End Class
End Namespace
