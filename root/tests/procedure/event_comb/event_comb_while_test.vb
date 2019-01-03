
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt

Public Class event_comb_while_test
    Inherits [case]

    Private Shared Function case1_true() As Boolean
        Const round As Int32 = 100
        Dim i As Int32 = 0
        Dim j As Int32 = 0
        assertion.is_true(async_sync(event_comb.while(Function(last_ec As event_comb,
                                                         error_break As pointer(Of Boolean)) As event_comb
                                                    Return sync_async(Function() As Boolean
                                                                          If i > 0 Then
                                                                              If assertion.is_not_null(last_ec) Then
                                                                                  assertion.is_true(last_ec.end_result())
                                                                              End If
                                                                          Else
                                                                              assertion.is_null(last_ec)
                                                                          End If
                                                                          assertion.is_not_null(error_break)
                                                                          assertion.is_false(+error_break)
                                                                          i += 1
                                                                          Return i < round
                                                                      End Function)
                                                End Function,
                                                Function() As event_comb
                                                    Return sync_async(Sub()
                                                                          j += 1
                                                                      End Sub)
                                                End Function)))
        assertion.equal(i, j + 1)
        assertion.equal(i, round)
        Return True
    End Function

    Private Shared Function case1_false() As Boolean
        Const round As Int32 = 100
        Dim i As Int32 = 0
        Dim j As Int32 = 0
        assertion.is_false(async_sync(event_comb.while(Function(last_ec As event_comb,
                                                          error_break As pointer(Of Boolean)) As event_comb
                                                     Return sync_async(Function() As Boolean
                                                                           If i > 0 Then
                                                                               If assertion.is_not_null(last_ec) Then
                                                                                   assertion.is_true(last_ec.end_result())
                                                                               End If
                                                                           Else
                                                                               assertion.is_null(last_ec)
                                                                           End If
                                                                           i += 1
                                                                           assertion.is_not_null(error_break)
                                                                           assertion.is_false(+error_break)
                                                                           eva(error_break, i = round)
                                                                           Return True
                                                                       End Function)
                                                 End Function,
                                                 Function() As event_comb
                                                     Return sync_async(Sub()
                                                                           j += 1
                                                                       End Sub)
                                                 End Function)))
        assertion.equal(i, j + 1)
        assertion.equal(i, round)
        Return True
    End Function

    Private Shared Function case1() As Boolean
        Return case1_true() AndAlso
               case1_false()
    End Function

    Private Shared Function case2_true() As Boolean
        Const round As Int32 = 100
        Dim i As Int32 = 0
        Dim j As Int32 = 0
        assertion.is_true(async_sync(event_comb.while(Function() As event_comb
                                                    Return sync_async(Sub()
                                                                          j += 1
                                                                      End Sub)
                                                End Function,
                                                Function(last_ec As event_comb,
                                                         error_break As pointer(Of Boolean)) As event_comb
                                                    Return sync_async(Function() As Boolean
                                                                          If assertion.is_not_null(last_ec) Then
                                                                              assertion.is_true(last_ec.end_result())
                                                                          End If
                                                                          assertion.is_not_null(error_break)
                                                                          assertion.is_false(+error_break)
                                                                          i += 1
                                                                          Return i < round
                                                                      End Function)
                                                End Function)))
        assertion.equal(i, j)
        assertion.equal(i, round)
        Return True
    End Function

    Private Shared Function case2_false() As Boolean
        Const round As Int32 = 100
        Dim i As Int32 = 0
        Dim j As Int32 = 0
        assertion.is_false(async_sync(event_comb.while(Function() As event_comb
                                                     Return sync_async(Sub()
                                                                           j += 1
                                                                       End Sub)
                                                 End Function,
                                                 Function(last_ec As event_comb,
                                                          error_break As pointer(Of Boolean)) As event_comb
                                                     Return sync_async(Function() As Boolean
                                                                           If assertion.is_not_null(last_ec) Then
                                                                               assertion.is_true(last_ec.end_result())
                                                                           End If
                                                                           assertion.is_not_null(error_break)
                                                                           assertion.is_false(+error_break)
                                                                           i += 1
                                                                           eva(error_break, i = round)
                                                                           Return True
                                                                       End Function)
                                                 End Function)))
        assertion.equal(i, j)
        assertion.equal(i, round)
        Return True
    End Function

    Private Shared Function case2() As Boolean
        Return case2_true() AndAlso
               case2_false()
    End Function

    Private Shared Function case3_true() As Boolean
        Const round As Int32 = 100
        Dim i As Int32 = 0
        Dim j As Int32 = 0
        assertion.is_true(async_sync(event_comb.while(Function(last_ec As event_comb,
                                                         ByRef error_break As Boolean) As Boolean
                                                    If i > 0 Then
                                                        If assertion.is_not_null(last_ec) Then
                                                            assertion.is_true(last_ec.end_result())
                                                        End If
                                                    Else
                                                        assertion.is_null(last_ec)
                                                    End If
                                                    assertion.is_false(error_break)
                                                    i += 1
                                                    Return i < round
                                                End Function,
                                                Function() As event_comb
                                                    Return sync_async(Sub()
                                                                          j += 1
                                                                      End Sub)
                                                End Function)))
        assertion.equal(i, j + 1)
        assertion.equal(i, round)
        Return True
    End Function

    Private Shared Function case3_false() As Boolean
        Const round As Int32 = 100
        Dim i As Int32 = 0
        Dim j As Int32 = 0
        assertion.is_false(async_sync(event_comb.while(Function(last_ec As event_comb,
                                                          ByRef error_break As Boolean) As Boolean
                                                     If i > 0 Then
                                                         If assertion.is_not_null(last_ec) Then
                                                             assertion.is_true(last_ec.end_result())
                                                         End If
                                                     Else
                                                         assertion.is_null(last_ec)
                                                     End If
                                                     i += 1
                                                     assertion.is_false(error_break)
                                                     error_break = (i = round)
                                                     Return True
                                                 End Function,
                                                 Function() As event_comb
                                                     Return sync_async(Sub()
                                                                           j += 1
                                                                       End Sub)
                                                 End Function)))
        assertion.equal(i, j + 1)
        assertion.equal(i, round)
        Return True
    End Function

    Private Shared Function case3() As Boolean
        Return case3_true() AndAlso
               case3_false()
    End Function

    Private Shared Function case4_true() As Boolean
        Const round As Int32 = 100
        Dim i As Int32 = 0
        Dim j As Int32 = 0
        assertion.is_true(async_sync(event_comb.while(Function() As event_comb
                                                    Return sync_async(Sub()
                                                                          j += 1
                                                                      End Sub)
                                                End Function,
                                                Function(last_ec As event_comb,
                                                         ByRef error_break As Boolean) As Boolean
                                                    If assertion.is_not_null(last_ec) Then
                                                        assertion.is_true(last_ec.end_result())
                                                    End If
                                                    assertion.is_false(error_break)
                                                    i += 1
                                                    Return i < round
                                                End Function)))
        assertion.equal(i, j)
        assertion.equal(i, round)
        Return True
    End Function

    Private Shared Function case4_false() As Boolean
        Const round As Int32 = 100
        Dim i As Int32 = 0
        Dim j As Int32 = 0
        assertion.is_false(async_sync(event_comb.while(Function() As event_comb
                                                     Return sync_async(Sub()
                                                                           j += 1
                                                                       End Sub)
                                                 End Function,
                                                 Function(last_ec As event_comb,
                                                          ByRef error_break As Boolean) As Boolean
                                                     If assertion.is_not_null(last_ec) Then
                                                         assertion.is_true(last_ec.end_result())
                                                     End If
                                                     assertion.is_false(error_break)
                                                     i += 1
                                                     error_break = (i = round)
                                                     Return True
                                                 End Function)))
        assertion.equal(i, j)
        assertion.equal(i, round)
        Return True
    End Function

    Private Shared Function case4() As Boolean
        Return case4_true() AndAlso
               case4_false()
    End Function

    Public Overrides Function run() As Boolean
        Return case1() AndAlso
               case2() AndAlso
               case3() AndAlso
               case4()
    End Function
End Class
