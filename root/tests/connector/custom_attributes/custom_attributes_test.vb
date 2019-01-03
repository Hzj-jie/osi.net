
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class custom_attributes_test
    Private Interface test_i
    End Interface

    Private Class test1
        Inherits Attribute
        Implements test_i
    End Class

    Private Class test2
        Inherits test1
    End Class

    Private Class test3
        Inherits Attribute
        Implements test_i
    End Class

    <test1>
    <test2>
    <test3>
    Private Class target
    End Class

    <test>
    Private Shared Sub inherited_attribute_classes_should_be_retrieved()
        Dim o() As test1 = Nothing
        assertion.is_true(GetType(target).custom_attributes(o))
        assertion.equal(array_size_i(o), 2)

        assertion.is_true(direct_cast(o(1), [default](Of test2).null))
    End Sub

    <test>
    Private Shared Sub attributes_should_be_able_to_implement_interfaces()
        Dim o() As test_i = Nothing
        assertion.is_true(GetType(target).custom_attributes(o))
        assertion.equal(array_size_i(o), 3)

        assertion.is_true(direct_cast(o(0), [default](Of test1).null))
        assertion.is_true(direct_cast(o(1), [default](Of test2).null))
        assertion.is_true(direct_cast(o(2), [default](Of test3).null))
    End Sub

    Private Sub New()
    End Sub
End Class
