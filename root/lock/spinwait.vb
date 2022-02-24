
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.envs
#Const USE_MEASURE_YIELD_WAIT = False
#Const USE_BUILT_IN_YIELD = True

Public Module spinwait
    <global_init(global_init_level.runtime_checkers)>
    Private NotInheritable Class assertions
        Private Shared Sub init()
            assert(Timeout.Infinite = -1)
        End Sub

        Private Sub New()
        End Sub
    End Class

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function should_yield() As Boolean
        Return single_cpu OrElse mono
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function not_force_yield() As Boolean
        Return yield(False)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function force_yield() As Boolean
        Return yield(True)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function yield() As Boolean
        Return yield(True)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function yield(ByVal force As Boolean) As Boolean
        If Not force AndAlso Not should_yield() Then
            Return False
        End If
#If USE_BUILT_IN_YIELD Then
        Return Thread.Yield()
#Else
        Dim start_ms As Int64 = environment_milliseconds()
        If mono Then
            sleep(1)
        Else
            sleep(0)
        End If
        Return (environment_milliseconds() - start_ms) >= 8 * timeslice_length_ms
#End If
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function try_wait_when(ByVal d As Func(Of Boolean),
                                  Optional ByVal ms As Int64 = npos,
                                  Optional ByRef wait_rounds As Int64 = 0,
                                  Optional ByRef yield_times As Int64 = 0) As Boolean
        If d Is Nothing Then
            Return False
        End If
        If ms < 0 Then
            ms = suggest_try_wait_length_ms
        End If
        wait_rounds = 0
        yield_times = 0
        Dim startMs As Int64 = environment_milliseconds()
        Dim rtn As Boolean = d()
        While (environment_milliseconds() - startMs) < ms AndAlso rtn
            If not_force_yield() Then
                yield_times.self_unchecked_inc()
            End If
            wait_rounds.self_unchecked_inc()
            rtn = d()
        End While

        Return rtn
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
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

    Private Structure ifunc_delegate(Of T)
        Implements ifunc(Of T, Boolean)

        Private ReadOnly f As _do(Of T, Boolean)

        Public Sub New(ByVal f As _do(Of T, Boolean))
            assert(Not f Is Nothing)
            Me.f = f
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Function run(ByRef i As T) As Boolean Implements ifunc(Of T, Boolean).run
            Return f(i)
        End Function
    End Structure

    ' Use structure and stack allocation to ensure the performance. 
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait_when(Of T, DT As {ifunc(Of T, Boolean), Structure})(ByRef f As DT, ByRef o As T)
        If should_yield() Then
            lazy_wait_when(f, o)
        Else
            Dim i As Int32 = 0
            While f.run(o)
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

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait_when(Of T)(ByVal d As _do(Of T, Boolean), ByRef o As T)
        wait_when(New ifunc_delegate(Of T)(d), o)
    End Sub

    ' Use structure and stack allocation to ensure the performance. 
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub lazy_wait_when(Of T, DT As {ifunc(Of T, Boolean), Structure})(ByRef f As DT, ByRef o As T)
        While f.run(o)
            force_yield()
        End While
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub lazy_wait_when(Of T)(ByVal d As _do(Of T, Boolean), ByRef o As T)
        lazy_wait_when(New ifunc_delegate(Of T)(d), o)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub strict_wait_when(ByVal d As Func(Of Boolean))
        assert(Not d Is Nothing)
        strict_wait_when(Function(ByRef x) d(), 0)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub wait_when(ByVal d As Func(Of Boolean))
        assert(Not d Is Nothing)
        wait_when(Function(ByRef x) d(), 0)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
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

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function yield_wait(ByVal e As WaitHandle, Optional ByVal wait_ms As Int32 = 8) As Boolean
        If e Is Nothing Then
            not_force_yield()
            Return False
        End If
        If wait_ms < Timeout.Infinite Then
            wait_ms = Timeout.Infinite
        End If
        Try
            If e.WaitOne(0) Then
                Return True
            End If
#If USE_MEASURE_YIELD_WAIT Then
            If mono Then
                Return measure_yield_wait(e, wait_ms)
            End If
#End If
            not_force_yield()
            Return e.WaitOne(wait_ms)
        Catch ex As ObjectDisposedException
            Return False
        End Try
    End Function
End Module

Public Module _sleep_wait
    'should not use this, for test only
    Private Function select_sleep_ms() As Int64
        Try
            Return CLng(timeslice_length_ms * Math.Pow(2, min(this_process.ref.Threads().Count() \ 10, 5) + 2))
        Catch
            Return 16 * timeslice_length_ms
        End Try
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub timeslice_sleep_wait_when(Of T)(ByVal d As _do(Of T, Boolean), ByRef i As T)
        sleep_wait_when(d, i, select_sleep_ms())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function timeslice_sleep_wait_when(Of T)(ByVal d As _do(Of T, Boolean),
                                                    ByRef i As T,
                                                    ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_when(d, i, select_sleep_ms(), timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub timeslice_sleep_wait_until(Of T)(ByVal d As _do(Of T, Boolean), ByRef i As T)
        sleep_wait_until(d, i, select_sleep_ms())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function timeslice_sleep_wait_until(Of T)(ByVal d As _do(Of T, Boolean),
                                                     ByRef i As T,
                                                     ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_until(d, i, select_sleep_ms(), timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub timeslice_sleep_wait_when(ByVal d As Func(Of Boolean))
        sleep_wait_when(d, select_sleep_ms())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function timeslice_sleep_wait_when(ByVal d As Func(Of Boolean), ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_when(d, select_sleep_ms(), timeout_ms)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub timeslice_sleep_wait_until(ByVal d As Func(Of Boolean))
        sleep_wait_until(d, select_sleep_ms())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function timeslice_sleep_wait_until(ByVal d As Func(Of Boolean), ByVal timeout_ms As Int64) As Boolean
        Return sleep_wait_until(d, select_sleep_ms(), timeout_ms)
    End Function
End Module
