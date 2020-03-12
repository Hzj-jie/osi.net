
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

Friend Module _delegate2_test
    Friend Sub action()
    End Sub
End Module

<test>
Public NotInheritable Class delegate2_test
    <test>
    Private Shared Sub module_method_identity()
        Dim v As Action = Nothing
        v = AddressOf _delegate2_test.action
        assertion.equal(v.method_identity(), "osi.tests.root.connector._delegate2_test:action")

        Dim o As invoker(Of Action) = Nothing
        assertion.is_true(typeless_invoker.of(Of Action)().
                              for_static_methods().
                              with_fully_qualifed_name(v.method_identity()).
                              build(o))
    End Sub

    Private Sub New()
    End Sub
End Class
