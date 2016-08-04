
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class event_comb
    Public Shared Function failed() As event_comb
        Return New event_comb(Function() As Boolean
                                  Return False
                              End Function)
    End Function

    Public Shared Function succeeded() As event_comb
        Return New event_comb(Function() As Boolean
                                  Return goto_end()
                              End Function)
    End Function

    Public Shared Function [return](ByVal d As Func(Of event_comb)) As event_comb
        assert(Not d Is Nothing)
        Return do_(Function() As event_comb
                       Dim rtn As event_comb = Nothing
                       rtn = d()
                       If rtn Is Nothing Then
                           Return event_comb.succeeded()
                       Else
                           Return rtn
                       End If
                   End Function,
                   event_comb.failed())
    End Function

    Public Shared Function [return](ByVal v As void(Of event_comb)) As event_comb
        assert(Not v Is Nothing)
        Return [return](Function() As event_comb
                            Dim r As event_comb = Nothing
                            v(r)
                            Return r
                        End Function)
    End Function

    Private Shared Function repeat(ByVal c As Func(Of Int32, Boolean),
                                   ByVal l As Func(Of Int32, Boolean),
                                   ByVal w As Func(Of Boolean)) As event_comb
        Dim i As Int32 = 0
        Return New event_comb(Function() As Boolean
                                  If w Is Nothing Then
                                      Return False
                                  End If
                                  If Not l Is Nothing AndAlso Not l(i) Then
                                      Return False
                                  End If
                                  If c(i) Then
                                      i += 1
                                      Return w()
                                  Else
                                      Return goto_end()
                                  End If
                              End Function)
    End Function

    Private Shared Function repeat(ByVal c As Int32,
                                   ByVal l As Func(Of Int32, Boolean),
                                   ByVal w As Func(Of Boolean)) As event_comb
        Return repeat(Function(i) i < c, l, w)
    End Function

    Public Shared Function repeat(ByVal c As Int32, ByVal w As Func(Of Boolean)) As event_comb
        Return repeat(c, Nothing, w)
    End Function

    Public Shared Function repeat(ByVal c As Func(Of Int32, Boolean), ByVal w As Func(Of Boolean)) As event_comb
        Return repeat(c, Nothing, w)
    End Function

    Public Shared Function repeat(ByVal c As Func(Of Int32, Boolean), ByVal d As Func(Of event_comb)) As event_comb
        Dim ec As event_comb = Nothing
        Return repeat(c,
                      Function(i As Int32) As Boolean
                          Return i = 0 OrElse
                                 (assert(Not ec Is Nothing) AndAlso ec.end_result())
                      End Function,
                      Function() As Boolean
                          If d Is Nothing Then
                              Return False
                          Else
                              ec = d()
                          End If
                          Return waitfor(ec)
                      End Function)
    End Function

    Public Shared Function repeat(ByVal c As Int32, ByVal d As Func(Of event_comb)) As event_comb
        Return repeat(Function(i) i < c, d)
    End Function

    Public Shared Function [while](ByVal condition As Func(Of event_comb, 
                                                              pointer(Of Boolean), 
                                                              event_comb),
                                   ByVal execution As Func(Of event_comb)) As event_comb
        Dim cec As event_comb = Nothing
        Dim ec As event_comb = Nothing
        Dim break_error As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  If execution Is Nothing Then
                                      Return False
                                  Else
                                      Return goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If condition Is Nothing Then
                                      cec = sync_async(Function() As Boolean
                                                           Return ec.end_result_or_null()
                                                       End Function)
                                  Else
                                      break_error.renew()
                                      cec = condition(ec, break_error)
                                  End If
                                  Return waitfor(cec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If Not break_error Is Nothing AndAlso
                                     (+break_error) Then
                                      Return False
                                  ElseIf cec.end_result() Then
                                      ec = execution()
                                      Return waitfor(ec) AndAlso
                                             goto_prev()
                                  Else
                                      Return goto_end()
                                  End If
                              End Function)
    End Function

    Public Shared Function [while](ByVal execution As Func(Of event_comb),
                                   ByVal condition As Func(Of event_comb, 
                                                              pointer(Of Boolean), 
                                                              event_comb)) As event_comb
        Dim cec As event_comb = Nothing
        Dim ec As event_comb = Nothing
        Dim break_error As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  If execution Is Nothing Then
                                      Return False
                                  Else
                                      Return goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If Not break_error Is Nothing AndAlso
                                     (+break_error) Then
                                      Return False
                                  ElseIf cec.end_result_or_null() Then
                                      ec = execution()
                                  Else
                                      Return goto_end()
                                  End If
                                  assert(Not ec Is Nothing)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If condition Is Nothing Then
                                      cec = sync_async(Function() As Boolean
                                                           Return ec.end_result()
                                                       End Function)
                                  Else
                                      break_error.renew()
                                      cec = condition(ec, break_error)
                                  End If
                                  Return waitfor(cec) AndAlso
                                         goto_prev()
                              End Function)
    End Function

    Private Shared Function condition_adapter(ByVal c As _do_val_ref(Of event_comb, Boolean, Boolean)) _
                                             As Func(Of event_comb, pointer(Of Boolean), event_comb)
        If c Is Nothing Then
            Return Nothing
        Else
            Return Function(ec As event_comb,
                            break_error As pointer(Of Boolean)) As event_comb
                       assert(Not c Is Nothing)
                       Return sync_async(Function() As Boolean
                                             Dim b As Boolean = False
                                             Return c(ec, b) AndAlso
                                                    eva(break_error, b)
                                         End Function)
                   End Function
        End If
    End Function

    Public Shared Function [while](ByVal condition As _do_val_ref(Of event_comb, Boolean, Boolean),
                                   ByVal execution As Func(Of event_comb)) As event_comb
        Return [while](condition_adapter(condition), execution)
    End Function

    Public Shared Function [while](ByVal execution As Func(Of event_comb),
                                   ByVal condition As _do_val_ref(Of event_comb, Boolean, Boolean)) As event_comb
        Return [while](execution, condition_adapter(condition))
    End Function

    Public Shared Function return_true(ByVal ec As event_comb) As event_comb
        Return New event_comb(Function() As Boolean
                                  Return waitfor(ec) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
