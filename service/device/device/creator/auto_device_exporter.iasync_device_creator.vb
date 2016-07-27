
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Partial Public MustInherit Class auto_device_exporter(Of T)
    Private NotInheritable Class for_iasync_device_creator
        Public Shared Shadows Function [New](ByVal id As String,
                                             ByVal c As iasync_device_creator(Of T),
                                             ByVal check_interval_ms As Int64,
                                             ByVal failure_wait_ms As Int64,
                                             ByVal max_concurrent_generations As Int32) As auto_device_exporter(Of T)
            Return for_async_device_creator_device_creator_adapter.[New](
                       id,
                       async_device_creator_device_creator_adapter.[New](c),
                       check_interval_ms,
                       failure_wait_ms,
                       max_concurrent_generations)
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
