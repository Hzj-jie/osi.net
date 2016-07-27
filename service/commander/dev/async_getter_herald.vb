
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.device
Imports osi.service.selector

<global_init(global_init_level.services)>
<type_attribute()>
Public Class async_getter_herald
    Inherits async_getter_dev_T(Of command, herald)
    Implements herald

    Shared Sub New()
        assert(async_device_device_converter.register(Function(i As async_getter(Of herald)) As herald
                                                          Return create(i)
                                                      End Function))
    End Sub

    Private Sub New(ByVal p As pair(Of async_getter(Of herald), type_attribute))
        MyBase.New(p)
    End Sub

    Public Shared Shadows Function create(Of T)(ByVal i As async_getter(Of T),
                                                ByVal c As Func(Of T, herald)) As async_getter_herald
        Return New async_getter_herald(async_getter_adapter(Of herald).convert(i, c))
    End Function

    Public Shared Shadows Function create(ByVal i As async_getter(Of herald)) As async_getter_herald
        Return New async_getter_herald(async_getter_adapter(Of herald).convert(i))
    End Function

    Private Shared Sub init()
    End Sub
End Class
