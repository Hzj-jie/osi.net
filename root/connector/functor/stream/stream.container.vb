
Option Explicit On
Option Infer Off
Option Strict On

Partial Public Class stream(Of T)
    Public NotInheritable Class container(Of CT)
        Inherits stream(Of T)

        Public Sub New(ByVal i As CT)
            Me.New(container_operator(Of CT, T).default, i)
        End Sub

        Public Sub New(ByVal o As container_operator(Of CT, T), ByVal i As CT)
            MyBase.New(assert_which.of(o).is_not_null().enumerate(i))
        End Sub
    End Class
End Class
