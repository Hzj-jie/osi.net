
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

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
        Dim r As Boolean = False
        ' Missing heap-struct-name - assertion failure
        r = r OrElse unordered_set.of("static_cast_ptr_type3.txt",
                                      "static_cast_ptr_type_bool.txt",
                                      "static_cast_ptr_type.txt").find(name).is_not_end()
        ' template class is not supported yet.
        r = r OrElse unordered_set.emplace_of("bool-heap.txt",
                                              "delegate-functions-with-same-name.txt",
                                              "delegate_in_class_on_heap.txt",
                                              "delegate_in_class.txt",
                                              "heap_ptr_test.txt",
                                              "heap.txt",
                                              "more-closing-angle-brackets.txt",
                                              "non-overridable-class-template-function.txt",
                                              "ptr_offset.txt",
                                              "ref_test.txt",
                                              "static_cast_ptr_type2.txt",
                                              "struct-and-primitive-type-with-same-name.txt",
                                              "template_before_type.txt",
                                              "template_class_with_ref.txt",
                                              "template-in-namespace.txt",
                                              "template-override.txt",
                                              "two-closing-angle-brackets.txt",
                                              "unmanaged_heap.txt",
                                              "vector-test.txt").find(name).is_not_end()
        ' expected compilation failure.
        r = r OrElse unordered_set.emplace_of("define-class-constructor-for-non-class.txt").find(name).is_not_end()
        Return r
    End Function
End Class
