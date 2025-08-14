
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_runnable_test
    Inherits compiler_self_test_runner

    Public Sub New()
        MyBase.New(b3style_runnable_test_cases.data)
    End Sub

    Protected Overrides Sub execute(ByVal name As String, ByVal content As String)
        Dim e As executor = Nothing
        assertion.is_true(b3style.with_functions(New interrupts(console_io.null())).compile(content, e), name)
        assertion.is_not_null(e, name)
        e.assert_execute_without_errors(name)
    End Sub

    Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
        Return b2style.compile_wrapper.with_current_file(filename)
    End Function
End Class
