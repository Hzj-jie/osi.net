
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt

Public NotInheritable Class optional_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim p As [optional](Of Object) = Nothing
        p = [optional].[New](New Object())
        assertion.is_true(p)
        assertion.is_not_null(+p)
        assertion.is_not_null(-p)

        p = [optional].empty(Of Object)()
        assertion.is_false(p)
        assertion.is_null(-p)

        Return True
    End Function
End Class
