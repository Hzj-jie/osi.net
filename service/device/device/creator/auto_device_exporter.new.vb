
Public NotInheritable Class auto_device_exporter
    Private Sub New()
    End Sub

    Public Shared Function [New](Of T)(ByVal c As idevice_creator(Of T),
                                       Optional ByVal check_interval_ms As Int64 =
                                           constants.default_auto_generation_check_interval_ms,
                                       Optional ByVal failure_wait_ms As Int64 =
                                           constants.default_auto_generation_failure_wait_ms,
                                       Optional ByVal max_concurrent_generations As Int32 =
                                           constants.default_auto_generation_max_concurrent_generations) _
                                      As auto_device_exporter(Of T)
        Return auto_device_exporter(Of T).[New](c, check_interval_ms, failure_wait_ms, max_concurrent_generations)
    End Function

    Public Shared Function [New](Of T)(ByVal id As String,
                                       ByVal c As idevice_creator(Of T),
                                       Optional ByVal check_interval_ms As Int64 =
                                           constants.default_auto_generation_check_interval_ms,
                                       Optional ByVal failure_wait_ms As Int64 =
                                           constants.default_auto_generation_failure_wait_ms,
                                       Optional ByVal max_concurrent_generations As Int32 =
                                           constants.default_auto_generation_max_concurrent_generations) _
                                      As auto_device_exporter(Of T)
        Return auto_device_exporter(Of T).[New](id, c, check_interval_ms, failure_wait_ms, max_concurrent_generations)
    End Function

    Public Shared Function [New](Of T)(ByVal id As String,
                                       ByVal c As iasync_device_creator(Of T),
                                       Optional ByVal check_interval_ms As Int64 =
                                           constants.default_auto_generation_check_interval_ms,
                                       Optional ByVal failure_wait_ms As Int64 =
                                           constants.default_auto_generation_failure_wait_ms,
                                       Optional ByVal max_concurrent_generations As Int32 =
                                           constants.default_auto_generation_max_concurrent_generations) _
                                      As auto_device_exporter(Of T)
        Return auto_device_exporter(Of T).[New](id, c, check_interval_ms, failure_wait_ms, max_concurrent_generations)
    End Function

    Public Shared Function [New](Of T)(ByVal c As iasync_device_creator(Of T),
                                       Optional ByVal check_interval_ms As Int64 =
                                           constants.default_auto_generation_check_interval_ms,
                                       Optional ByVal failure_wait_ms As Int64 =
                                           constants.default_auto_generation_failure_wait_ms,
                                       Optional ByVal max_concurrent_generations As Int32 =
                                           constants.default_auto_generation_max_concurrent_generations) _
                                      As auto_device_exporter(Of T)
        Return auto_device_exporter(Of T).[New](c, check_interval_ms, failure_wait_ms, max_concurrent_generations)
    End Function
End Class

Partial Public MustInherit Class auto_device_exporter(Of T)
    Public Shared Function [New](ByVal id As String,
                                 ByVal c As idevice_creator(Of T),
                                 Optional ByVal check_interval_ms As Int64 =
                                     constants.default_auto_generation_check_interval_ms,
                                 Optional ByVal failure_wait_ms As Int64 =
                                     constants.default_auto_generation_failure_wait_ms,
                                 Optional ByVal max_concurrent_generations As Int32 =
                                     constants.default_auto_generation_max_concurrent_generations) _
                                As auto_device_exporter(Of T)
        Return for_idevice_creator.[New](id, c, check_interval_ms, failure_wait_ms, max_concurrent_generations)
    End Function

    Public Shared Function [New](ByVal c As idevice_creator(Of T),
                                 Optional ByVal check_interval_ms As Int64 =
                                     constants.default_auto_generation_check_interval_ms,
                                 Optional ByVal failure_wait_ms As Int64 =
                                     constants.default_auto_generation_failure_wait_ms,
                                 Optional ByVal max_concurrent_generations As Int32 =
                                     constants.default_auto_generation_max_concurrent_generations) _
                                As auto_device_exporter(Of T)
        Return [New](Nothing, c, check_interval_ms, failure_wait_ms, max_concurrent_generations)
    End Function

    Public Shared Function [New](ByVal id As String,
                                 ByVal c As iasync_device_creator(Of T),
                                 Optional ByVal check_interval_ms As Int64 =
                                     constants.default_auto_generation_check_interval_ms,
                                 Optional ByVal failure_wait_ms As Int64 =
                                     constants.default_auto_generation_failure_wait_ms,
                                 Optional ByVal max_concurrent_generations As Int32 =
                                     constants.default_auto_generation_max_concurrent_generations) _
                                As auto_device_exporter(Of T)
        Return for_iasync_device_creator.[New](id, c, check_interval_ms, failure_wait_ms, max_concurrent_generations)
    End Function

    Public Shared Function [New](ByVal c As iasync_device_creator(Of T),
                                 Optional ByVal check_interval_ms As Int64 =
                                     constants.default_auto_generation_check_interval_ms,
                                 Optional ByVal failure_wait_ms As Int64 =
                                     constants.default_auto_generation_failure_wait_ms,
                                 Optional ByVal max_concurrent_generations As Int32 =
                                     constants.default_auto_generation_max_concurrent_generations) _
                                As auto_device_exporter(Of T)
        Return [New](Nothing, c, check_interval_ms, failure_wait_ms, max_concurrent_generations)
    End Function
End Class
