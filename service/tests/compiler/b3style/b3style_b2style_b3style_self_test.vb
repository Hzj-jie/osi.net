
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt.attributes
Imports osi.service.compiler
Imports osi.service.interpreter.primitive

<test>
Public NotInheritable Class b3style_b2style_b3style_self_test
    Inherits b2style_self_test_runner

    Protected Overrides Function parse(ByVal functions As interrupts,
                                       ByVal content As String,
                                       ByRef e As executor) As Boolean
        Return New b2style.compile_wrapper_b3style(functions).compile(content, e)
    End Function

    Protected Overrides Function with_current_file(ByVal filename As String) As IDisposable
        Return b2style.compile_wrapper_b3style.with_current_file(filename)
    End Function

    Protected Overrides Function ignore_case(ByVal name As String) As Boolean
        assert(Not name.null_or_whitespace())
        If name.Equals("struct-and-primitive-type-with-same-name.txt") Then
            ' Unsupported scenario
            Return True
        End If
        If name.Equals("ptr_offset.txt") Then
            ' b3style uses ::ptr_offset as function name, and the predefine will be ignored by b2style
            Return True
        End If
        Return False
    End Function
End Class
