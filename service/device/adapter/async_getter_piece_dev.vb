
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.selector
Imports osi.service.transmitter

' Consumers should use async_device_creator_device_creator_adatper, instead of using this class directly.
<type_attribute()>
Public Class async_getter_piece_dev
    Inherits async_getter_dev_T(Of piece, piece_dev)
    Implements piece_dev

    Private Sub New(ByVal p As pair(Of async_getter(Of piece_dev), type_attribute))
        MyBase.New(p)
    End Sub

    Public Shared Shadows Function [New](Of T, PT As piece_dev) _
                                        (ByVal i As async_getter(Of T),
                                         ByVal c As Func(Of T, PT)) As async_getter_piece_dev
        Return New async_getter_piece_dev(async_getter_adapter(Of piece_dev).convert(i, c))
    End Function

    Public Shared Shadows Function [New](Of T)(ByVal i As async_getter(Of T),
                                               ByVal c As Func(Of T, piece_dev)) As async_getter_piece_dev
        Return New async_getter_piece_dev(async_getter_adapter(Of piece_dev).convert(i, c))
    End Function

    Public Shared Shadows Function [New](ByVal i As async_getter(Of piece_dev)) As async_getter_piece_dev
        Return New async_getter_piece_dev(async_getter_adapter(Of piece_dev).convert(i))
    End Function
End Class
