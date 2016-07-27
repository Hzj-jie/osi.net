
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device

Public Class buffered_flower(Of T)
    Inherits flower(Of T)

    Public ReadOnly input_flower As direct_flower(Of T)
    Public ReadOnly output_flower As direct_flower(Of T)

    Private Shared Function create_input_flower(ByVal input As T_receiver(Of T),
                                                ByVal pipe_dev As pipe_dev(Of T),
                                                ByVal sense_timeout_ms As Int64,
                                                ByVal broken_pipe As ref(Of singleentry),
                                                ByVal idle_timeout_ms As Int64,
                                                ByVal result As atomic_int64,
                                                ByVal sizeof As binder(Of Func(Of T, UInt64), 
                                                                          sizeof_binder_protector),
                                                ByVal treat_no_flow_as_failure As Boolean) _
                                               As direct_flower(Of T)
        assert(Not input Is Nothing)
        assert(Not pipe_dev Is Nothing)
        assert(Not broken_pipe Is Nothing)
        Return New direct_flower(Of T)(input,
                                       pipe_dev,
                                       sense_timeout_ms,
                                       broken_pipe,
                                       idle_timeout_ms,
                                       result,
                                       sizeof,
                                       treat_no_flow_as_failure)
    End Function

    Private Shared Function create_output_flower(ByVal pipe_dev As pipe_dev(Of T),
                                                 ByVal output As T_sender(Of T),
                                                 ByVal sense_timeout_ms As Int64,
                                                 ByVal broken_pipe As ref(Of singleentry),
                                                 ByVal idle_timeout_ms As Int64) As direct_flower(Of T)
        assert(Not pipe_dev Is Nothing)
        assert(Not output Is Nothing)
        assert(Not broken_pipe Is Nothing)
        Return New direct_flower(Of T)(pipe_dev, output, sense_timeout_ms, broken_pipe, idle_timeout_ms)
    End Function

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal sense_timeout_ms As Int64,
                   ByVal broken_pipe As ref(Of singleentry),
                   ByVal pipe As pipe(Of T),
                   ByVal idle_timoeut_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        MyBase.New()
        assert(Not pipe Is Nothing)
        Dim pipe_dev As pipe_dev(Of T) = Nothing
        pipe_dev = New pipe_dev(Of T)(pipe)
        If broken_pipe Is Nothing Then
            broken_pipe = New ref(Of singleentry)()
        End If
        Me.input_flower = create_input_flower(input,
                                              pipe_dev,
                                              sense_timeout_ms,
                                              broken_pipe,
                                              idle_timoeut_ms,
                                              result,
                                              sizeof,
                                              treat_no_flow_as_failure)
        Me.output_flower = create_output_flower(pipe_dev, output, sense_timeout_ms, broken_pipe, idle_timoeut_ms)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal sense_timeout_ms As Int64,
                   ByVal broken_pipe As ref(Of singleentry),
                   ByVal pipe As pipe(Of T),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, sense_timeout_ms, broken_pipe, pipe, npos, result, sizeof, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal sense_timeout_ms As Int64,
                   ByVal broken_pipe As ref(Of singleentry),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input,
               output,
               sense_timeout_ms,
               broken_pipe,
               New pipe(Of T)(),
               idle_timeout_ms,
               result,
               sizeof,
               treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal sense_timeout_ms As Int64,
                   ByVal broken_pipe As ref(Of singleentry),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, sense_timeout_ms, broken_pipe, npos, result, sizeof, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal broken_pipe As ref(Of singleentry),
                   ByVal pipe As pipe(Of T),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input,
               output,
               constants.default_sense_timeout_ms,
               broken_pipe,
               pipe,
               idle_timeout_ms,
               result,
               sizeof, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal broken_pipe As ref(Of singleentry),
                   ByVal pipe As pipe(Of T),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, broken_pipe, pipe, npos, result, sizeof, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal broken_pipe As ref(Of singleentry),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, broken_pipe, New pipe(Of T)(), idle_timeout_ms, result, sizeof, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal broken_pipe As ref(Of singleentry),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, broken_pipe, npos, result, sizeof, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal pipe As pipe(Of T),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, Nothing, pipe, idle_timeout_ms, result, sizeof, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal pipe As pipe(Of T),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, pipe, npos, result, sizeof, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, New pipe(Of T)(), idle_timeout_ms, result, sizeof, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, npos, result, sizeof, treat_no_flow_as_failure)
    End Sub

    Public Overrides Function [stop]() As Boolean
        Return input_flower.stop() AndAlso
               output_flower.stop()
    End Function

    Protected Overrides Function flow() As event_comb
        Dim ec1 As event_comb = Nothing
        Dim ec2 As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec1 = (+input_flower)
                                  ec2 = (+output_flower)
                                  Return waitfor(ec1, ec2) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec1.end_result() AndAlso
                                         ec2.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
