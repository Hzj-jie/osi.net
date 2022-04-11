
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class thread_static_argument_default_test
    Private Shared arg As argument(Of Boolean)
    Private Shared arg2 As argument(Of Int32)

    <test>
    Private Shared Sub current_thread()
        If Not expectation.is_false(-arg) Then
            Return
        End If
        assertion.is_false(arg Or thread_static_argument_default(Of thread_static_argument_default_test).of(False))
        Using thread_static_argument_default(Of Boolean, thread_static_argument_default_test).scoped_register(True)
            assertion.is_true(arg Or thread_static_argument_default(Of thread_static_argument_default_test).of(False))
        End Using
    End Sub

    <test>
    Private Shared Sub multiple_threads()
        If Not expectation.is_false(-arg2) Then
            Return
        End If
        Dim m As New AutoResetEvent(False)
        Using thread_static_argument_default(Of Int32, thread_static_argument_default_test).scoped_register(1)
            assertion.equal(arg2 Or thread_static_argument_default(Of thread_static_argument_default_test).of(2), 1)
            Dim t As New Thread(
                Sub()
                    assertion.equal(arg2 Or
                                    thread_static_argument_default(Of thread_static_argument_default_test).of(2),
                                    2)
                    assertion.is_true(m.force_set())
                End Sub)
            t.Start()
            assertion.is_true(m.wait())
        End Using
    End Sub

    Private Sub New()
    End Sub
End Class
