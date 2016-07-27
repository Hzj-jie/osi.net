
Imports osi.root.connector
Imports osi.root.utt

Public Module _value_types_cast
    Private Function cast_case(Of T, T2)(ByVal i As T) As Boolean
        Dim j As T2 = Nothing
        Dim k As T = Nothing
        assert_true(cast(Of T2)(i, j))
        'if T / T2 are cross casting
        If cast(Of T)(j, k) Then
            assert_equal(i, k)
        ElseIf comparable(i, j) Then
            assert_equal(compare(i, j), 0)
        Else
            'the casting is success, but the compare has trouble, such as int has only CompareTo(int)
        End If
        Return True
    End Function

    Private Function int32_cast() As Boolean
        Const i32 As Int32 = 100
        Return cast_case(Of Int32, Int32)(i32) AndAlso
               cast_case(Of Int32, Int64)(i32) AndAlso
               cast_case(Of Int32, String)(i32) AndAlso
               cast_case(Of Int32, Int16)(i32) AndAlso
               cast_case(Of Int32, SByte)(i32) AndAlso
               cast_case(Of Int32, UInt64)(i32) AndAlso
               cast_case(Of Int32, UInt32)(i32) AndAlso
               cast_case(Of Int32, UInt16)(i32) AndAlso
               cast_case(Of Int32, Byte)(i32)
    End Function

    Private Function int64_cast() As Boolean
        Const i64 As Int64 = 100
        Return cast_case(Of Int64, Int64)(i64) AndAlso
               cast_case(Of Int64, String)(i64) AndAlso
               cast_case(Of Int64, Int32)(i64) AndAlso
               cast_case(Of Int64, Int16)(i64) AndAlso
               cast_case(Of Int64, SByte)(i64) AndAlso
               cast_case(Of Int64, UInt64)(i64) AndAlso
               cast_case(Of Int64, UInt32)(i64) AndAlso
               cast_case(Of Int64, UInt16)(i64) AndAlso
               cast_case(Of Int64, Byte)(i64)
    End Function

    Private Function nullable_cast() As Boolean
        Const i32 As Int32 = 100
        Return cast_case(Of Int32?, Int32)(New Int32?(i32)) AndAlso
               cast_case(Of Int32?, Int64)(New Int32?(i32)) AndAlso
               cast_case(Of Int32?, String)(New Int32?(i32)) AndAlso
               cast_case(Of Int32?, Int16)(New Int32?(i32)) AndAlso
               cast_case(Of Int32?, SByte)(New Int32?(i32)) AndAlso
               cast_case(Of Int32?, UInt64)(New Int32?(i32)) AndAlso
               cast_case(Of Int32?, UInt32)(New Int32?(i32)) AndAlso
               cast_case(Of Int32?, UInt16)(New Int32?(i32)) AndAlso
               cast_case(Of Int32?, Byte)(New Int32?(i32))
    End Function

    Private Enum cast_enum As Int32
        a = 0
        b = 1
        c = 2
    End Enum

    Private Function enum_cast() As Boolean
        Return cast_case(Of Int32, cast_enum)(0) AndAlso
               cast_case(Of Int32, cast_enum)(1) AndAlso
               cast_case(Of Int32, cast_enum)(2) AndAlso
               cast_case(Of cast_enum, Int32)(cast_enum.a) AndAlso
               cast_case(Of cast_enum, Int32)(cast_enum.b) AndAlso
               cast_case(Of cast_enum, Int32)(cast_enum.c)
    End Function

    Public Function value_types() As Boolean
        Return int32_cast() AndAlso
               int64_cast() AndAlso
               nullable_cast() AndAlso
               enum_cast() AndAlso
               cast_test.failed_case(Of String, Int64)("abc")
    End Function
End Module
