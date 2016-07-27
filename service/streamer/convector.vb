
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.device

Public Class convector(Of T)
    Private ReadOnly a_b As flower(Of T)
    Private ReadOnly b_a As flower(Of T)

    Private Shared Function create_flower(ByVal dev1 As T_receiver(Of T),
                                          ByVal dev2 As T_sender(Of T),
                                          ByVal sense_timeout_ms As Int64,
                                          ByVal broken_pipe As ref(Of singleentry),
                                          ByVal use_buffered_flow As Boolean,
                                          ByVal idle_timeout_ms As Int64,
                                          ByVal result As atomic_int64,
                                          ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector),
                                          ByVal treat_no_flow_as_failure As Boolean) _
                                         As flower(Of T)
        Return If(use_buffered_flow,
                  New buffered_flower(Of T)(dev1,
                                            dev2,
                                            sense_timeout_ms,
                                            broken_pipe,
                                            idle_timeout_ms,
                                            result,
                                            sizeof,
                                            treat_no_flow_as_failure),
                  New direct_flower(Of T)(dev1,
                                          dev2,
                                          sense_timeout_ms,
                                          broken_pipe,
                                          idle_timeout_ms,
                                          result,
                                          sizeof,
                                          treat_no_flow_as_failure))
    End Function

    Public Sub New(ByVal create_flower_a_b As Func(Of ref(Of singleentry), flower(Of T)),
                   ByVal create_flower_b_a As Func(Of ref(Of singleentry), flower(Of T)))
        assert(Not create_flower_a_b Is Nothing)
        assert(Not create_flower_b_a Is Nothing)
        Dim broken_pipe As ref(Of singleentry) = Nothing
        broken_pipe = New ref(Of singleentry)()
        Me.a_b = create_flower_a_b(broken_pipe)
        Me.b_a = create_flower_b_a(broken_pipe)
    End Sub

    Public Sub New(ByVal dev1 As dev_T(Of T),
                   ByVal dev2 As dev_T(Of T),
                   ByVal sense_timeout_ms As Int64,
                   ByVal use_buffered_flower As Boolean,
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(Function(broken_pipe As ref(Of singleentry)) As flower(Of T)
                   Return create_flower(dev1,
                                        dev2,
                                        sense_timeout_ms,
                                        broken_pipe,
                                        use_buffered_flower,
                                        idle_timeout_ms,
                                        result,
                                        sizeof,
                                        treat_no_flow_as_failure)
               End Function,
               Function(broken_pipe As ref(Of singleentry)) As flower(Of T)
                   Return create_flower(dev2,
                                        dev1,
                                        sense_timeout_ms,
                                        broken_pipe,
                                        use_buffered_flower,
                                        idle_timeout_ms,
                                        result,
                                        sizeof,
                                        treat_no_flow_as_failure)
               End Function)
    End Sub

    Public Sub New(ByVal dev1 As dev_T(Of T),
                   ByVal dev2 As dev_T(Of T),
                   ByVal sense_timeout_ms As Int64,
                   ByVal pipe1 As pipe(Of T),
                   ByVal pipe2 As pipe(Of T),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(Function(broken_pipe As ref(Of singleentry)) As flower(Of T)
                   Return New buffered_flower(Of T)(dev1,
                                                    dev2,
                                                    sense_timeout_ms,
                                                    broken_pipe,
                                                    assert_return(object_compare(pipe1, pipe2) <> 0, pipe1),
                                                    idle_timeout_ms,
                                                    result,
                                                    sizeof,
                                                    treat_no_flow_as_failure)
               End Function,
               Function(broken_pipe As ref(Of singleentry)) As flower(Of T)
                   Return New buffered_flower(Of T)(dev2,
                                                    dev1,
                                                    sense_timeout_ms,
                                                    broken_pipe,
                                                    assert_return(object_compare(pipe1, pipe2) <> 0, pipe2),
                                                    idle_timeout_ms,
                                                    result,
                                                    sizeof,
                                                    treat_no_flow_as_failure)
               End Function)
    End Sub

    Public Sub New(ByVal dev1 As dev_T(Of T),
                   ByVal dev2 As dev_T(Of T),
                   ByVal sense_timeout_ms As Int64,
                   ByVal pipe As pipe(Of T),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(dev1,
               dev2,
               sense_timeout_ms,
               pipe,
               New pipe(Of T)(pipe),
               idle_timeout_ms,
               result,
               sizeof,
               treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal dev1 As dev_T(Of T),
                   ByVal dev2 As dev_T(Of T),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal sizeof As binder(Of Func(Of T, UInt64), sizeof_binder_protector) = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(Function(broken_pipe As ref(Of singleentry)) As flower(Of T)
                   Return New buffered_flower(Of T)(dev1, dev2, broken_pipe, treat_no_flow_as_failure)
               End Function,
               Function(broken_pipe As ref(Of singleentry)) As flower(Of T)
                   Return New buffered_flower(Of T)(dev2, dev1, broken_pipe, treat_no_flow_as_failure)
               End Function)
    End Sub

    Private Function convect() As event_comb
        Dim ec1 As event_comb = Nothing
        Dim ec2 As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec1 = (+a_b)
                                  ec2 = (+b_a)
                                  Return waitfor(ec1, ec2) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec1.end_result() AndAlso
                                         ec2.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function [stop]() As Boolean
        Return If(object_compare(a_b, b_a) = 0, a_b.stop(), a_b.stop() AndAlso b_a.stop())
    End Function

    Public Function stopped() As Boolean
        Return If(object_compare(a_b, b_a) = 0, a_b.stopped(), a_b.stopped() AndAlso b_a.stopped())
    End Function

    Public Shared Operator +(ByVal this As convector(Of T)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If this Is Nothing Then
                                      Return False
                                  Else
                                      ec = this.convect()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Operator
End Class
