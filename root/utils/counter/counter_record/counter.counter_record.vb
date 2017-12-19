
Option Explicit On
Option Infer Off
Option Strict On

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

        ' Interlocked.Increment is used to avoid lock.
        Private count As Int64 = 0
        Private last_averages() As Int64 = Nothing
        Private last_times_ticks() As Int64 = Nothing
        Private calltimes As Int64 = 0
        Private sample_rate As Double = 0

        Public Function can_sample() As Boolean
            Return Not count_selected() AndAlso
                   Not rate_selected() AndAlso
                   Not last_rate_selected()
        End Function

        Public Function has_value() As Boolean
            Return calltimes > 0
        End Function
    End Class
End Namespace
