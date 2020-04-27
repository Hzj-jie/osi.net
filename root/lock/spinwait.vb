
Imports System.DateTime
Imports System.Threading
Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.delegates
Imports osi.root.constants
#Const USE_MEASURE_YIELD_WAIT = False

Public Module spinwait
    Sub New()
        Dim i As Int32 = loops_per_yield
        Dim b As Boolean = single_cpu
        assert(Timeout.Infinite = -1)
    End Sub

    Public Function should_yield() As Boolean
        Return single_cpu OrElse mono
    End Function

    Public Function not_force_yield() As Boolean
        Return yield(False)
    End Function

    Public Function force_yield() As Boolean
        Return yield(True)
    End Function

    Public Function yield(Optional ByVal force As Boolean = True) As Boolean
        If force OrElse should_yield() Then
            Dim start_ms As Int64 = 0
            start_ms = environment_milliseconds()
            If mono Then
                sleep(1)
            Else
                sleep(0)
            End If
            Return (environment_milliseconds() - start_ms) >= eighth_timeslice_length_ms
        Else
            Return False
        End If
    End Function

    Public Function try_wait_when(ByVal d As Func(Of Boolean),
                                  Optional ByVal ms As Int64 = npos,
                                  Optional ByRef wait_rounds As Int64 = 0,
                                  Optional ByRef yield_times As Int64 = 0) As Boolean
        If d Is Nothing Then
            Return False
        Else
            If ms < 0 Then
                ms = suggest_try_wait_length_ms
            End If
            wait_rounds = 0
            yield_times = 0
            Dim startMs As Int64 = 0
            startMs = environment_milliseconds()
            Dim rtn As Boolean = False
            rtn = d()
            While (environment_milliseconds() - startMs) < ms AndAlso rtn
                If not_force_yield() Then
                    yield_times.self_unchecked_inc()
                End If
                wait_rounds.self_unchecked_inc()
                rtn = d()
            End While

            Return rtn
        End If
    End Function

    Public Sub strict_wait_when(Of T)(ByVal d As _do(Of T, Boolean), ByRef o As T)
        If should_yield() Then
            lazy_wait_when(d, o)
        Else
#If DEBUG Then
            assert(Not d Is Nothing)
#End If
            While d(o)
            End While
        End If
    End Sub

    Public Sub wait_when(Of T)(ByVal d As _do(Of T, Boolean), ByRef o As T)
        If should_yield() Then
            lazy_wait_when(d, o)
        Else
#If DEBUG Then
            assert(Not d Is Nothing)
#End If
            Dim i As Int32 = 0
            While d(o)
                i += 1
                If i > loops_per_yield Then
                    If force_yield() Then
                        i = 0
                    Else
                        i = loops_per_yield
                    End If
                End If
            End While
        End If
    End Sub

    Public Sub lazy_wait_when(Of T)(ByVal d As _do(Of T, Boolean), ByRef o As T)
#If DEBUG Then
        assert(Not d Is Nothing)
#End If
        While d(o)
            force_yield()
        End While
    End Sub

    Public Sub strict_wait_when(ByVal d As Func(Of Boolean))
        assert(Not d Is Nothing)
        strict_wait_when(Function(ByRef x) d(), 0)
    End Sub

    Public Sub wait_when(ByVal d As Func(Of Boolean))
        assert(Not d Is Nothing)
        wait_when(Function(ByRef x) d(), 0)
    End Sub

    Public Sub lazy_wait_when(ByVal d As Func(Of Boolean))
        assert(Not d Is Nothing)
        lazy_wait_when(Function(ByRef x) d(), 0)
    End Sub

#If USE_MEASURE_YIELD_WAIT Then
    Private Function measure_yield_wait(ByVal e As WaitHandle, ByVal wait_ms As Int32) As Boolean
        Dim left As Int32 = 0
        left = Math.Ceiling(wait_ms * measure_sleep_percentage)
        Dim till As Int64 = 0
        till = Now().milliseconds() + left
        While left > 0
            not_force_yield()
            If e.WaitOne(left) Then
                Return True
            Else
                left = till - Now().milliseconds()
            End If
        End While
        Return e.WaitOne(0)
    End Function
#End If

    Public Function yield_wait(ByVal e As WaitHandle, Optional ByVal wait_ms As Int32 = 8) As Boolean
        If e Is Nothing Then
            not_force_yield()
            Return False
        Else
            If wait_ms < Timeout.Infinite Then
                wait_ms = Timeout.Infinite
            End If
            Try
                If e.WaitOne(0) Then
                    Return True
#If USE_MEASURE_YIELD_WAIT Then
                ElseIf mono Then
                    Return measure_yield_wait(e, wait_ms)
#End If
                Else
                    not_force_yield()
                    Return e.WaitOne(wait_ms)
                End If
            Catch ex As ObjectDisposedException
                Return False
            End Try
        End If
    End Function
End Module

Public Module _sleep_wait
    'should not use this, for test only
    Private Function select_sleep_ms() As Int64
        Try
            Return timeslice_length_ms * Math.Pow(2, min(this_process.ref.Threads().Count() \ 10, 5) + 2)
        Catch
            Return sixteen_timeslice_length_ms
        End Try
    End Function

    Public Sub timeslice_sleep_wait_when(Of T)(ByVal d As _do(Of T, Boolean), ByRef i As T)
        sleep_wait_when(d, i, select_sleep_ms())
    End Sub

    Public Function timeslice_sleep_wait_when(Of T)(ByVal d As _do(Of T, Boolean),
                                                    ByRef i As T,
                                                    ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_when(d, i, select_sleep_ms(), timeout_ms)
    End Function

    Public Sub timeslice_sleep_wait_until(Of T)(ByVal d As _do(Of T, Boolean), ByRef i As T)
        sleep_wait_until(d, i, select_sleep_ms())
    End Sub

    Public Function timeslice_sleep_wait_until(Of T)(ByVal d As _do(Of T, Boolean),
                                                     ByRef i As T,
                                                     ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_until(d, i, select_sleep_ms(), timeout_ms)
    End Function

    Public Sub timeslice_sleep_wait_when(ByVal d As Func(Of Boolean))
        sleep_wait_when(d, select_sleep_ms())
    End Sub

    Public Function timeslice_sleep_wait_when(ByVal d As Func(Of Boolean), ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_when(d, select_sleep_ms(), timeout_ms)
    End Function

    Public Sub timeslice_sleep_wait_until(ByVal d As Func(Of Boolean))
        sleep_wait_until(d, select_sleep_ms())
    End Sub

    Public Function timeslice_sleep_wait_until(ByVal d As Func(Of Boolean), ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_until(d, select_sleep_ms(), timeout_ms)
    End Function
End Module
