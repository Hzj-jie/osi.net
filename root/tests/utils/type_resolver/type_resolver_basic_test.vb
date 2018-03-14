
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_resolver_basic_test
    <test>
    Private Shared Sub register_and_resolve()
        Dim r As type_resolver(Of Int32) = Nothing
        r = New type_resolver(Of Int32)()
        r.assert_first_register(GetType(Int32), 1)
        r.assert_first_register(GetType(Boolean), 2)
        r.assert_first_register(GetType(String), 3)
        r.register(GetType(Int32), 4)

        Dim o As Int32 = 0
        assert_true(r.from_type(GetType(Int32), o))
        assert_equal(o, 4)
        assert_true(r.from_type(GetType(Boolean), o))
        assert_equal(o, 2)
        assert_true(r.from_type(GetType(String), o))
        assert_equal(o, 3)
    End Sub

    Private Sub New()
    End Sub
End Class
