
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class container_operator(Of T)
    Public Interface enumerator
        Function [end]() As Boolean
        Function current() As T
        Sub [next]()
    End Interface

    Private Sub New()
    End Sub
End Class
