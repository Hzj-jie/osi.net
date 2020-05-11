
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_string_serializer_test
    <test>
    Private Shared Sub base_test()
        assertion.equal(type_string_serializer.r.to_str_or_null(GetType(UInt32), CUInt(100)), "100")
        assertion.equal(type_string_serializer.r.from_str_or_null(GetType(UInt32), "100"), CUInt(100))
        assertion.equal(type_string_serializer.r.to_str_or_null(GetType(Int32), 100), "100")
        assertion.equal(type_string_serializer.r.from_str_or_null(GetType(Int32), "100"), 100)
        assertion.equal(type_string_serializer.r.to_str_or_null(GetType(Int32), -100), "-100")
        assertion.equal(type_string_serializer.r.from_str_or_null(GetType(Int32), "-100"), -100)

        assertion.equal(type_string_serializer.r.to_str_or_null(GetType(Boolean), True), "True")
        assertion.equal(type_string_serializer.r.to_str_or_null(GetType(Boolean), False), "False")
        assertion.equal(type_string_serializer.r.from_str_or_null(GetType(Boolean), "True"), True)
        assertion.equal(type_string_serializer.r.from_str_or_null(GetType(Boolean), "False"), False)
    End Sub

    <test>
    Private Shared Sub unimplemented_test()
        Dim implemented As Boolean = False
        implemented = True
        assertion.is_false(type_string_serializer.r.from_str(GetType(test_interface), implemented, "abc", Nothing))
        assertion.is_false(implemented)
    End Sub

    Private Interface test_interface
    End Interface

    Private Sub New()
    End Sub
End Class
