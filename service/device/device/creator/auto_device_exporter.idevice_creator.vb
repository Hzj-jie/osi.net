
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Partial Public MustInherit Class auto_device_exporter(Of T)
    Private NotInheritable Class for_idevice_creator
        Inherits auto_device_exporter(Of T)

        Private ReadOnly c As idevice_creator(Of T)

        Public Shared Shadows Function [New](ByVal id As String,
                                             ByVal c As idevice_creator(Of T),
                                             ByVal check_interval_ms As Int64,
                                             ByVal failure_wait_ms As Int64,
                                             ByVal max_concurrent_generations As Int32) As auto_device_exporter(Of T)
            Return New for_idevice_creator(id, c, check_interval_ms, failure_wait_ms, max_concurrent_generations)
        End Function

        Private Sub New(ByVal id As String,
                       ByVal c As idevice_creator(Of T),
                       ByVal check_interval_ms As Int64,
                       ByVal failure_wait_ms As Int64,
                       ByVal max_concurrent_generations As Int32)
            MyBase.New(id, check_interval_ms, failure_wait_ms, max_concurrent_generations)
            assert(c IsNot Nothing)
            Me.c = c
        End Sub

        Protected Overrides Function create_device(ByVal p As ref(Of idevice(Of T))) As event_comb
            Return sync_async(Function() As Boolean
                                  Dim o As idevice(Of T) = Nothing
                                  Return c.create(o) AndAlso
                                         eva(p, o)
                              End Function)
        End Function
    End Class
End Class
