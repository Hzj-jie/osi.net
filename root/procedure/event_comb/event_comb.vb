
Option Explicit On
Option Infer Off
Option Strict On

#Const USE_LOCK_T = False
#Const DISALLOW_REENTERABLE_LOCK = True
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.threadpool
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
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Protected Event suspending()
    Private _end_result As ternary
    Private cb As event_comb
    Private [step] As Int32
    Private pends As UInt32
    Private _begin_ticks As Int64
    Private _end_ticks As Int64
#If USE_LOCK_T Then
    Private _l As lock_t
#End If

    Public Shared Property current() As event_comb
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
            Return instance_stack(Of event_comb).current()
        End Get
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Protected Set(ByVal value As event_comb)
            instance_stack(Of event_comb).current() = value
        End Set
    End Property

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub New(ByVal d() As Func(Of Boolean), ByVal callstack As String)
        assert(Not callstack Is Nothing)
#If USE_LOCK_T Then
        _l = New lock_t(Me)
#End If
        ds = d
        ds_len = array_size(ds)
        assert(ds_len > 0)
        _callstack = callstack
        cancellation_control = New cancellation_controller(Me)
        _end_result = ternary.unknown
        cb = Nothing
        'following functions are all assert_in_lock protected
        debug_reenterable_locked(Sub()
                                     assert_goto_not_started()
                                     clear_pends()
                                     begin_ticks() = npos
                                     end_ticks() = npos
                                 End Sub)
    End Sub

#If DEBUG Then
    <MethodImpl(MethodImplOptions.NoInlining)>
    Public Sub New(ByVal ParamArray d() As Func(Of Boolean))
#Else
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal ParamArray d() As Func(Of Boolean))
#End If
        Me.New(d, Function() As String
                      If Not event_comb_trace AndAlso Not event_comb_alloc_trace Then
                          Return "##NOT_TRACE##"
                      End If
                      If event_comb_full_alloc_stack Then
                          Return connector.callstack()
                      End If
                      Return backtrace(Of event_comb)()
                  End Function())
    End Sub

    ' Creates a new event_comb instance with initial state.
    Public Function renew() As event_comb
        Return New event_comb(ds, callstack())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Function callstack() As String
        Return _callstack
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function end_result_raw() As ternary
        Return _end_result
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function end_result() As Boolean
        assert(end_result_raw().notunknown())
        Return end_result_raw().true_()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function pending() As Boolean
        Return pends > 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function not_pending() As Boolean
        Return pends = 0
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Sub attach_suspending(ByVal v As suspendingEventHandler)
        If Not v Is Nothing Then
            AddHandler suspending, v
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Sub inc_pends()
        assert_in_lock()
        pends += uint32_1
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Sub dec_pends()
        assert_in_lock()
        assert(pends > 0)
        pends -= uint32_1
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Protected Sub mark_as_failed()
        _end_result = False
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub clear_pends()
        assert_in_lock()
        pends = 0
    End Sub

    Public Property begin_ticks() As Int64
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
            Return _begin_ticks
        End Get
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Set(ByVal value As Int64)
            assert_in_lock()
            assert(_begin_ticks = npos OrElse value = npos)
            _begin_ticks = value
        End Set
    End Property

    Public Property end_ticks() As Int64
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
            Return _end_ticks
        End Get
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Private Set(ByVal value As Int64)
            assert_in_lock()
            assert(_end_ticks = npos OrElse value = npos)
            _end_ticks = value
        End Set
    End Property

    Private Sub _do()
        assert_in_lock()
        assert(not_pending())

        If _end() Then
            'the timeout has been called before the queued item in thread pool
            Return
        End If

        If _not_started() Then
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
        While _working() AndAlso not_pending()
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
            assert(Not _end())
        End While

        assert(_ending() = _callback_resume_ready()) 'really means Not [end]()
        If _ending() Then
            If event_comb_trace Then
                raise_error("event ", callstack(), " finished in step ", [step])
            End If
            end_ticks() = nowadays.ticks()
            cancellation_control.cancel()
            [resume](cb)
            assert(_end())
        ElseIf event_comb_trace Then
            raise_error("event ", callstack(), ":<step>", [step], " is now pending")
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Sub [resume](ByVal cb As event_comb)
        If cb Is Nothing Then
            Return
        End If
#If DISALLOW_REENTERABLE_LOCK Then
        thread_pool.push(AddressOf cb.resume)
#Else
        cb.resume()
#End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub [resume]()
        reenterable_locked(Sub()
                               If pending() Then
                                   dec_pends()
                                   If not_pending() Then
                                       _do()
                                   End If
                               End If
                           End Sub)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Friend Sub [do]()
        reenterable_locked(AddressOf _do)
    End Sub

    'ATTENTION, the cancel function does not try to cancel all the event_combs / callback_actions / void it waits for
    'it cancels itself only, so cancel from the latest event_comb is a good idea
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub cancel()
        suspend("has been canceled")
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub timeout()
        suspend("timeout")
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub suspend(ByVal action As String)
        reenterable_locked(Sub()
                               If _end() Then
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
                               assert(_end())
                           End Sub)
    End Sub
End Class
