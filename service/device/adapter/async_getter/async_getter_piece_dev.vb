
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.selector

<type_attribute()>
Public Class async_getter_piece_dev
    Inherits async_getter_dev_T(Of piece, piece_dev)
    Implements piece_dev

    Private Sub New(ByVal p As pair(Of async_getter(Of piece_dev), type_attribute))
        MyBase.New(p)
    End Sub

    Public Shared Shadows Function create(Of T, PT As piece_dev) _
                                         (ByVal i As async_getter(Of T),
                                          ByVal c As Func(Of T, PT)) As async_getter_piece_dev
        Return New async_getter_piece_dev(async_getter_adapter(Of piece_dev).convert(i, c))
    End Function

    Public Shared Shadows Function create(Of T)(ByVal i As async_getter(Of T),
                                                ByVal c As Func(Of T, piece_dev)) As async_getter_piece_dev
        Return New async_getter_piece_dev(async_getter_adapter(Of piece_dev).convert(i, c))
    End Function

    Public Shared Shadows Function create(ByVal i As async_getter(Of piece_dev)) As async_getter_piece_dev
        Return New async_getter_piece_dev(async_getter_adapter(Of piece_dev).convert(i))
    End Function
End Class
