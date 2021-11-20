
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class copy_heap_out
        Inherits copy_heap

        Public Sub New(ByVal types As types,
                       ByVal target As String,
                       ByVal source As String,
                       ByVal source_index As String)
            MyBase.New(types,
                       source,
                       source_index,
                       target,
                       Function(ByVal stack_var As variable, ByVal heap_var As variable) As Boolean
                           Return stack_var.is_assignable_from(heap_var)
                       End Function,
                       Function(ByVal array_ptr As String, ByVal stack_ptr As String, ByRef o As String) As Boolean
                           assert(Not array_ptr.null_or_whitespace())
                           assert(Not stack_ptr.null_or_whitespace())
                           o = instruction_builder.str(command.hcpout, stack_ptr, array_ptr)
                           Return True
                       End Function)
        End Sub
    End Class
End Namespace
