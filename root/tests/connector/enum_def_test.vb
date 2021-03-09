
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class enum_def_test
    Inherits [case]

    Private Enum test_enum
        a
        b
        c
        d
        e
        f

        COUNT
    End Enum

    Private Shared ReadOnly test_enum_str() As String = {"a", "b", "c", "d", "e", "f", "COUNT"}

    Private Shared Function traversal_case() As Boolean
        assertion.equal(enum_def(Of test_enum).count_i(), test_enum.COUNT + 1)
        enum_def(Of test_enum).foreach(Sub(ByVal x As test_enum, ByVal y As String)
                                           assertion.equal(test_enum_str(cast(Of Int32).from(x)), y)
                                       End Sub)

        assertion.is_false(type_info(Of String).is_enum)

        expect_assertion_failure(Sub()
                                     enum_def(Of test_enum).
                                         foreach(direct_cast(Of Action(Of test_enum, String))(Nothing))
                                 End Sub,
                                 AddressOf assertion.not_reach)

        Dim i As Int32 = 0
        Const target As test_enum = test_enum.d
        enum_def(Of test_enum).foreach(Sub(ByVal x As test_enum, ByVal y As String)
                                           i += 1
                                           If x = target Then
                                               break_lambda.at_here()
                                           End If
                                       End Sub)
        assertion.equal(i, target + 1)

        Return True
    End Function

    Private Shared Function has_case() As Boolean
        For i As Int32 = test_enum.a To test_enum.COUNT
            assertion.is_true(enum_def(Of test_enum).has(i))
        Next
        For i As Int32 = -100 To test_enum.a - 1
            assertion.is_false(enum_def(Of test_enum).has(i))
        Next
        For i As Int32 = test_enum.COUNT + 1 To 100
            assertion.is_false(enum_def(Of test_enum).has(i))
        Next

        Return True
    End Function

    Private Shared Function cast_case() As Boolean
        Dim i As Int32 = test_enum.a
        enum_def(Of test_enum).foreach(Sub(ByVal x As test_enum, ByVal y As String)
                                           Dim o As test_enum = Nothing
                                           assertion.is_true(enum_def.from(i, o))
                                           assertion.equal(o, x)
                                           assertion.is_true(enum_def.from(y, o))
                                           assertion.equal(o, x)
                                           i += 1
                                       End Sub)
        Return True
    End Function

    Private Shared Function cast_case2() As Boolean
        enum_def(Of test_enum).foreach(
            Sub(ByVal x As test_enum, ByVal y As String)
                assertion.equal(enum_def(Of test_enum).from(enum_def(Of test_enum).to(Of SByte)(x)), x)
                assertion.equal(enum_def(Of test_enum).from(enum_def(Of test_enum).to(Of Byte)(x)), x)
                assertion.equal(enum_def(Of test_enum).from(enum_def(Of test_enum).to(Of Int16)(x)), x)
                assertion.equal(enum_def(Of test_enum).from(enum_def(Of test_enum).to(Of UInt16)(x)), x)
                assertion.equal(enum_def(Of test_enum).from(enum_def(Of test_enum).to(Of Int32)(x)), x)
                assertion.equal(enum_def(Of test_enum).from(enum_def(Of test_enum).to(Of UInt32)(x)), x)
                assertion.equal(enum_def(Of test_enum).from(enum_def(Of test_enum).to(Of Int64)(x)), x)
                assertion.equal(enum_def(Of test_enum).from(enum_def(Of test_enum).to(Of UInt64)(x)), x)
            End Sub)
        Return True
    End Function

    Private Shared Function to_string_case() As Boolean
        Dim o() As String = enum_def(Of test_enum).strings()
        assertion.is_not_null(o)
        assertion.array_equal(o, test_enum_str)
        Return True
    End Function

    Private Shared Function bytes_case() As Boolean
        enum_def(Of test_enum).foreach(Sub(ByVal i As test_enum)
                                           Dim b() As Byte = bytes_serializer.to_bytes(i)
                                           Dim y As test_enum = Nothing
                                           assertion.is_true(bytes_serializer.from_bytes(b, y))
                                           assertion.equal(i, y)
                                       End Sub)
        Return True
    End Function

    Private Shared Function str_case() As Boolean
        enum_def(Of test_enum).foreach(Sub(ByVal i As test_enum)
                                           Dim s As String = Nothing
                                           assertion.is_true(string_serializer.to_str(i, s))
                                           Dim y As test_enum = Nothing
                                           assertion.is_true(string_serializer.from_str(s, y))
                                           assertion.equal(i, y)
                                       End Sub)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return traversal_case() AndAlso
               has_case() AndAlso
               cast_case() AndAlso
               cast_case2() AndAlso
               bytes_case() AndAlso
               str_case() AndAlso
               to_string_case()
    End Function
End Class
