
Option Explicit On
Option Infer Off
Option Strict On

Namespace counter
    Public Module _counter
#If DEBUG Then
        Public Const write_frequence_seconds As Int32 = 60
#Else
        Public Const write_frequence_seconds As Int32 = 5 * 60
#End If
        Public Const default_last_average_length As Int64 = 8196
        Public Const default_last_rate_length As Int64 = 8196
        Public Const default_interval_scale As Int64 = 1
        Public Const default_sample_rate As Double = 0.0001
        Public Const separator As String = character.comma + character.blank
        Public Const write_frequence_minutes As Int32 = write_frequence_seconds \ minute_second
        Public Const write_frequence_milliseconds As Int32 = write_frequence_seconds * second_milli

        Public Class counter_type
            Public Const count As Int16 = 1
            Public Const average As Int16 = 2
            Public Const last_average As Int16 = 4
            Public Const rate As Int16 = 8
            Public Const last_rate As Int16 = 16
        End Class
    End Module
End Namespace
