
Imports osi.root.connector

Partial Public Class type_attribute
    Public Sub forward_from(ByVal i As type_attribute)
        [set](New forward_signal(i))
    End Sub

    Public Sub forward_to(ByVal i As type_attribute)
        assert(Not i Is Nothing)
        i.forward_from(Me)
    End Sub

    Public Sub forward_from(Of T)()
        forward_from([of](Of T)())
    End Sub

    Public Sub forward_to(Of T)()
        forward_to([of](Of T)())
    End Sub

    Public Sub forward_from(Of T As signal)(ByVal i As T)
        forward_from([of](i))
    End Sub

    Public Sub forward_to(Of T As signal)(ByVal i As T)
        forward_to([of](i))
    End Sub

    Public Sub forward_from(ByVal i As signal)
        forward_from([of](i))
    End Sub

    Public Sub forward_to(ByVal i As signal)
        forward_to([of](i))
    End Sub

    Public Sub forward_from(ByVal t As Type)
        forward_from([of](t))
    End Sub

    Public Sub forward_to(ByVal t As Type)
        forward_to([of](t))
    End Sub

    Public Sub forward_from(ByVal i As Object)
        forward_from([of](i))
    End Sub

    Public Sub forward_to(ByVal i As Object)
        forward_to([of](i))
    End Sub
End Class
