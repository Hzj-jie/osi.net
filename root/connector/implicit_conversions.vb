
Option Explicit On
Option Infer Off
Option Strict Off

' TODO: Remove, should use binary_operations.
Public NotInheritable Class implicit_conversions
    Public Shared Function object_add(ByVal i As Object, ByVal j As Object) As Object
        Return i + j
    End Function

    Private Sub New()
    End Sub
End Class
