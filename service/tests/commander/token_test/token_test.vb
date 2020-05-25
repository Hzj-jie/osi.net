
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.commander

Partial Public Class token_test
    Inherits [case]

    Private Const concurrency_connection As UInt32 = 128
    Private Const mock_ppt_count_lower_bound As UInt32 = 30
    Private Const mock_ppt_count_upper_bound As UInt32 = 40

    Private ReadOnly forward_questioner_responder As forward_questioner_responder
    Private ReadOnly defender_creator As Func(Of token_info(Of mock_ppt, mock_conn), 
                                                 itoken_defender(Of mock_ppt, mock_conn))
    Private ReadOnly challenger_creator As Func(Of token_info(Of mock_ppt, mock_conn), 
                                                   mock_ppt, 
                                                   mock_conn, 
                                                   itoken_challenger(Of mock_ppt, mock_conn))
    Private ReadOnly token_info_creator As Func(Of forward_questioner_responder, 
                                                   UInt32, 
                                                   mock_conn, 
                                                   token_info(Of mock_ppt, mock_conn))
    Private ReadOnly with_empty_token As Boolean

    Public Sub New()
        Me.New(AddressOf token_defender.[New](Of mock_ppt, mock_conn),
               AddressOf token_challenger.[New](Of mock_ppt, mock_conn),
               Function(x, y, z) New info(x, y, z),
               True)
    End Sub

    Protected Sub New(ByVal defender_creator As Func(Of token_info(Of mock_ppt, mock_conn), 
                                                        itoken_defender(Of mock_ppt, mock_conn)),
                      ByVal challenger_creator As Func(Of token_info(Of mock_ppt, mock_conn), 
                                                          mock_ppt, 
                                                          mock_conn, 
                                                          itoken_challenger(Of mock_ppt, mock_conn)),
                      ByVal token_info_creator As Func(Of forward_questioner_responder, 
                                                          UInt32, 
                                                          mock_conn, 
                                                          token_info(Of mock_ppt, mock_conn)),
                      ByVal with_empty_token As Boolean)
        assert(Not defender_creator Is Nothing)
        assert(Not challenger_creator Is Nothing)
        assert(Not token_info_creator Is Nothing)
        Me.defender_creator = defender_creator
        Me.challenger_creator = challenger_creator
        Me.token_info_creator = token_info_creator
        forward_questioner_responder = New forward_questioner_responder(concurrency_connection)
        Me.with_empty_token = with_empty_token
    End Sub

    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Private Shared Function defend_event_comb(ByVal d As itoken_defender(Of mock_ppt, mock_conn),
                                              ByVal ppts() As mock_ppt,
                                              ByVal exp_accepted As Boolean,
                                              ByVal with_empty_ppt As Boolean,
                                              ByVal ppt As mock_ppt,
                                              ByVal conn As mock_conn) As event_comb
        ' Usually we should not use the same ppt and conn instance.
        Dim selected_ppt As ref(Of mock_ppt) = Nothing
        Dim accept_ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  selected_ppt = New ref(Of mock_ppt)()
                                  selected_ppt.set(ppts(rnd_int(0, array_size(ppts))))
                                  accept_ec = d(conn, selected_ppt)
                                  Return waitfor(accept_ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(accept_ec.end_result())
                                  'If with_empty_ppt Then
                                  '    If assertion.is_not_null(+selected_ppt) Then
                                  '        assertion.is_true(object_compare(+selected_ppt, ppt) OrElse
                                  '                    String.IsNullOrEmpty((+selected_ppt).token))
                                  '    End If
                                  'Else
                                  If exp_accepted Then
                                      assertion.reference_equal(+selected_ppt, ppt)
                                  Else
                                      assertion.is_null(+selected_ppt)
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function challenge_event_comb(ByVal c As itoken_challenger,
                                                 ByVal exp_accepted As Boolean,
                                                 ByVal with_empty_ppt As Boolean,
                                                 ByVal conn As mock_conn) As event_comb
        Dim challenge_ec As event_comb = Nothing
        Dim challenge_accepted As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  challenge_accepted = New ref(Of Boolean)()
                                  challenge_accepted.set(rnd_bool())
                                  challenge_ec = c(challenge_accepted)
                                  Return waitfor(challenge_ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(challenge_ec.end_result())
                                  assertion.equal(+challenge_accepted, exp_accepted)
                                  If Not +challenge_accepted Then
                                      assertion.is_true(conn.closed())
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Function execute(ByVal d As itoken_defender(Of mock_ppt, mock_conn),
                             ByVal with_empty_ppt As Boolean,
                             ByVal id As UInt32,
                             ByVal ppts() As mock_ppt) As event_comb
        Dim round As Int32 = 0
        Dim aec As event_comb = Nothing
        Dim dec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If round < 1023 Then
                                      round += 1
                                      Return goto_next()
                                  Else
                                      Return goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  forward_questioner_responder.reset(id)
                                  Dim ppt As mock_ppt = Nothing
                                  ppt = ppts(rnd_int(0, array_size(ppts)))
                                  Dim exp_accepted As Boolean = False
                                  exp_accepted = rnd_bool()
                                  Dim conn As mock_conn = Nothing
                                  conn = If(exp_accepted, mock_conn.create_good(ppt), mock_conn.create_bad(ppts))
                                  Dim c As itoken_challenger = Nothing
                                  c = challenger_creator(token_info_creator(forward_questioner_responder, id, conn),
                                                         ppt,
                                                         conn)
                                  aec = defend_event_comb(d, ppts, exp_accepted, with_empty_ppt, ppt, conn)
                                  dec = challenge_event_comb(c, exp_accepted, with_empty_ppt, conn)
                                  Return waitfor(aec, dec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assertion.is_true(aec.end_result())
                                  assertion.is_true(dec.end_result())
                                  Return goto_begin()
                              End Function)
    End Function

    Private Function execute(ByVal id As UInt32,
                             ByVal with_empty_token As Boolean,
                             ByVal finish_count As count_event) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim d As itoken_defender(Of mock_ppt, mock_conn) = Nothing
                                  d = defender_creator(token_info_creator(forward_questioner_responder, id, Nothing))
                                  Dim ppts() As mock_ppt = Nothing
                                  ppts = mock_ppt.create(rnd_uint(mock_ppt_count_lower_bound,
                                                                  mock_ppt_count_upper_bound),
                                                         with_empty_token:=with_empty_token)
                                  For i As UInt32 = uint32_0 To array_size(ppts) - uint32_1
                                      d.attach(ppts(i))
                                  Next
                                  ec = execute(d, with_empty_token, id, ppts)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  assert(ec.end_result())
                                  finish_count.decrement()
                                  Return goto_end()
                              End Function)
    End Function

    Private Function run_case(ByVal with_empty_token As Boolean) As Boolean
        Dim c As count_event = Nothing
        c = New count_event(concurrency_connection)
        For i As UInt32 = uint32_0 To concurrency_connection - uint32_1
            assert_begin(execute(i, with_empty_token, c))
        Next
        assertion.is_true(c.wait(minutes_to_milliseconds(10)))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        If Not run_case(False) Then
            Return False
        End If
        If with_empty_token AndAlso Not run_case(True) Then
            Return False
        End If
        Return True
    End Function
End Class
