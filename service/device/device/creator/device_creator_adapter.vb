
Imports osi.root.connector
Imports osi.service.selector

Public NotInheritable Class device_creator_adapter
    Public Shared Function [New](Of IT, OT)(ByVal i As idevice_creator(Of IT),
                                            ByVal c As Func(Of IT, OT)) As device_creator_adapter(Of IT, OT)
        Return New device_creator_adapter(Of IT, OT)(i, c)
    End Function

    Public Shared Function [New](Of IT, OT)(ByVal i As iasync_device_creator(Of IT),
                                            ByVal c As Func(Of IT, OT),
                                            Optional ByVal c2 As Func(Of async_getter(Of OT), OT) = Nothing) _
                                           As device_creator_adapter(Of IT, OT)
        Return New device_creator_adapter(Of IT, OT)(i, c, c2)
    End Function

    Private Sub New()
    End Sub
End Class

Partial Public Class device_creator_adapter(Of IT, OT)
    Implements idevice_creator(Of OT)

    Private ReadOnly impl As idevice_creator(Of OT)

    Public Sub New(ByVal i As idevice_creator(Of IT), ByVal c As Func(Of IT, OT))
        impl = New for_idevice_creator(i, c)
    End Sub

    Public Sub New(ByVal i As iasync_device_creator(Of IT),
                   ByVal c As Func(Of IT, OT),
                   Optional ByVal c2 As Func(Of async_getter(Of OT), OT) = Nothing)
        impl = New for_iasync_device_creator(i, c, c2)
    End Sub

    Public Function create(ByRef o As idevice(Of OT)) As Boolean Implements idevice_creator(Of OT).create
        Return impl.create(o)
    End Function
End Class
