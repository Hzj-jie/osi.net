﻿
Imports osi.root.connector
Imports osi.root.utt

Public Class enum_test
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
        assert_true(enum_traversal(Of test_enum)(Sub(x As test_enum, y As String)
                                                     assert_true(test_enum_str(cast(Of Int32)(x)) = y)
                                                 End Sub,
                                                 Sub(c As Int32)
                                                     assert_true(c = test_enum.COUNT + 1)
                                                 End Sub))

        assert_false(enum_traversal(Of String)(Sub(x As String, y As String)
                                                   assert_true(False)
                                               End Sub))

        assert_false(enum_traversal(Of test_enum)(CType(Nothing, Action(Of test_enum, String))))

        Dim i As Int32 = 0
        Const target As test_enum = test_enum.d
        assert_true(enum_traversal(Of test_enum)(Function(x As test_enum, y As String) As Boolean
                                                     i += 1
                                                     Return x <> target
                                                 End Function))
        assert_equal(i, target + 1)

        Return True
    End Function

    Private Shared Function has_case() As Boolean
        For i As Int32 = test_enum.a To test_enum.COUNT
            assert_true(enum_has(Of test_enum, Int32)(i))
        Next
        For i As Int32 = -100 To test_enum.a - 1
            assert_false(enum_has(Of test_enum, Int32)(i))
        Next
        For i As Int32 = test_enum.COUNT + 1 To 100
            assert_false(enum_has(Of test_enum, Int32)(i))
        Next

        Return True
    End Function

    Private Shared Function cast_case() As Boolean
        Dim i As Int32 = 0
        i = test_enum.a
        assert_true(enum_traversal(Of test_enum)(Sub(x As test_enum, y As String)
                                                     Dim o As test_enum = Nothing
                                                     assert_true(enum_cast(i, o))
                                                     assert_equal(o, x)
                                                     assert_true(enum_cast(y, o))
                                                     assert_equal(o, x)
                                                     i += 1
                                                 End Sub))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return traversal_case() AndAlso
               has_case() AndAlso
               cast_case()
    End Function
End Class
