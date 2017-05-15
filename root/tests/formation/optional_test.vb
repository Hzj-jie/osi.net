
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utt

Public Class optional_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim p As [optional](Of Object) = Nothing
        p = [optional].[New](New Object())
        assert_true(p)
        assert_not_nothing(+p)
        assert_not_nothing(-p)

        p = [optional].[New](Of Object)()
        assert_false(p)
        assert_nothing(-p)

        Return True
    End Function
End Class
