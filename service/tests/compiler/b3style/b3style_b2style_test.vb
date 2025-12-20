
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
Public NotInheritable Class b3style_b2style_test
    Inherits b2style_test_runner_b3style_supported

    Protected Overrides Function parse(ByVal io As console_io.test_wrapper,
                                       ByVal content As String,
                                       ByRef o As executor) As Boolean
        Return New b3style.parse_wrapper(New interrupts(+io)).compile(content, o)
    End Function

    ' This test triggers a compiler error on b3style.
    <test>
    Private Sub reinterpret_cast_to_a_different_class_type()
        assertions.of(error_event.capture_log(error_type.user,
            Sub()
                assertion.is_false(parse(New console_io.test_wrapper(),
                                         _b2style_test_data.reinterpret_cast_to_a_different_class_type.as_text(),
                                         Nothing))
            End Sub)).contains("s.::S2__struct__type__id")
    End Sub

    <test>
    Private Sub __func__()
        Dim io As New console_io.test_wrapper()
        Dim e As executor = Nothing
        assertion.is_true(parse(io, _b2style_test_data.__func__.as_text(), e))
        assertion.is_not_null(e)
        e.assert_execute_without_errors()
        assertion.equal(io.output().Trim(), String.Join(character.newline,
            "::type0 main([])",
            "::type0 ::std_out:::C__struct__type__id__type([this.::C__struct__type__id: ::C__struct__type__id__type&])",
            "::type0 ::N::print([])",
            "::type0 ::N::f2:::Integer:::String([x: ::Integer, s: ::String])"))
    End Sub

    Private Sub New()
    End Sub
End Class

<command_line_specified>
<test>
Public NotInheritable Class b3style_b2style_test_all
    Inherits b2style_test_runner

    Protected Overrides Function parse(ByVal io As console_io.test_wrapper,
                                       ByVal content As String,
                                       ByRef o As executor) As Boolean
        Return New b3style.parse_wrapper(New interrupts(+io)).compile(content, o)
    End Function

    Private Sub New()
    End Sub
End Class
