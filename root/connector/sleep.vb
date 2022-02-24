
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports System.Threading
Imports osi.root.constants
Imports osi.root.delegates

Public Module _sleep
    Public Sub measure_sleep(ByVal ms As Int32)
        If ms > 0 Then
            Dim left As Int32 = 0
            left = CInt(Math.Ceiling(ms * measure_sleep_percentage))
            Dim till As Int64 = 0
            till = Now().milliseconds() + left
            While left > 0
                Thread.Sleep(left)
                left = CInt(till - Now().milliseconds())
            End While
        ElseIf ms = 0 Then
            Thread.Sleep(0)
        End If
    End Sub

    Public Sub sleep(ByVal ms As Int32)
        If ms = 0 Then
            Thread.Sleep(ms)
        ElseIf ms > 0 Then
            'for mono
            'Thread.Sleep has huge difference on mono
            If on_mono() Then
                measure_sleep(ms)
            Else
                Thread.Sleep(ms)
            End If
        End If
    End Sub

    Public Sub sleep(ByVal ms As Int64)
        If ms > max_int32 Then
            sleep(max_int32)
        Else
            sleep(CInt(ms))
        End If
    End Sub

    Public Sub sleep(ByVal ms As Double)
        If ms > max_int32 Then
            sleep(max_int32)
        Else
            sleep(CInt(ms))
        End If
    End Sub

    Public Sub sleep()
        sleep(second_milli)
    End Sub

    Public Sub suspend()
        Thread.Sleep(Timeout.Infinite)
    End Sub

    Public Sub sleep_seconds(Optional ByVal s As Int32 = 1)
        Dim i As Int64 = 0
        i = seconds_to_milliseconds(s)
        If i > max_int32 Then
            sleep(max_int32)
        ElseIf i < min_int32 Then
            sleep(min_int32)
        Else
            sleep(i)
        End If
    End Sub

    Public Sub sleep_ticks(Optional ByVal t As Int64 = second_milli * milli_tick)
        If t >= 0 Then
            Thread.Sleep(TimeSpan.FromTicks(t))
        End If
    End Sub

    Private Function until_when(Of T)(ByVal d As _do(Of T, Boolean)) As _do(Of T, Boolean)
        assert(Not d Is Nothing)
        Return Function(ByRef x) Not d(x)
    End Function

    Private Function when_when(ByVal d As Func(Of Boolean)) As _do(Of Byte, Boolean)
        assert(Not d Is Nothing)
        Return Function(ByRef x) d()
    End Function

    Private Function until_when(ByVal d As Func(Of Boolean)) As _do(Of Byte, Boolean)
        Return until_when(when_when(d))
    End Function

    Public Sub sleep_wait_when(Of T)(ByVal d As _do(Of T, Boolean), ByRef i As T, ByVal ms As Int64)
        assert(Not d Is Nothing)
        While d(i)
            sleep(ms)
        End While
    End Sub

    Public Function sleep_wait_when(Of T)(ByVal d As _do(Of T, Boolean),
                                          ByRef i As T,
                                          ByVal ms As Int64,
                                          ByVal timeout_ms As Int64) As Boolean
        assert(Not d Is Nothing)
        Dim start_ms As Int64 = 0
        start_ms = Now().milliseconds()
        Dim timeouted As Boolean = False
        sleep_wait_when(Function(ByRef x) As Boolean
                            If Not d(x) Then
                                Return False
                            End If
                            If Now().milliseconds() - start_ms >= timeout_ms Then
                                timeouted = True
                                Return False
                            End If
                            Return True
                        End Function,
                        i,
                        ms)
        Return Not timeouted
    End Function

    Public Sub sleep_wait_until(Of T)(ByVal d As _do(Of T, Boolean), ByRef i As T, ByVal ms As Int64)
        sleep_wait_when(until_when(d), i, ms)
    End Sub

    Public Function sleep_wait_until(Of T)(ByVal d As _do(Of T, Boolean),
                                           ByRef i As T,
                                           ByVal ms As Int64,
                                           ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_when(until_when(d), i, ms, timeout_ms)
    End Function

    Public Sub sleep_wait_when(ByVal d As Func(Of Boolean), ByVal ms As Int64)
        sleep_wait_when(when_when(d), int8_0, ms)
    End Sub

    Public Function sleep_wait_when(ByVal d As Func(Of Boolean),
                                    ByVal ms As Int64,
                                    ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_when(when_when(d), int8_0, ms, timeout_ms)
    End Function

    Public Function lazy_sleep_wait_when(ByVal d As Func(Of Boolean),
                                         ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_when(d, int64_0, timeout_ms)
    End Function

    Public Sub sleep_wait_until(ByVal d As Func(Of Boolean), ByVal ms As Int64)
        sleep_wait_when(until_when(d), int8_0, ms)
    End Sub

    Public Function sleep_wait_until(ByVal d As Func(Of Boolean),
                                     ByVal ms As Int64,
                                     ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_when(until_when(d), int8_0, ms, timeout_ms)
    End Function

    Public Function lazy_sleep_wait_until(ByVal d As Func(Of Boolean),
                                          ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_until(d, int64_0, timeout_ms)
    End Function
End Module
