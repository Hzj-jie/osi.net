
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.selector

Partial Public MustInherit Class auto_device_exporter(Of T)
    Private NotInheritable Class for_iasync_device_creator
        Inherits auto_device_exporter(Of T)

        Public Shared Shadows Function [New](ByVal id As String,
                                             ByVal c As iasync_device_creator(Of T),
                                             ByVal check_interval_ms As Int64,
                                             ByVal failure_wait_ms As Int64,
                                             ByVal max_concurrent_generations As Int32,
                                             Optional ByVal adapter As Func(Of async_getter(Of T), T) = Nothing) As auto_device_exporter(Of T)
            Return New for_iasync_device_creator(
                       id,
                       c,
                       check_interval_ms,
                       failure_wait_ms,
                       max_concurrent_generations,
                       adapter)
        End Function

        Private ReadOnly c As iasync_device_creator(Of T)
        Private ReadOnly converter As async_device_device_converter(Of T)

        Private Sub New(ByVal id As String,
                        ByVal c As iasync_device_creator(Of T),
                        ByVal check_interval_ms As Int64,
                        ByVal failure_wait_ms As Int64,
                        ByVal max_concurrent_generations As Int32,
                        Optional ByVal adapter As Func(Of async_getter(Of T), T) = Nothing)
            MyBase.New(id, check_interval_ms, failure_wait_ms, max_concurrent_generations)
            assert(Not c Is Nothing)
            Me.c = c
            Me.converter = async_device_device_converter.[New](adapter)
        End Sub

        ' Create the device and wait for its initialization.
        Protected Overrides Function create_device(ByVal p As ref(Of idevice(Of T))) As event_comb
            Dim ad As idevice(Of async_getter(Of T)) = Nothing
            Return New event_comb(Function() As Boolean
                                      If c.create(ad) AndAlso Not ad Is Nothing Then
                                          Dim ag As async_getter(Of T) = Nothing
                                          ag = ad.get()
                                          If Not ag Is Nothing Then
                                              Return waitfor(ag.get(DirectCast(Nothing, ref(Of T)))) AndAlso
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
                                      Return converter.adapt(ad, o) AndAlso
                                             eva(p, o) AndAlso
                                             goto_end()
                                  End Function)
        End Function
    End Class
End Class
