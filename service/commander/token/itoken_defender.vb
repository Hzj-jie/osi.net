
Option Explicit On
Option Infer Off
Option Strict On

#Const bypass_empty_token = False
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils

Public MustInherit Class itoken_defender(Of COLLECTION As Class, CONNECTION)
    Public ReadOnly powerpoints As object_unique_set(Of COLLECTION)
    Protected ReadOnly info As token_info(Of COLLECTION, CONNECTION)
    Private ReadOnly max_connecting As UInt32
    Private ReadOnly connecting As atomic_int
    Private ReadOnly accepting_over_max_connecting_wait_ms As Int64

    Protected Sub New(ByVal powerpoints As object_unique_set(Of COLLECTION),
                      ByVal info As token_info(Of COLLECTION, CONNECTION),
                      Optional ByVal max_connecting As UInt32 = uint32_0,
                      Optional ByVal accepting_over_max_connecting_wait_ms As Int64 = int64_0)
        assert(Not powerpoints Is Nothing)
        assert(Not info Is Nothing)
        Me.powerpoints = powerpoints
        Me.info = info
        If max_connecting = 0 Then
            Me.max_connecting = constants.max_concurrent_connecting
        Else
            Me.max_connecting = max_connecting
        End If
        Me.connecting = New atomic_int()
        If accepting_over_max_connecting_wait_ms <= 0 Then
            Me.accepting_over_max_connecting_wait_ms = constants.accepting_over_max_connecting_wait_ms
        Else
            Me.accepting_over_max_connecting_wait_ms = accepting_over_max_connecting_wait_ms
        End If
    End Sub

    Protected Sub New(ByVal info As token_info(Of COLLECTION, CONNECTION),
                      Optional ByVal max_connecting As UInt32 = uint32_0,
                      Optional ByVal accepting_over_max_connecting_wait_ms As Int64 = int64_0)
        Me.New(New object_unique_set(Of COLLECTION)(), info, max_connecting, accepting_over_max_connecting_wait_ms)
    End Sub

    Public Sub attach(ByVal p As COLLECTION)
        assert(Not p Is Nothing)
        assert(powerpoints.insert(p))
    End Sub

    Public Sub detach(ByVal p As COLLECTION)
        assert(Not p Is Nothing)
        assert(powerpoints.erase(p))
    End Sub

    Public Function attached_powerpoint_count() As UInt32
        Return powerpoints.size()
    End Function

    Public Function empty() As Boolean
        Return attached_powerpoint_count() = uint32_0
    End Function

    Protected Overridable Function verify_token(ByVal h As herald,
                                                ByVal p As COLLECTION,
                                                ByVal r As pointer(Of COLLECTION)) As event_comb
        assert(False)
        Return Nothing
    End Function

    Protected Overridable Function verify_token(ByVal c As CONNECTION,
                                                ByVal h As herald,
                                                ByVal p As COLLECTION,
                                                ByVal r As pointer(Of COLLECTION)) As event_comb
        Return verify_token(h, p, r)
    End Function

    Protected Overridable Function verify_token(ByVal c As CONNECTION,
                                                ByVal p As COLLECTION,
                                                ByVal r As pointer(Of COLLECTION)) As event_comb
        Dim ec As event_comb = Nothing
        Dim h As herald = Nothing
        Return New event_comb(Function() As Boolean
                                  ' The responder is created from a random powerpoint, so the timeout setting
                                  ' may be different.
                                  If info.create_responder_herald(p, c, h) Then
                                      ec = verify_token(c, h, p, r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Protected Function search_for_token(ByVal match As Func(Of COLLECTION, Boolean),
                                        ByVal c As pointer(Of COLLECTION)) As Boolean
        assert(Not match Is Nothing)
        Dim selected As COLLECTION = Nothing
        powerpoints.foreach(Sub(ByVal current As COLLECTION)
                                assert(Not current Is Nothing)
                                If match(current) Then
                                    selected = current
                                    break_lambda.at_here()
                                End If
                            End Sub)
        eva(c, selected)
        Return Not selected Is Nothing
    End Function

    Protected Function search_for_token(ByVal challenge As piece, ByVal c As pointer(Of COLLECTION)) As Boolean
        Return search_for_token(Function(current As COLLECTION) As Boolean
                                    Return challenge.compare(info.token(current)) = 0
                                End Function,
                                c)
    End Function

    Private Shared Function bypass_empty_token() As Boolean
#If bypass_empty_token Then
        Return True
#Else
        Return False
#End If
    End Function

    Private Function accepting(ByVal c As CONNECTION, ByVal r As pointer(Of COLLECTION)) As event_comb
        assert(Not r Is Nothing)
        Dim p As COLLECTION = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If bypass_empty_token() AndAlso search_for_token(piece.blank, r) Then
                                      Return goto_end()
                                  Else
                                      If powerpoints.get(uint32_0, p) Then
                                          ec = verify_token(c, p, r)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return False
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+r) Is Nothing AndAlso info.trace(p) Then
                                          raise_error("cannot find the assosiated powerpoint ",
                                                      "to insert the connection, close the connection ",
                                                      info.identity(c))
                                      End If
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Default Public ReadOnly Property accept(ByVal c As CONNECTION, ByVal r As pointer(Of COLLECTION)) As event_comb
        Get
            assert(Not c Is Nothing)
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      If max_connecting > 0 AndAlso connecting.increment() > max_connecting Then
                                          connecting.decrement()
                                          Return waitfor(accepting_over_max_connecting_wait_ms)
                                      Else
                                          r.renew()
                                          ec = accepting(c, r)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      assert(connecting.decrement() >= 0)
                                      If Not ec.end_result() OrElse +r Is Nothing Then
                                          info.shutdown(c)
                                      End If
                                      Return ec.end_result() AndAlso
                                             goto_end()
                                  End Function)
        End Get
    End Property
End Class
