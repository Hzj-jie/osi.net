
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation

Partial Public Class callback_action
    Public Const check_pass As Boolean = True
    Public Const check_stay As Boolean = False
    Public Shared ReadOnly step_check_pass As ternary = ternary.true
    Public Shared ReadOnly step_check_stay As ternary = ternary.unknown
    Public Shared ReadOnly step_check_finish As ternary = ternary.false

    Private Shared Function _step_sub_check(ByVal step_check_pass_action As _do(Of Int32, ternary),
                                            ByVal ParamArray d() As Func(Of ternary)) As Func(Of ternary)
        assert(Not step_check_pass_action Is Nothing)
        If d Is Nothing OrElse d.Length() = 0 Then
            Return todo(step_check_pass)
        Else
            Dim i As Int32 = 0
            Return Function() As ternary
                       Dim rtn As ternary = Nothing
                       rtn = do_(d(i), step_check_finish)
                       Dim rtn2 As ternary = Nothing
                       Select Case rtn
                           Case step_check_pass
                               rtn2 = step_check_pass_action(i)
                           Case step_check_stay
                               rtn2 = step_check_stay
                           Case step_check_finish
                               rtn2 = step_check_pass
                           Case Else
                               assert(False)
                               rtn2 = rtn
                       End Select
                       assert(rtn2 <> step_check_finish)
                       Return rtn2
                   End Function
        End If
    End Function

    Public Shared Function step_sub_check(ByVal ParamArray d() As Func(Of ternary)) As Func(Of ternary)
        Return _step_sub_check(Function(ByRef i As Int32) As ternary
                                   i += 1
                                   If i = d.Length() Then
                                       Return step_check_pass
                                   Else
                                       Return step_check_stay
                                   End If
                               End Function,
                               d)
    End Function

    Public Shared Function rotate_sub_check(ByVal ParamArray d() As Func(Of ternary)) As Func(Of ternary)
        Return _step_sub_check(Function(ByRef i As Int32) As ternary
                                   i += 1
                                   If i = d.Length() Then
                                       i = 0
                                   End If
                                   Return step_check_stay
                               End Function,
                               d)
    End Function

    Private Shared Function _action_check(Of T)(ByVal last_action As Func(Of callback_action),
                                                ByVal this_action As Func(Of Boolean),
                                                ByVal failure_action As Action,
                                                ByVal success_return As T,
                                                ByVal failure_return As T,
                                                ByVal stay_return As T) As Func(Of T)
        If last_action Is Nothing Then
            raise_error(error_type.exclamation,
                        "last_action is nothing in _action_check, callstack ", connector.callstack())
            Return todo(success_return)
        End If
        'callbackManager.checkqueue is single threading model
        Return Function() As T
                   If last_action() Is Nothing Then
                       raise_error(error_type.exclamation,
                                       "last_action() is nothing in _action_check.Function(), callstack ",
                                       current().callstack())
                       Return success_return
                   Else
                       If last_action().finished() Then
                           If last_action().end_result().true_() Then
                               If this_action Is Nothing OrElse do_(this_action, False) Then
                                   Return success_return
                               Else
                                   Return failure_return
                               End If
                           Else
                               If failure_action Is Nothing Then
                                   raise_error(error_type.warning,
                                                   last_action().callstack(), ".end_result().nottrue()")
                               Else
                                   failure_action()
                               End If
                               Return failure_return
                           End If
                       Else
                           Return stay_return
                       End If
                   End If
               End Function
    End Function

    Private Shared Function action_check_this_action(ByVal this_action As Action) As Func(Of Boolean)
        Return If(this_action Is Nothing,
                  todo(check_pass),
                  Function() As Boolean
                      void_(this_action)
                      Return check_pass
                  End Function)
    End Function

    Private Shared Function empty_this_action() As Func(Of Boolean)
        Return Nothing
    End Function

    Public Shared Function action_check(ByVal last_action As Func(Of callback_action),
                                        ByVal this_action As Func(Of Boolean),
                                        Optional ByVal failure_action As Action = Nothing) As Func(Of Boolean)
        Return _action_check(last_action, this_action, failure_action, check_pass, check_pass, check_stay)
    End Function

    Public Shared Function action_check(ByVal last_action As callback_action,
                                        ByVal this_action As Func(Of Boolean),
                                        Optional ByVal failure_action As Action = Nothing) As Func(Of Boolean)
        Return action_check(todo(last_action), this_action, failure_action)
    End Function

    Public Shared Function action_check(ByVal last_action As Func(Of callback_action),
                                        ByVal this_action As Action,
                                        Optional ByVal failure_action As Action = Nothing) As Func(Of Boolean)
        Return action_check(last_action, action_check_this_action(this_action), failure_action)
    End Function

    Public Shared Function action_check(ByVal last_action As callback_action,
                                        ByVal this_action As Action,
                                        Optional ByVal failure_action As Action = Nothing) As Func(Of Boolean)
        Return action_check(todo(last_action), this_action, failure_action)
    End Function

    Public Shared Function action_check(ByVal last_action As Func(Of callback_action)) As Func(Of Boolean)
        Return action_check(last_action, empty_this_action())
    End Function

    Public Shared Function action_check(ByVal last_action As callback_action) As Func(Of Boolean)
        Return action_check(todo(last_action))
    End Function

    Public Shared Function action_sub_check(ByVal last_action As Func(Of callback_action),
                                            ByVal this_action As Func(Of Boolean),
                                            Optional ByVal failure_action As Action = Nothing) As Func(Of ternary)
        Return _action_check(last_action, this_action, failure_action,
                             callback_action.step_check_pass,
                             callback_action.step_check_finish,
                             callback_action.step_check_stay)
    End Function

    Public Shared Function action_sub_check(ByVal last_action As callback_action,
                                            ByVal this_action As Func(Of Boolean),
                                            Optional ByVal failure_action As Action = Nothing) As Func(Of ternary)
        Return action_sub_check(todo(last_action), this_action, failure_action)
    End Function

    Public Shared Function action_sub_check(ByVal last_action As Func(Of callback_action),
                                            ByVal this_action As Action,
                                            Optional ByVal failure_action As Action = Nothing) As Func(Of ternary)
        Return action_sub_check(last_action, action_check_this_action(this_action), failure_action)
    End Function

    Public Shared Function action_sub_check(ByVal last_action As callback_action,
                                            ByVal this_action As Action,
                                            Optional ByVal failure_action As Action = Nothing) As Func(Of ternary)
        Return action_sub_check(todo(last_action), this_action, failure_action)
    End Function

    Public Shared Function action_sub_check(ByVal last_action As Func(Of callback_action)) As Func(Of ternary)
        Return action_sub_check(last_action, empty_this_action())
    End Function

    Public Shared Function action_sub_check(ByVal last_action As callback_action) As Func(Of ternary)
        Return action_sub_check(todo(last_action))
    End Function

    Private Shared Function _step_check(ByVal step_check_pass_action As _do(Of Int32, Boolean),
                                        ByVal ParamArray d() As Func(Of ternary)) As Func(Of Boolean)
        assert(Not step_check_pass_action Is Nothing)
        If d Is Nothing OrElse d.Length() = 0 Then
            Return todo(check_pass)
        End If
        Dim i As Int32 = 0
        Return Function() As Boolean
                   Dim rtn As ternary = Nothing
                   rtn = do_(d(i), step_check_finish)
                   Select Case rtn
                       Case step_check_pass
                           Return step_check_pass_action(i)
                       Case step_check_finish
                           Return check_pass
                       Case step_check_stay
                           Return check_stay
                       Case Else
                           assert(False)
                           Return check_pass
                   End Select
               End Function
    End Function

    Public Shared Function step_check(ByVal ParamArray d() As Func(Of ternary)) As Func(Of Boolean)
        Return _step_check(Function(ByRef i As Int32) As Boolean
                               i += 1
                               If i = d.Length() Then
                                   Return check_pass
                               End If
                               Return check_stay
                           End Function,
                           d)
    End Function

    Public Shared Function rotate_check(ByVal ParamArray d() As Func(Of ternary)) As Func(Of Boolean)
        Return _step_check(Function(ByRef i As Int32) As Boolean
                               i += 1
                               If i = d.Length() Then
                                   i = 0
                               End If
                               Return check_stay
                           End Function,
                           d)
    End Function

    Private Shared Function multi_check(ByVal expect As Boolean,
                                        ByVal ParamArray d() As Func(Of Boolean)) As Func(Of Boolean)
        If d Is Nothing OrElse d.Length() = 0 Then
            Return todo(check_pass)
        End If
        Return Function() As Boolean
                   For i As Int32 = 0 To d.Length() - 1
                       If do_(d(i), check_pass) = expect Then
                           Return expect
                       End If
                   Next

                   Return Not expect
               End Function
    End Function

    Public Shared Function or_check(ByVal ParamArray d() As Func(Of Boolean)) As Func(Of Boolean)
        Return multi_check(check_pass, d)
    End Function

    Public Shared Function and_check(ByVal ParamArray d() As Func(Of Boolean)) As Func(Of Boolean)
        Return multi_check(check_stay, d)
    End Function

    Public Shared Function true_to_pass(ByVal c As Boolean) As Boolean
        Return If(c, check_pass, check_stay)
    End Function

    Public Shared Function false_to_pass(ByVal c As Boolean) As Boolean
        Return true_to_pass(Not c)
    End Function

    Public Shared Function pass() As Func(Of Boolean)
        Return todo(check_pass)
    End Function

    Public Shared Function true_to_pass(ByVal d As Func(Of Boolean)) As Func(Of Boolean)
        If d Is Nothing Then
            Return pass()
        Else
            Return Function() As Boolean
                       Return true_to_pass(do_(d, True))
                   End Function
        End If
    End Function

    Public Shared Function false_to_pass(ByVal d As Func(Of Boolean)) As Func(Of Boolean)
        If d Is Nothing Then
            Return pass()
        Else
            Return Function() As Boolean
                       Return false_to_pass(do_(d, False))
                   End Function
        End If
    End Function
End Class
