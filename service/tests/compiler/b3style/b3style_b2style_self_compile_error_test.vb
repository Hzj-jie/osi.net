﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_b2style_self_compile_error_test
    Inherits b2style_self_compile_error_test_runner

    Protected Overrides Function parse(ByVal content As String, ByRef o As executor) As Boolean
        Return New b3style.parse_wrapper(interrupts.default).compile(content, o)
    End Function

    Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
        Return b2style.compile_wrapper_b3style.with_current_file(filename)
    End Function

    ' TODO: Should work once the template is fully functional in b3style.
    Protected Overrides Function ignore_case(ByVal name As String) As Boolean
        Return name.Equals("struct-with-myself.txt")
    End Function
End Class
