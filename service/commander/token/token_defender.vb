
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utils
Imports osi.root.procedure
Imports osi.root.formation

Public NotInheritable Class token_defender
    Public Shared Function [New](Of COLLECTION As Class, CONNECTION) _
                                (ByVal powerpoints As object_unique_set(Of COLLECTION),
                                 ByVal info As token_info(Of COLLECTION, CONNECTION),
                                 Optional ByVal max_connecting As UInt32 = uint32_0,
                                 Optional ByVal accepting_over_max_connecting_wait_ms As Int64 = int64_0) _
                                As token_defender(Of COLLECTION, CONNECTION)
        Return New token_defender(Of COLLECTION, CONNECTION)(powerpoints,
                                                             info,
                                                             max_connecting,
                                                             accepting_over_max_connecting_wait_ms)
    End Function

    Public Shared Function [New](Of COLLECTION As Class, CONNECTION) _
                                (ByVal info As token_info(Of COLLECTION, CONNECTION),
                                 Optional ByVal max_connecting As UInt32 = uint32_0,
                                 Optional ByVal accepting_over_max_connecting_wait_ms As Int64 = int64_0) _
                                As token_defender(Of COLLECTION, CONNECTION)
        Return New token_defender(Of COLLECTION, CONNECTION)(info,
                                                             max_connecting,
                                                             accepting_over_max_connecting_wait_ms)
    End Function

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class token_defender(Of COLLECTION As Class, CONNECTION)
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

    Private Overloads Function verify_token(ByVal r As pointer(Of COLLECTION)) As executor
        Return New executor_wrapper(Function(i() As Byte, o As pointer(Of Byte())) As event_comb
                                        Return sync_async(
                                            Sub()
                                                Dim p As piece = Nothing
                                                p = New piece(i)
                                                If p.start_with(constants.token1_prefix) Then
                                                    If search_for_token(p.consume(constants.token1_prefix_len), r) Then
                                                        eva(o, i)
                                                    Else
                                                        eva(o, array_concat(str_bytes("reject "), i))
                                                    End If
                                                End If
                                            End Sub)
                                    End Function)
    End Function

    Protected Overrides Function verify_token(ByVal h As herald,
                                              ByVal p As COLLECTION,
                                              ByVal r As pointer(Of COLLECTION)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = +(New responder(Of _false)(h,
                                                                  info.response_timeout_ms(p),
                                                                  verify_token(r)))
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
