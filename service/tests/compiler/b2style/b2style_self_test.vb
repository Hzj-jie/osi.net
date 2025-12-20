
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

Public MustInherit Class b2style_self_test_runner
    Inherits compiler_self_assertion_test_runner

    Public Sub New()
        MyBase.New(b2style_self_test_cases.data)
    End Sub

    Protected Overrides Function ignore_case(ByVal name As String) As Boolean
        Return unordered_set.of(
            "delegate_in_class.txt",
            "delegate_in_class_on_heap.txt").find(name).is_not_end()
    End Function
End Class

<test>
Public NotInheritable Class b2style_self_test
    Inherits b2style_self_test_runner

    Protected Overrides Function parse(ByVal functions As interrupts,
                                       ByVal content As String,
                                       ByRef e As executor) As Boolean
        Return b2style.with_functions(functions).compile(content, e)
    End Function

    Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
        Return b2style.compile_wrapper.with_current_file(filename)
    End Function

    Protected Overrides Function ignore_case(ByVal name As String) As Boolean
        Return MyBase.ignore_case(name) OrElse is_ignored_case(name)
    End Function

    Public Shared Function is_ignored_case(ByVal name As String) As Boolean
        assert(Not name.null_or_whitespace())
        Return unordered_set.emplace_of(
            "struct-and-primitive-type-with-same-name.txt").find(name).is_not_end()
    End Function
End Class
