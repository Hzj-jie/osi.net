
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
#If DEBUG Then
Imports lock_t = osi.root.lock.monitorlock
#Else
Imports lock_t = osi.root.lock.slimlock.monitorlock
#End If

Partial Public Class event_comb
    Public Const end_step As Int32 = max_int32
    Public Const not_started_step As Int32 = -1
    Public Const first_step As Int32 = 0
    Private ReadOnly ds() As Func(Of Boolean)
    Private ReadOnly _callstack As String
    Private ReadOnly ds_len As UInt32    'for perf concern
    Protected Event suspending()
    Private _end_result As ternary
    Private timeouted_event As stopwatch.[event]
    Private cb As event_comb
    Private [step] As Int32
    Private pends As UInt32
    Private _begin_ticks As Int64
    Private _end_ticks As Int64
    Private _l As lock_t

    Public Shared Property current() As event_comb
        Get
            Return instance_stack(Of event_comb).current()
        End Get
        Protected Set(ByVal value As event_comb)
            instance_stack(Of event_comb).current() = value
        End Set
    End Property

    Shared Sub New()
        If event_comb_alloc_trace Then
            callstack_alloc = New map(Of String, Int64)()
        End If
    End Sub

    Protected Sub reenterable_locked(ByVal d As Action)
        assert(Not d Is Nothing)
        If lock_trace AndAlso event_comb_trace Then
            Dim n As Int64 = 0
            n = Now().milliseconds()
            _l.reenterable_locked(d)
            If Now().milliseconds() - n > half_timeslice_length_ms Then
                raise_error(error_type.performance,
                            callstack(), ":", [step],
                            " is using ", Now().milliseconds() - n, "ms to wait for another thread to finish")
            End If
        Else
            _l.reenterable_locked(d)
        End If
    End Sub

    Private Function reenterable_locked(ByVal f As Func(Of Boolean)) As Boolean
        assert(Not f Is Nothing)
        Dim r As Boolean = False
        reenterable_locked(Sub()
                               r = f()
                           End Sub)
        Return r
    End Function

#If DEBUG Then
    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Sub New(ByVal ParamArray d() As Func(Of Boolean))
#Else
    Public Sub New(ByVal ParamArray d() As Func(Of Boolean))
#End If
        _l = New lock_t(Me)
        ds = d
        ds_len = array_size(ds)
        If event_comb_trace OrElse event_comb_alloc_trace Then
            If event_comb_full_alloc_stack Then
                _callstack = connector.callstack()
            Else
                _callstack = callingcode("event_comb")
            End If
        Else
            _callstack = "##NOT_TRACE##"
        End If
        'following functions are all assert_in_lock protected
#If DEBUG Then
        reenterable_locked(Sub()
#End If
                               _end_result = ternary.unknown
                               timeouted_event = Nothing
                               cb = Nothing
                               assert_goto_not_started()
                               clear_pends()
                               begin_ticks() = npos
                               end_ticks() = npos
#If DEBUG Then
                           End Sub)
#End If
    End Sub

    Protected Function callstack() As String
        Return _callstack
    End Function

    Public Function end_result_raw() As ternary
        Return _end_result
    End Function

    Public Function end_result() As Boolean
        assert(end_result_raw().notunknown())
        Return end_result_raw().true_()
    End Function

    Public Function pending() As Boolean
        Return pends > 0
    End Function

    Public Function not_pending() As Boolean
        Return pends = 0
    End Function

    Protected Sub attach_suspending(ByVal v As suspendingEventHandler)
        If Not v Is Nothing Then
            AddHandler suspending, v
        End If
    End Sub

    Private Sub assert_in_lock()
#If DEBUG Then
        assert(_l.held_in_thread())
