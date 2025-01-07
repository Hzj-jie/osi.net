
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<command_line_specified>
<test>
Public NotInheritable Class b3style_b2style_self_test
    Inherits b2style_self_test_runner

    Protected Overrides Function parse(ByVal functions As interrupts,
                                       ByVal content As String,
                                       ByRef e As executor) As Boolean
        Return New b3style.parse_wrapper(functions).compile(content, e)
    End Function

    Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
        Return b3style.parse_wrapper.with_current_file(filename)
    End Function

    Protected Overrides Function ignore_case(ByVal name As String) As Boolean
        Return unordered_set.of("static_cast_ptr_type2.txt",
                                "static_cast_ptr_type3.txt",
                                "static_cast_ptr_type_bool.txt",
                                "static_cast_ptr_type.txt").find(name).is_not_end()
    End Function
End Class
