
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.service.argument

Public Class wrappered_device_creator(Of T)
    Implements idevice_creator(Of T)

    Private ReadOnly c As idevice_creator(Of T)
    Private ReadOnly v As var

    Public Sub New(ByVal c As idevice_creator(Of T), ByVal v As var)
        assert(Not c Is Nothing)
        assert(Not v Is Nothing)
        Me.c = c
        Me.v = v
    End Sub

    Public Function create(ByRef o As idevice(Of T)) As Boolean Implements idevice_creator(Of T).create
        Dim i As idevice(Of T) = Nothing
        Return c.create(i) AndAlso device_adapter.wrap(v, i, o)
    End Function
End Class
