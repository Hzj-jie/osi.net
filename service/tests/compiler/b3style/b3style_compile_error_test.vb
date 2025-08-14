
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive
Imports osi.service.resource

<test>
Public NotInheritable Class b3style_compile_error_test
    Private Shared Sub run(ByVal d() As Byte, ByVal ParamArray errs() As String)
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(New b3style.parse_wrapper(interrupts.default).compile(d.as_text(), Nothing))
            End Sub)).contains(errs)
    End Sub

    <test>
    Private Shared Sub define_class_constructor_for_non_class()
    End Sub
End Class