#End If
    End Sub

    Protected Sub inc_pends()
        assert_in_lock()
        pends += uint32_1
    End Sub

    Protected Sub dec_pends()
        assert_in_lock()
        assert(pends > 0)
        pends -= uint32_1
    End Sub

    Protected Sub mark_as_failed()
        _end_result = False
    End Sub

    Private Sub clear_pends()
        assert_in_lock()
        pends = 0
    End Sub

    Public Property begin_ticks() As Int64
        Get
            Return _begin_ticks
        End Get
        Private Set(ByVal value As Int64)
            assert(_begin_ticks = npos OrElse value = npos)
            _begin_ticks = value
        End Set
    End Property

    Public Property end_ticks() As Int64
        Get
            Return _end_ticks
        End Get
        Private Set(ByVal value As Int64)
            assert(_end_ticks = npos OrElse value = npos)
            _end_ticks = value
        End Set
    End Property

    Private Sub _do()
        assert_in_lock()
        assert(not_pending())

        If [end]() Then
            'the timeout has been called before the queued item in thread pool
            Return
        End If

        If not_started() Then
            If ds_len = uint32_0 Then
                assert_goto_end()
            Else
                assert_goto_begin()
            End If
            begin_ticks() = nowadays.ticks()
        End If

        If event_comb_trace Then
            raise_error("start event ", callstack(), ":<step>", [step])
        End If

        Dim laststep As Int32 = 0
        While working() AndAlso not_pending()
            Dim rtn As Boolean = False
            laststep = [step]
            current() = Me
            Try
                rtn = do_(ds([step]), False)
            Finally
                current() = Nothing
            End Try
            If rtn = False Then
                If event_comb_trace Then
                    raise_error(error_type.warning,
                                "event ", callstack(), ":<step>", laststep, " failed")
                End If
                assert_goto_end()
                clear_pends()
            End If
            If in_end_step() Then
                _end_result = rtn
            End If
            assert(Not [end]())
        End While

        assert(ending() = callback_resume_ready()) 'really means Not [end]()
        If ending() Then
            If event_comb_trace Then
                raise_error("event ", callstack(), " finished in step ", [step])
            End If
            end_ticks() = nowadays.ticks()
            cancel_timeout_event()
            [resume](cb)
            assert([end]())
        ElseIf event_comb_trace Then
            raise_error("event ", callstack(), ":<step>", [step], " is now pending")
        End If
    End Sub

    Private Shared Sub [resume](ByVal cb As event_comb)
        If Not cb Is Nothing Then
            cb.reenterable_locked(Sub()
                                      If cb.pending() Then
                                          cb.dec_pends()
                                          If cb.not_pending() Then
                                              cb._do()
                                          End If
                                      End If
                                  End Sub)
        End If
    End Sub

    Friend Sub [do]()
        reenterable_locked(AddressOf _do)
    End Sub

    Private Function cancel_timeout_event() As Boolean
        assert_in_lock()
        If timeouted_event Is Nothing Then
            Return False
        Else
            timeouted_event.cancel()
            timeouted_event = Nothing
            Return True
        End If
    End Function

    Friend Sub set_timeout(ByVal timeout_ms As Int64)
        reenterable_locked(Sub()
                               If timeout_ms < 0 Then
                                   cancel_timeout_event()
                               Else
                                   If event_comb_trace AndAlso timeout_ms >= max_int32 Then
                                       raise_error(error_type.performance,
                                                   "timeout of event_comb @ ",
                                                   callstack(),
                                                   " has been set to a number over maxInt32, ",
                                                   "which means it almost cannot be timeout forever")
                                   End If
                                   Dim wp As weak_pointer(Of event_comb) = Nothing
                                   wp = New weak_pointer(Of event_comb)(Me)
                                   timeouted_event = stopwatch.push(timeout_ms,
                                                                    Sub()
                                                                        'may be canceled between
                                                                        'stopwatch canceled test and do
                                                                        Dim ec As event_comb = Nothing
                                                                        ec = (+wp)
                                                                        If Not ec Is Nothing Then
                                                                            ec.timeout()
                                                                        End If
                                                                    End Sub)
                                   assert(Not timeouted_event Is Nothing)
                               End If
                           End Sub)
    End Sub

    'ATTENTION, the cancel function does not try to cancel all the event_combs / callback_actions / void it waits for
    'it cancels itself only, so cancel from the latest event_comb is a good idea
    Public Sub cancel()
        reenterable_locked(Sub()
                               If Not [end]() Then
                                   If event_comb_trace Then
                                       raise_error(error_type.warning, "event ", callstack(), " has been canceled")
                                   End If
                                   suspend()
                               End If
                           End Sub)
    End Sub

    Private Sub suspend()
        assert_in_lock()
        RaiseEvent suspending()
        assert_goto_end()
        mark_as_failed()
        clear_pends()
        _do()
        assert([end]())
    End Sub

    Private Sub timeout()
        reenterable_locked(Sub()
                               If Not [end]() AndAlso
                                  object_compare(timeouted_event, stopwatch.[event].current()) = 0 Then
                                   If event_comb_trace Then
                                       raise_error(error_type.warning, "event ", callstack(), " timeout")
                                   End If
                                   suspend()
                               End If
                           End Sub)
    End Sub
End Class
