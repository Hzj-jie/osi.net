
Option Explicit On
Option Infer Off
Option Strict On

Imports c = osi.root.utils.counter

Namespace counter
    Public NotInheritable Class builder
        Private name As String
        Private count As Boolean
        Private average As Boolean
        Private last_average As Boolean
        Private last_average_length As Int64
        Private rate As Boolean
        Private last_rate As Boolean
        Private last_rate_length As Int64
        Private interval_scale As Int64
        Private sample_rate As Double

        Public Shared Function [New]() As builder
            Return New builder()
        End Function

        Public Function with_name(ByVal name As String) As builder
            Me.name = name
            Return Me
        End Function

        Public Function write_count() As builder
            Me.count = True
            Return Me
        End Function

        Public Function write_average() As builder
            Me.average = True
            Return Me
        End Function

        Public Function write_last_average() As builder
            Me.last_average = True
            Return Me
        End Function

        Public Function with_last_average_length(ByVal i As Int64) As builder
            Me.last_average_length = i
            Return Me
        End Function

        Public Function write_rate() As builder
            Me.rate = True
            Return Me
        End Function

        Public Function write_last_rate() As builder
            Me.last_rate = True
            Return Me
        End Function

        Public Function with_last_rate_length(ByVal i As Int64) As builder
            Me.last_rate_length = i
            Return Me
        End Function

        Public Function with_interval_scale(ByVal i As Int64) As builder
            Me.interval_scale = i
            Return Me
        End Function

        Public Function with_sample_rate(ByVal i As Double) As builder
            Me.sample_rate = i
            Return Me
        End Function

        Public Function register() As Int64
            Return c.register(name,
                              count,
                              average,
                              last_average,
                              last_average_length,
                              rate,
                              last_rate,
                              last_rate_length,
                              interval_scale,
                              sample_rate)
        End Function
    End Class
End Namespace
