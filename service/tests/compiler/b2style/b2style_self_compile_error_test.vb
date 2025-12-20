
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt.attributes
Imports osi.service.compiler

Public MustInherit Class b2style_self_compile_error_test_runner
    Inherits compiler_self_compile_error_test_runner

    Public Sub New()
        MyBase.New(b2style_self_compile_error_cases.data)
    End Sub
End Class

<test>
Public NotInheritable Class b2style_self_compile_error_test
    Inherits b2style_self_compile_error_test_runner

    Protected Overrides Function parse(ByVal content As String) As Boolean
        Return b2style.with_default_functions().compile(content, Nothing)
    End Function

    Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
        Return b2style.compile_wrapper.with_current_file(filename)
    End Function
End Class
