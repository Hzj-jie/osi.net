
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.formation

Namespace counter
    Public Module _counter
        Public Function increase(ByVal id As Int64, ByVal dc As lazier(Of Int64)) As Boolean
            Dim cr As counter_record = Nothing
            cr = counter(id)
            Return cr IsNot Nothing AndAlso cr.increase(dc)
        End Function

        Public Function increase(ByVal id As Int64, ByVal dc As Func(Of Int64)) As Boolean
            Return increase(id, New lazier(Of Int64)(dc))
        End Function

        Public Function increase(ByVal id As Int64, Optional ByVal c As Int64 = 1) As Boolean
            Dim cr As counter_record = Nothing
            cr = counter(id)
            Return cr IsNot Nothing AndAlso cr.increase(c)
        End Function

        Public Function decrease(ByVal id As Int64, ByVal dc As lazier(Of Int64)) As Boolean
            Return increase(id,
                            New lazier(Of Int64)(Function() As Int64
                                                     Return -(+dc)
                                                 End Function))
        End Function

        Public Function decrease(ByVal id As Int64, ByVal dc As Func(Of Int64)) As Boolean
            Return decrease(id, New lazier(Of Int64)(dc))
        End Function

        Public Function decrease(ByVal id As Int64, Optional ByVal c As Int64 = 1) As Boolean
            Return increase(id, -c)
        End Function

        Private Class ts
            <ThreadStatic()> Public Shared startticks As Int64
        End Class

        Public Sub record_time_begin()
            ts.startticks = Now().Ticks()
        End Sub

        Public Function record_time_startticks() As Int64
            assert(ts.startticks > 0)
            Return ts.startticks
        End Function

        Public Function record_time_ms(ByVal id As Int64, ByVal startMs As Int64) As Boolean
            Return increase(id, Now().milliseconds() - startMs)
        End Function

        Public Function record_time_ms(ByVal id As Int64) As Boolean
            Return record_time_ms(id, ticks_to_milliseconds(record_time_startticks()))
        End Function

        Public Function record_time_ticks(ByVal id As Int64, ByVal startticks As Int64) As Boolean
            Return increase(id, Now().Ticks() - startticks)
        End Function

        Public Function record_time_ticks(ByVal id As Int64) As Boolean
            Return record_time_ticks(id, record_time_startticks())
        End Function
    End Module
End Namespace
