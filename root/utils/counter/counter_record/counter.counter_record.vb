
' TODO: Rewrite
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.constants
Imports osi.root.constants.counter

Namespace counter
    Partial Friend Class counter_record
        Public ReadOnly name As String = Nothing
        Private ReadOnly multiple_instances As Boolean = False
        Private ReadOnly type As Int16 = 0
        Private ReadOnly register_time_ticks As Int64 = 0
        Private ReadOnly interval_scale As Int64 = 0

        Public ReadOnly count_name As String = Nothing
        Public ReadOnly average_name As String = Nothing
        Public ReadOnly last_average_name As String = Nothing
        Public ReadOnly rate_name As String = Nothing
        Public ReadOnly last_rate_name As String = Nothing

        Private count As Int64 = 0
        Private last_averages() As Int64 = Nothing
        Private last_times_ticks() As Int64 = Nothing
        Private calltimes As Int64 = 0
        Private sample_rate As Double = 0

        Public Function can_sample() As Boolean
            Return (Not type And counter_type.count) AndAlso
                   (Not type And counter_type.rate) AndAlso
                   (Not type And counter_type.last_rate)
        End Function

        Public Function has_value() As Boolean
            Return calltimes > 0
        End Function
    End Class
End Namespace
