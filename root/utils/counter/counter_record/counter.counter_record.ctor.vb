
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.constants.counter

Namespace counter
    Partial Friend Class counter_record
        Public Sub New(ByVal name As String,
                       ByVal write_count As Boolean,
                       ByVal write_average As Boolean,
                       ByVal write_last_average As Boolean,
                       ByVal last_average_length As Int64,
                       ByVal write_rate As Boolean,
                       ByVal write_last_rate As Boolean,
                       ByVal last_rate_length As Int64,
                       ByVal interval_scale As Int64,
                       ByVal sample_rate As Double)
            name = name.Replace(separator, character.underline)
            If last_average_length <= 0 Then
                last_average_length = default_last_average_length
            End If
            If last_rate_length <= 0 Then
                last_rate_length = default_last_rate_length
            End If
            If interval_scale <= 0 Then
                interval_scale = default_interval_scale
            End If

            copy(Me.name, name)
            Me.interval_scale = interval_scale
            If write_count Then
                select_count(Me.type)
            End If
            If write_average Then
                select_average(Me.type)
            End If
            If write_last_average Then
                select_last_average(Me.type)
            End If
            If write_last_average OrElse write_last_rate Then
                last_average_length = max(last_average_length, last_rate_length)
                ReDim Me.last_averages(CInt(last_average_length - 1))
            End If
            If write_rate Then
                select_rate(Me.type)
                Me.register_time_ticks = Now().Ticks()
            End If
            If write_last_rate Then
                select_last_rate(Me.type)
                ReDim Me.last_times_ticks(CInt(last_average_length - 1))
            End If
            If can_sample() Then
                Me.sample_rate = sample_rate
            Else
                Me.sample_rate = 1
            End If

            multiple_instances = (_1count(type) > 1)
            If count_selected() Then
                count_name = typed_counter_name("COUNT")
            End If
            If average_selected() Then
                average_name = typed_counter_name("AVERAGE")
            End If
            If last_average_selected() Then
                last_average_name = typed_counter_name(strcat("LAST",
                                                              Convert.ToString(last_averages.Length()),
                                                              "AVERAGE"))
            End If
            If rate_selected() Then
                rate_name = typed_counter_name("RATE")
            End If
            If last_rate_selected() Then
                last_rate_name = typed_counter_name(strcat("LAST",
                                                           Convert.ToString(last_times_ticks.Length()),
                                                           "RATE"))
            End If
        End Sub

        Private Function typed_counter_name(ByVal type As String) As String
            Return typed_counter_name(name, If(multiple_instances, type, Nothing))
        End Function

        Private Shared Function typed_counter_name(ByVal name As String, ByVal type As String) As String
            If type Is Nothing Then
                Return name
            Else
                Return strcat(name, character.underline, type)
            End If
        End Function
    End Class
End Namespace
