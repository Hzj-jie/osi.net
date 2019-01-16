
Option Explicit On
Option Infer Off
Option Strict On

Imports System.DateTime
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.lock
#If DEBUG Then
Imports lock_t = osi.root.lock.monitorlock
#Else
Imports lock_t = osi.root.lock.slimlock.monitorlock
#End If

Partial Public Class event_comb
    Public Const end_step As Int32 = max_int32
    Public Const not_started_step As Int32 = -1
    Public Const first_step As Int32 = 0
    Public ReadOnly cancellation_control As cancellation_controller
    Private ReadOnly ds() As Func(Of Boolean)
    Private ReadOnly _callstack As String
    Private ReadOnly ds_len As UInt32    'for perf concern
    Protected Event suspending()
    Private _end_result As ternary
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

    Private Sub New(ByVal d() As Func(Of Boolean), ByVal callstack As String)
        assert(Not callstack Is Nothing)
        _l = New lock_t(Me)
        ds = d
        ds_len = array_size(ds)
        assert(ds_len > 0)
        _callstack = callstack
        cancellation_control = New cancellation_controller(Me)
        _end_result = ternary.unknown
        cb = Nothing
        'following functions are all assert_in_lock protected
#If DEBUG Then
        reenterable_locked(Sub()
#End If
                               assert_goto_not_started()
                               clear_pends()
                               begin_ticks() = npos
                               end_ticks() = npos
#If DEBUG Then
                           End Sub)
#End If
    End Sub

#If DEBUG Then
    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Sub New(ByVal ParamArray d() As Func(Of Boolean))
#Else
    Public Sub New(ByVal ParamArray d() As Func(Of Boolean))
#End If
        Me.New(d, Function() As String
                      If event_comb_trace OrElse event_comb_alloc_trace Then
                          If event_comb_full_alloc_stack Then
                              Return connector.callstack()
                          End If
                          Return backtrace(Of event_comb)()
                      End If
                      Return "##NOT_TRACE##"
                  End Function())
    End Sub

    ' Creates a new event_comb instance with initial state.
    Public Function renew() As event_comb
        Return New event_comb(ds, callstack())
    End Function

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
            cancellation_control.cancel()
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

    'ATTENTION, the cancel function does not try to cancel all the event_combs / callback_actions / void it waits for
    'it cancels itself only, so cancel from the latest event_comb is a good idea
    Public Sub cancel()
        suspend("has been canceled")
    End Sub

    Private Sub timeout()
        suspend("timeout")
    End Sub

    Private Sub suspend(ByVal action As String)
        reenterable_locked(Sub()
                               If [end]() Then
                                   Return
                               End If
                               If event_comb_trace Then
                                   raise_error(error_type.warning, "event ", callstack(), " ", action)
                               End If
                               RaiseEvent suspending()
                               assert_goto_end()
                               mark_as_failed()
                               clear_pends()
                               _do()
                               assert([end]())
                           End Sub)
    End Sub
End Class
