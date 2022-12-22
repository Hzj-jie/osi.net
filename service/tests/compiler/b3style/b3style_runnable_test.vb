
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.delegates
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_runnable_test
    Inherits compiler_self_test_runner

    Private Shared filter As argument(Of String)

    Public Sub New()
        MyBase.New(filter Or "*", b3style_runnable_test_cases.data)
    End Sub

    <test>
    Private Shadows Sub run()
        MyBase.run()
    End Sub

    Protected Overrides Sub execute(ByVal name As String, ByVal content As String)
        Dim e As executor = Nothing
        assertion.is_true(b2style.with_functions(New interrupts(New console_io())).parse(content, e), name)
        assertion.is_not_null(e, name)
        e.assert_execute_without_errors(name)
    End Sub
End Class
