
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.constants
Imports osi.root.envs

Partial Public Class callback_action
    Protected timeouted As Action = Nothing
    Protected resetting As Action = Nothing
    Private ReadOnly _callstack As String = Nothing
    Private begin As Func(Of Boolean) = Nothing
    Private check As Func(Of Boolean) = Nothing
    Private [end] As Func(Of Boolean) = Nothing
    Private before_step As before_step_t = Nothing
    Private timeoutticks As Int64 = npos

    Private Enum before_step_t As Byte
        begin
        check
        [end]
        finished
    End Enum

    Protected Sub trigger_check()
        callback_manager.global.trigger_check()
    End Sub

    Public Function finished() As Boolean
        Return before_step = before_step_t.finished
    End Function

    Public Function pending() As Boolean
        Return before_step = before_step_t.check
    End Function

    Public Function begining() As Boolean
        Return before_step = before_step_t.begin
    End Function

    Public Function ending() As Boolean
        Return before_step = before_step_t.end
    End Function

    Private Sub to_begining()
        before_step = before_step_t.begin
    End Sub

    Private Sub to_pending()
        before_step = before_step_t.check
    End Sub

    Private Sub to_ending()
        before_step = before_step_t.end
    End Sub

    Private Sub to_finished()
        before_step = before_step_t.finished
    End Sub

    Private _begin_result As ternary = ternary.unknown
    Public Property begin_result As ternary
        Get
            Return _begin_result
        End Get
        Private Set(ByVal value As ternary)
            assert(_end_result.unknown_() OrElse value.unknown_())
            _begin_result = value
        End Set
    End Property

    Private _check_result As ternary = ternary.unknown
    Public Property check_result As ternary
        Get
            Return _check_result
        End Get
        Private Set(ByVal value As ternary)
            assert(_check_result.nottrue() OrElse value.unknown_())
            _check_result = value
        End Set
    End Property

    Private _end_result As ternary = ternary.unknown
    Public Property end_result As ternary
        Get
            Return _end_result
        End Get
        Private Set(ByVal value As ternary)
            assert(_end_result.unknown_() OrElse value.unknown_())
            _end_result = value
        End Set
    End Property

    Private _ctor_ticks As Int64 = npos
    Public Property ctor_ticks As Int64
        Get
            Return _ctor_ticks
        End Get
        Private Set(ByVal value As Int64)
            _ctor_ticks = value
        End Set
    End Property

    Private _begin_ticks As Int64 = npos
    Public Property begin_ticks As Int64
        Get
            Return _begin_ticks
        End Get
        Private Set(ByVal value As Int64)
            If _begin_ticks = npos OrElse value = npos Then
                _begin_ticks = value
            End If
        End Set
    End Property

    Private _check_ticks As Int64 = npos
    Public Property check_ticks As Int64
        Get
            Return _check_ticks
        End Get
        Private Set(ByVal value As Int64)
            If _check_ticks = npos OrElse value = npos Then
                _check_ticks = value
            End If
        End Set
    End Property

    Private _end_ticks As Int64 = npos
    Public Property end_ticks As Int64
        Get
            Return _end_ticks
        End Get
        Private Set(ByVal value As Int64)
            assert(_end_ticks = npos OrElse value = npos)
            _end_ticks = value
        End Set
    End Property

    Private _finish_ticks As Int64 = npos
    Public Property finish_ticks As Int64
        Get
            Return _finish_ticks
        End Get
        Private Set(ByVal value As Int64)
            assert(_finish_ticks = npos OrElse value = npos)
            _finish_ticks = value
        End Set
    End Property

    Public Shared Property current As callback_action
        Get
            Return instance_stack(Of callback_action).current()
        End Get
        Protected Set(ByVal value As callback_action)
            'removed, since callbackManager2 will run several chained callbackActions in one thread
            'assert(value Is Nothing OrElse _current Is Nothing OrElse object_compare(value, _current) = 0)
            instance_stack(Of callback_action).current() = value
        End Set
    End Property

    Friend Shared Function in_callback_action_thread() As Boolean
        Return Not instance_stack(Of callback_action).empty()
    End Function

    Public Sub cancel(Optional ByVal beginResult As Boolean = True, Optional ByVal endResult As Boolean = True)
        begin = todo(beginResult)
        check = todo(check_pass)
        [end] = todo(endResult)
    End Sub

    Public Sub set_timeout_ticks(ByVal ticks As Int64)
        timeoutticks = ticks
    End Sub

    Public Sub set_timeout_ms(ByVal ms As Int64)
        set_timeout_ticks(milliseconds_to_ticks(ms))
    End Sub

    Public Sub reset()
        begin_ticks() = npos
        check_ticks() = npos
        end_ticks() = npos
        finish_ticks() = npos
        to_begining()
        begin_result() = ternary.unknown
        check_result() = ternary.unknown
        end_result() = ternary.unknown
        void_(resetting)
    End Sub

    Public Sub reset(ByVal timeoutticks As Int64)
        set_timeout_ticks(timeoutticks)
        reset()
    End Sub

    Protected Function callstack() As String
        Return _callstack
    End Function

    Friend Function timeout() As Boolean
        Return timeoutticks >= 0 AndAlso (nowadays.ticks() - begin_ticks()) >= timeoutticks
    End Function

    Private Function action(ByVal d As Func(Of Boolean)) As Boolean
        assert(Not d Is Nothing)
        Dim rtn As Boolean = Nothing
        current() = Me
        Try
            rtn = do_(d, False)
        Finally
            current() = Nothing
        End Try
        Return rtn
    End Function

    Friend Sub action_begin()
        begin_ticks() = nowadays.ticks()
        begin_result() = action(begin)
        If begin_result().true_() Then
            to_pending()
        Else
            raise_error(error_type.warning, "action ", callstack(), ":begin failed.")
            to_ending()
        End If
    End Sub

    Friend Function action_check() As Boolean
        assert(pending())
        check_ticks() = nowadays.ticks()
        check_result() = action(check)
        If check_result() = check_pass Then
            to_ending()
        ElseIf timeout() Then
            void_(timeouted)
            If callback_trace Then
                raise_error(error_type.warning, "action ", callstack(), " timeout.")
            End If
            to_ending()
        End If
        If pending() Then
            Return True
        Else
            assert_begin(Me)
            Return False
        End If
    End Function

    Friend Sub action_end()
        end_ticks() = nowadays.ticks()
        end_result() = action([end])
        to_finished()
        finish_ticks() = nowadays.ticks()
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Protected Sub New(ByVal begin As Func(Of Boolean),
                      ByVal check As Func(Of Boolean),
                      ByVal [end] As Func(Of Boolean),
                      ByVal timeoutticks As Int64,
                      ByVal additional_jump As Int32)
        Me.begin = If(begin Is Nothing, todo(True), begin)
        Me.check = If(check Is Nothing, todo(check_pass), check)
        Me.end = If([end] Is Nothing, todo(True), [end])
        Me.ctor_ticks() = nowadays.ticks()
        Me.timeoutticks = timeoutticks
        If callback_trace Then
            Me._callstack = backtrace(additional_jump + 1)
        Else
            Me._callstack = "##NOT_TRACE##"
        End If
        reset()
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)> _
    Public Sub New(Optional ByVal begin As Func(Of Boolean) = Nothing,
                   Optional ByVal check As Func(Of Boolean) = Nothing,
                   Optional ByVal [end] As Func(Of Boolean) = Nothing,
                   Optional ByVal timeoutticks As Int64 = npos)
        Me.new(begin, check, [end], timeoutticks, 1)
    End Sub

    Protected Shared Function todo(Of T)(ByVal v As T) As Func(Of T)
        Return Function() As T
                   Return v
               End Function
    End Function
End Class
