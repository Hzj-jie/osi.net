﻿
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Partial Public MustInherit Class auto_device_exporter(Of T)
    Private NotInheritable Class for_async_device_creator_device_creator_adapter
        Inherits auto_device_exporter(Of T)

        Public Shared Shadows Function [New](ByVal id As String,
                                             ByVal c As async_device_creator_device_creator_adapter(Of T),
                                             ByVal check_interval_ms As Int64,
                                             ByVal failure_wait_ms As Int64,
                                             ByVal max_concurrent_generations As Int32) As auto_device_exporter(Of T)
            Return New for_async_device_creator_device_creator_adapter(id,
                                                                       c,
                                                                       check_interval_ms,
                                                                       failure_wait_ms,
                                                                       max_concurrent_generations)
        End Function

        Private ReadOnly c As async_device_creator_device_creator_adapter(Of T)

        Private Sub New(ByVal id As String,
                        ByVal c As async_device_creator_device_creator_adapter(Of T),
                        ByVal check_interval_ms As Int64,
                        ByVal failure_wait_ms As Int64,
                        ByVal max_concurrent_generations As Int32)
            MyBase.New(id, check_interval_ms, failure_wait_ms, max_concurrent_generations)
            assert(Not c Is Nothing)
            Me.c = c
        End Sub

        Protected Overrides Function create_device(ByVal p As pointer(Of idevice(Of T))) As event_comb
            Return c.create(p)
        End Function
    End Class
End Class
