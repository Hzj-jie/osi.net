
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class object_extensions_test
    Inherits [case]

    Private Shared Function boolean_cast_case() As Boolean
        assert_true(True.is_null_or_true())
        assert_true(DirectCast(Nothing, String).is_null_or_true())
        assert_true(_object_extensions.is_null_or_true(DirectCast(Nothing, Object)))
        assert_false(False.is_null_or_true())
        assert_false("abc".is_null_or_true())
        assert_false(100.is_null_or_true())

        assert_true(True.is_not_null_and_true())
        assert_false(DirectCast(Nothing, String).is_not_null_and_true())
        assert_false(_object_extensions.is_not_null_and_true(DirectCast(Nothing, Object)))
        assert_false(False.is_not_null_and_true())
        assert_false("abc".is_not_null_and_true())
        assert_false(100.is_not_null_and_true())

        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return boolean_cast_case()
    End Function
End Class
