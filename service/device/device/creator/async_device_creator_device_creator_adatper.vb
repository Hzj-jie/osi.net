
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.selector

Public Class async_device_creator_device_creator_adapter
    Public Shared Function [New](Of T)(ByVal i As iasync_device_creator(Of T)) _
                                      As async_device_creator_device_creator_adapter(Of T)
        Return New async_device_creator_device_creator_adapter(Of T)(i)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class async_device_creator_device_creator_adapter(Of T)
    Implements idevice_creator(Of T)

    Private ReadOnly ac As iasync_device_creator(Of T)
    Private ReadOnly adapter As async_device_device_converter(Of T)

    Public Sub New(ByVal ac As iasync_device_creator(Of T))
        assert(Not ac Is Nothing)
        Me.ac = ac
        Me.adapter = New async_device_device_converter(Of T)(AddressOf async_getter_wrapper)
    End Sub

    Protected Overridable Function async_getter_wrapper(ByVal i As async_getter(Of T), ByRef o As T) As Boolean
        assert(False)
        Return False
    End Function

    ' Creates a device and waits for its initialization.
    Public Function create(ByVal p As pointer(Of idevice(Of T))) As event_comb
        Dim ad As idevice(Of async_getter(Of T)) = Nothing
        Return New event_comb(Function() As Boolean
                                  If ac.create(ad) AndAlso Not ad Is Nothing Then
                                      Dim ag As async_getter(Of T) = Nothing
                                      ag = ad.get()
                                      If Not ag Is Nothing Then
                                          Return waitfor(ag.get(DirectCast(Nothing, pointer(Of T)))) AndAlso
                                                 goto_next()
                                      Else
                                          Return False
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert(Not ad Is Nothing)
                                  Dim o As idevice(Of T) = Nothing
                                  Return adapter.adapt(ad, o) AndAlso
                                         eva(p, o) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function create(ByRef o As idevice(Of T)) As Boolean Implements idevice_creator(Of T).create
        Dim ad As idevice(Of async_getter(Of T)) = Nothing
        Return ac.create(ad) AndAlso adapter.adapt(ad, o)
    End Function
End Class
