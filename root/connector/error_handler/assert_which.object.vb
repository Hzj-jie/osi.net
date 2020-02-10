
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class assert_which
    Public Structure object_assertion
        Private ReadOnly obj As Object

        Public Sub New(ByVal obj As Object)
            Me.obj = obj
        End Sub

        Public Function is_not_null() As Object
            assert(Not obj Is Nothing)
            Return obj
        End Function
    End Structure
End Class
