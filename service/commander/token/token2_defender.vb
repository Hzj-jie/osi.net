
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure

Public NotInheritable Class token2_defender
    Public Shared Function [New](Of COLLECTION As Class, CONNECTION) _
                                (ByVal powerpoints As object_unique_set(Of COLLECTION),
                                 ByVal info As token_info(Of COLLECTION, CONNECTION),
                                 Optional ByVal max_connecting As UInt32 = uint32_0,
                                 Optional ByVal accepting_over_max_connecting_wait_ms As Int64 = int64_0) _
                                As token2_defender(Of COLLECTION, CONNECTION)
        Return New token2_defender(Of COLLECTION, CONNECTION)(powerpoints,
                                                              info,
                                                              max_connecting,
                                                              accepting_over_max_connecting_wait_ms)
    End Function

    Public Shared Function [New](Of COLLECTION As Class, CONNECTION) _
                                (ByVal info As token_info(Of COLLECTION, CONNECTION),
                                 Optional ByVal max_connecting As UInt32 = uint32_0,
                                 Optional ByVal accepting_over_max_connecting_wait_ms As Int64 = int64_0) _
                                As token2_defender(Of COLLECTION, CONNECTION)
        Return New token2_defender(Of COLLECTION, CONNECTION)(info,
                                                              max_connecting,
                                                              accepting_over_max_connecting_wait_ms)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class token2_defender(Of COLLECTION As Class, CONNECTION)
    Inherits itoken_defender(Of COLLECTION, CONNECTION)

    Public Sub New(ByVal powerpoints As object_unique_set(Of COLLECTION),
                   ByVal info As token_info(Of COLLECTION, CONNECTION),
                   Optional ByVal max_connecting As UInt32 = uint32_0,
                   Optional ByVal accepting_over_max_connecting_wait_ms As Int64 = int64_0)
        MyBase.New(powerpoints, info, max_connecting, accepting_over_max_connecting_wait_ms)
    End Sub

    Public Sub New(ByVal info As token_info(Of COLLECTION, CONNECTION),
                   Optional ByVal max_connecting As UInt32 = uint32_0,
                   Optional ByVal accepting_over_max_connecting_wait_ms As Int64 = int64_0)
        MyBase.New(info, max_connecting, accepting_over_max_connecting_wait_ms)
    End Sub

    Protected Overrides Function verify_token(ByVal c As CONNECTION,
                                              ByVal h As herald,
                                              ByVal p As COLLECTION,
                                              ByVal r As pointer(Of COLLECTION)) As event_comb
        assert(Not h Is Nothing)
        assert(Not r Is Nothing)
        assert(r.empty())
        Dim cmd As pointer(Of command) = Nothing
        Dim code As piece = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  code = New piece(info.challenge_code(p))
                                  ec = h.send(command.[New]().attach(array_concat(constants.token2_prefix,
                                                                                  code.export())))
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cmd = New pointer(Of command)()
                                      ec = h.wait_and_receive(info.response_timeout_ms(p), cmd)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not cmd.empty() Then
                                      Dim reply As piece = Nothing
                                      assert(Not code Is Nothing AndAlso Not code.empty())
                                      If (+cmd).has_action() AndAlso
                                         search_for_token(Function(x As COLLECTION) As Boolean
                                                              Return info.sign_match(x,
                                                                                     code,
                                                                                     New piece((+cmd).action()))
                                                          End Function,
                                                          r) AndAlso
                                         assert(Not r.empty()) Then
                                          assert(info.sign(+r, New piece((+cmd).action()), reply))
                                      Else
                                          assert(r.empty())
                                          If info.trace(p) Then
                                              raise_error(info.identity(c), " did not send a valid signing result")
                                          End If
                                          If Not info.forge_signature(p, New piece((+cmd).action()), reply) Then
                                              reply = New piece(info.challenge_code(p))
                                          End If
                                      End If
                                      ec = h.send(command.[New]().attach(reply.export()))
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cmd.clear()
                                      ec = h.wait_and_receive(info.response_timeout_ms(p), cmd)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not cmd.empty() Then
                                      If Not (+cmd).action_is(constants.token2_prefix) Then
                                          If info.trace(p) Then
                                              raise_error(info.identity(c), " did not send a valid prefix")
                                          End If
                                          r.clear()
                                      End If
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
