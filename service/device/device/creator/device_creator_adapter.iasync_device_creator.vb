
Imports osi.root.connector
Imports osi.service.selector

Partial Public Class device_creator_adapter(Of IT, OT)
    Private Class for_iasync_device_creator
        Implements idevice_creator(Of OT)

        Private ReadOnly i As iasync_device_creator(Of IT)
        Private ReadOnly c As Func(Of IT, OT)
        Private ReadOnly c2 As Func(Of async_getter(Of OT), OT)

        Public Sub New(ByVal i As iasync_device_creator(Of IT),
                       ByVal c As Func(Of IT, OT),
                       ByVal c2 As Func(Of async_getter(Of OT), OT))
            assert(i IsNot Nothing)
            assert(c IsNot Nothing)
            Me.i = i
            Me.c = c
            Me.c2 = c2
        End Sub

        Public Function create(ByRef o As idevice(Of OT)) As Boolean Implements idevice_creator(Of OT).create
            Dim it As idevice(Of async_getter(Of IT)) = Nothing
            If i.create(it) Then
                Return async_device_device_converter.adapt(
                           device_adapter.[New](Of async_getter(Of IT), async_getter(Of OT)) _
                                               (it, Function(i As async_getter(Of IT)) As async_getter(Of OT)
                                                        Return async_getter_adapter.[New](i, c)
                                                    End Function),
                           o,
                           c2)
            Else
                Return False
            End If
        End Function
    End Class
End Class
