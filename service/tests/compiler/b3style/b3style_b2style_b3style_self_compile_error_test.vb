
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_b2style_b3style_self_compile_error_test
    Inherits b2style_self_compile_error_test_runner

    Protected Overrides Function parse(ByVal content As String, ByRef o As executor) As Boolean
<<<<<<< HEAD
        Return New b2style.parse_wrapper_b3style(interrupts.default).compile(content, o)
=======
        Return New b2style.compile_wrapper_b3style(interrupts.default).compile(content, o)
>>>>>>> master
    End Function

    Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
        Return b2style.compile_wrapper_b3style.with_current_file(filename)
    End Function
End Class
