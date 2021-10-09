
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants.counter
Imports osi.root.envs

Namespace counter
    Friend Module _counter_internal
        Friend Class internal_counter
            Public name As String = Nothing
            Public value As Func(Of Int64) = Nothing

            Public Sub New(ByVal name As String, ByVal value As Func(Of Int64),
                           Optional ByVal addTimeRange As Boolean = True)
                If addTimeRange Then
                    Me.name = strcat(name, "/", write_frequence_minutes, "MIN")
                Else
                    Me.name = name
                End If
                Me.value = value
            End Sub
        End Class

        Private ReadOnly recent_processor_usage_record As recent_processor_usage_record =
            recent_processor_usage_record.[New]()

        Friend ReadOnly internal_counters() As internal_counter = {
                New internal_counter("COUNTER_INCREASE_LATENCY_TICKS", AddressOf selfhealth.increase_latency),
                New internal_counter("COUNTER_INCREASE_TIMES", AddressOf selfhealth.increase_times),
                New internal_counter("COUNTER_WRITE_LATENCY_TICKS", AddressOf selfhealth.write_latency),
                New internal_counter("COUNTER_WRITE_TIMES", AddressOf selfhealth.write_times),
                New internal_counter("PRIVATE_BYTES", AddressOf private_bytes_usage, False),
                New internal_counter("VIRTUAL_BYTES", AddressOf virtual_bytes_usage, False),
                New internal_counter("WORKINGSET_BYTES", AddressOf workingset_bytes_usage, False),
                New internal_counter("GC_TOTAL_MEMORY", AddressOf gc_total_memory, False),
                New internal_counter("PROCESSOR_USAGE",
                                     Function() As Int64
                                         Return CLng(processor_usage() * 100)
                                     End Function,
                                    False),
                New internal_counter("RECENT_PROCESSOR_USAGE",
                                     Function() As Int64
                                         Return CLng(recent_processor_usage(recent_processor_usage_record) * 100)
                                     End Function)}
    End Module
End Namespace
