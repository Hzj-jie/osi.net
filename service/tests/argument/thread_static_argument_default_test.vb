
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class thread_static_argument_default_test
    Private Shared arg As argument(Of Boolean)

    <test>
    Private Shared Sub run()
        If Not expectation.is_false(-arg) Then
            Return
        End If
        assertion.is_false(arg Or thread_static_argument_default(Of thread_static_argument_default_test).of(False))
        Using thread_static_argument_default(Of Boolean, thread_static_argument_default_test).scoped_register(True)
            assertion.is_true(arg Or thread_static_argument_default(Of thread_static_argument_default_test).of(False))
        End Using
    End Sub

    Private Sub New()
    End Sub
End Class
