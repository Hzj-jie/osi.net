
Imports osi.root.utt
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.envs
Imports osi.root.utils.stopwatch
Imports osi.root.lock
Imports System.Threading

Public Class stopwatch_test
    Inherits [case]

    Private Shared ReadOnly acceptable_latency_ms As Int64

    Shared Sub New()
        acceptable_latency_ms = sixteen_timeslice_length_ms
    End Sub

    Public Sub New()
        'make sure stopwatch has been initialized
        Dim finished As Boolean = False
        stopwatch.push(100,
                       Sub()
                           finished = True
                       End Sub)
        timeslice_sleep_wait_until(Function() finished)
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Public Overrides Function run() As Boolean
        Dim count As Int64 = 0
        count = rnd_int(100, 1000)
        Dim finished As Int64 = 0
        Dim wait_until_ms As Int64 = 0
        Dim sws() As stopwatch.[event] = Nothing
        ReDim sws(count - 1)
        For i As Int64 = 0 To count - 1
            Dim waitms As Int64 = 0
            waitms = rnd_int(300, 3000)
            Dim low As Int64 = 0
            low = nowadays.milliseconds() + waitms
            Dim high As Int64 = 0
            high = low + acceptable_latency_ms
            wait_until_ms = max(wait_until_ms, high)
            sws(i) = stopwatch.push(waitms,
                                    Sub()
                                        assertion.less_or_equal(nowadays.milliseconds(), high)
                                        assertion.more_or_equal(nowadays.milliseconds(), low)
                                        Interlocked.Increment(finished)
                                    End Sub)
        Next
        timeslice_sleep_wait_when(Function() finished < count AndAlso nowadays.milliseconds() < wait_until_ms)
        assertion.equal(finished, count)
        Return True
    End Function
End Class
