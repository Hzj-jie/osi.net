
Option Explicit On
Option Infer Off
Option Strict On

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
        Return succeeded(action_empty)
    End Function

    Public Shared Function succeeded(ByVal a As Action) As event_comb
        assert(Not a Is Nothing)
        Return New event_comb(Function() As Boolean
                                  a()
                                  Return goto_end()
                              End Function)
    End Function

    Public Shared Function one_step(ByVal f As Func(Of Boolean)) As event_comb
        assert(Not f Is Nothing)
        Return New event_comb(Function() As Boolean
                                  Return f() AndAlso
                                         goto_end()
                              End Function)
    End Function

    ' Provide a way to chain with repeat() and call Func(of event_comb) for multiple times.
    Public Shared Function [of](ByVal f As Func(Of event_comb)) As event_comb
        assert(Not f Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = f()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    ' TODO: Remove event_comb.while
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

    Public Shared Function suppress_error(ByVal ec As event_comb) As event_comb
        Return succeeded(Sub()
                             waitfor(ec)
                         End Sub)
    End Function

    Public Function suppress_error() As event_comb
        Return suppress_error(Me)
    End Function
End Class
