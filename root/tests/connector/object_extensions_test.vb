
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class object_extensions_test
    Inherits [case]

    Private Shared Function boolean_cast_case() As Boolean
        assertion.is_true(True.is_null_or_true())
        assertion.is_true(DirectCast(Nothing, String).is_null_or_true())
        assertion.is_true(_object_extensions.is_null_or_true(DirectCast(Nothing, Object)))
        assertion.is_false(False.is_null_or_true())
        assertion.is_false("abc".is_null_or_true())
        assertion.is_false(100.is_null_or_true())

        assertion.is_true(True.is_not_null_and_true())
        assertion.is_false(DirectCast(Nothing, String).is_not_null_and_true())
        assertion.is_false(_object_extensions.is_not_null_and_true(DirectCast(Nothing, Object)))
        assertion.is_false(False.is_not_null_and_true())
        assertion.is_false("abc".is_not_null_and_true())
        assertion.is_false(100.is_not_null_and_true())

        Return True
    End Function

    Private Shared Function is_null_case() As Boolean
        assertion.is_false(_object_extensions.is_null(Of Int32)(Nothing))
        assertion.is_true(_object_extensions.is_null(Of String)(Nothing))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return boolean_cast_case() AndAlso
               is_null_case()
    End Function
End Class
