
Imports osi.root.connector

Partial Public Class type_attribute
    ' Users can write 'type_attribute.of(Of ...)().copy_from(Of ...)().get(Of ...)(). ...'
    Public Function copy_from(ByVal i As type_attribute) As type_attribute
        [set](copy(i.get()))
        Return Me
    End Function

    Public Function copy_to(ByVal i As type_attribute) As type_attribute
        assert(Not i Is Nothing)
        Return i.copy_from(Me)
    End Function

    Public Function copy_from(Of T)() As type_attribute
        Return copy_from([of](Of T)())
    End Function

    Public Function copy_to(Of T)() As type_attribute
        Return copy_to([of](Of T)())
    End Function

    Public Function copy_from(Of T As signal)(ByVal i As T) As type_attribute
        Return copy_from([of](i))
    End Function

    Public Function copy_to(Of T As signal)(ByVal i As T) As type_attribute
        Return copy_to([of](i))
    End Function

    Public Function copy_from(ByVal i As signal) As type_attribute
        Return copy_from([of](i))
    End Function

    Public Function copy_to(ByVal i As signal) As type_attribute
        Return copy_to([of](i))
    End Function

    Public Function copy_from(ByVal t As Type) As type_attribute
        Return copy_from([of](t))
    End Function

    Public Function copy_to(ByVal t As Type) As type_attribute
        Return copy_to([of](t))
    End Function

    Public Function copy_from(ByVal i As Object) As type_attribute
        Return copy_from([of](i))
    End Function

    Public Function copy_to(ByVal i As Object) As type_attribute
        Return copy_to([of](i))
    End Function
End Class
