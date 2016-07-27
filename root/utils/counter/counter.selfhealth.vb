
Imports System.DateTime
Imports System.Threading
Imports osi.root.lock

Namespace counter
    Public Module _counter_selfhealth
        Friend Class selfhealth
            Private Shared _increase_latency As Int64 = 0
            Private Shared _increase_times As Int32 = 0
            Private Shared _write_latency As Int64 = 0
            Private Shared _write_times As Int32 = 0

            Public Shared Sub record_increase_latency(ByVal startticks As Int64)
                If envs.counter_selfhealth Then
                    Interlocked.Add(_increase_latency, Now().Ticks() - startticks)
                    Interlocked.Increment(_increase_times)
                End If
            End Sub

            Public Shared Sub record_write_latency(ByVal startticks As Int64)
                If envs.counter_selfhealth Then
                    Interlocked.Add(_write_latency, Now().Ticks() - startticks)
                    Interlocked.Increment(_write_times)
                End If
            End Sub

            Public Shared Function increase_latency() As Int64
                Return return_clear(_increase_latency)
            End Function

            Public Shared Function increase_times() As Int64
                Return return_clear(_increase_times)
            End Function

            Public Shared Function write_latency() As Int64
                Return return_clear(_write_latency)
            End Function

            Public Shared Function write_times() As Int64
                Return return_clear(_write_times)
            End Function

            Private Shared Function return_clear(ByRef i As Int32) As Int64
                If envs.counter_selfhealth Then
                    Dim j As Int64 = 0
                    j = atomic.read(i)
                    atomic.eva(i, 0)
                    Return j
                Else
                    Return 0
                End If
            End Function

            Private Shared Function return_clear(ByRef i As Int64) As Int64
                If envs.counter_selfhealth Then
                    Dim j As Int64 = 0
                    j = atomic.read(i)
                    atomic.eva(i, 0)
                    Return j
                Else
                    Return 0
                End If
            End Function
        End Class
    End Module
End Namespace
