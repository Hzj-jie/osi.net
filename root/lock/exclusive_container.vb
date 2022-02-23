
Imports System.Threading
Imports osi.root.connector

Public Class exclusive_container(Of T As Class)
    Private v As T

    Public Function has_value() As Boolean
        Return v IsNot Nothing
    End Function

    Public Function [get]() As T
        assert(has_value())
        Return v
    End Function

    Public Function clear() As Boolean
        Return atomic.clear_if_not_nothing(v)
    End Function

    Public Function create(ByVal ctor As Func(Of T), Optional ByVal destroy As Action(Of T) = Nothing) As Boolean
        Return atomic.create_if_nothing(v, ctor, destroy)
    End Function

    Public Function create(Of N As {T, Class, New})(Optional ByVal destroy As Action(Of N) = Nothing) As Boolean
        'Both following patterns trigger an N to T conversion, and break atomic
        'Return atomic.create_if_nothing(Of N)(v, destroy)
        'Return atomic.create_if_nothing(v, Function() New N(), destroy)
        Return create(Function() New N(),
                      Sub(x As T)
                          If destroy IsNot Nothing Then
                              destroy(cast(Of N)(x))
                          End If
                      End Sub)
    End Function
End Class
