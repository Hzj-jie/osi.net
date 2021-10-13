
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive
Imports osi.service.resource

<test>
Public NotInheritable Class struct_test
    <test>
    Private Shared Sub single_level_struct()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(bstyle.with_functions(New interrupts(+io)).
                                 parse(_bstyle_test_data.single_level_struct.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), "dabc")
    End Sub

    <test>
    Private Shared Sub nested_struct()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(bstyle.with_functions(New interrupts(+io)).
                                 parse(_bstyle_test_data.nested_struct.as_text(), e))
        assertion.is_not_null(e)
        e.execute()
        assertion.equal(io.output(), "dd")
    End Sub

    Private Sub New()
    End Sub
End Class
