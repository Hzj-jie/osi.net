
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.service.dataprovider

Public Class config_object(Of T)
    Inherits combine_dataprovider(Of T)

    Friend Sub New(ByVal dps() As dataprovider(Of config), ByVal d As Func(Of T))
        MyBase.New(New config_object_loader(Of T)(d, dps), dps)
    End Sub

    Friend Sub New(ByVal dps() As dataprovider(Of config), ByVal l As config_object_loader(Of T))
        MyBase.New(l, dps)
    End Sub
End Class

Public Class config_object_loader(Of T)
    Inherits dataprovider_dataloader(Of T)

    Private ReadOnly d As Func(Of T)

    Public Sub New(ByVal d As Func(Of T), ByVal dps() As dataprovider(Of config))
        MyBase.New(dps) 'do not use it, but still waste time / memory to handle the array
        Me.d = d
    End Sub

    Protected NotOverridable Overrides Function load(ByVal dps() As idataprovider) As T
        Return load()
    End Function

    Protected Overridable Overloads Function load() As T
        assert(d IsNot Nothing)
        Return d()
    End Function
End Class
