
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class container_operator(Of CONTAINER, T)
    Public Interface enumerator
        Function [end]() As Boolean
        Function current() As T
        Sub [next]()
    End Interface
End Class
