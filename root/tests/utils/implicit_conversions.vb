
Option Explicit On
Option Infer Off
Option Strict Off

Imports osi.root.connector
Imports osi.root.utils

' This file contains expected implicit conversions.
Public NotInheritable Class implicit_conversions
    Public Shared Function valuer_test_get_only_case_try_get(Of T As New, T2 As {T, New}) _
                                                            (ByVal i As valuer(Of T2), ByRef o As T) As Boolean
        assert(Not i Is Nothing)
        Return i.try_get(o)
    End Function

    Private Sub New()
    End Sub
End Class
