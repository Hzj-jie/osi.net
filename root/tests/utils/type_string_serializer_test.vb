
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_string_serializer_test
    <test>
    Private Shared Sub base_test()
        assert_equal(type_string_serializer.to_str_or_null(GetType(UInt32), CUInt(100)), "100")
        assert_equal(type_string_serializer.from_str_or_null(GetType(UInt32), "100"), CUInt(100))
        assert_equal(type_string_serializer.to_str_or_null(GetType(Int32), 100), "100")
        assert_equal(type_string_serializer.from_str_or_null(GetType(Int32), "100"), 100)
        assert_equal(type_string_serializer.to_str_or_null(GetType(Int32), -100), "-100")
        assert_equal(type_string_serializer.from_str_or_null(GetType(Int32), "-100"), -100)

        assert_equal(type_string_serializer.to_str_or_null(GetType(Boolean), True), "True")
        assert_equal(type_string_serializer.to_str_or_null(GetType(Boolean), False), "False")
        assert_equal(type_string_serializer.from_str_or_null(GetType(Boolean), "True"), True)
        assert_equal(type_string_serializer.from_str_or_null(GetType(Boolean), "False"), False)
    End Sub

    <test>
    Private Shared Sub unimplemented_test()
        Dim implemented As Boolean = False
        implemented = True
        assert_false(type_string_serializer.from_str(GetType(test_interface), implemented, "abc", Nothing))
        assert_false(implemented)
    End Sub

    Private Interface test_interface
    End Interface

    Private Sub New()
    End Sub
End Class
