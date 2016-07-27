
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.constants

Partial Public Class callback_action
    Public Const check_pass As Boolean = True
    Public Const check_stay As Boolean = False
    Public Shared ReadOnly step_check_pass As ternary = ternary.true
    Public Shared ReadOnly step_check_stay As ternary = ternary.unknown
    Public Shared ReadOnly step_check_finish As ternary = ternary.false

    Private Shared Function _step_sub_check(ByVal step_check_pass_action As _do(Of Int64, ternary),
                                            ByVal ParamArray d() As Func(Of ternary)) As Func(Of ternary)
        assert(Not step_check_pass_action Is Nothing)
        If d Is Nothing OrElse d.Length() = 0 Then
            Return todo(step_check_pass)
        Else
            Dim i As Int64 = 0
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
        Return _step_sub_check(Function(ByRef i As Int64) As ternary
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
        Return _step_sub_check(Function(ByRef i As Int64) As ternary
                                   i += 1
                                   If i = d.Length() Then
                                       i = 0
                                   End If
                                   Return step_check_stay
                               End Function,
                               d)
    End Function

    Private Shared Function _action_check(Of T)(ByVal lastAction As Func(Of callback_action),
                                                ByVal thisAction As Func(Of Boolean),
                                                ByVal failureAction As Action,
                                                ByVal successReturn As T,
                                                ByVal failureReturn As T,
                                                ByVal stayReturn As T) As Func(Of T)
        If lastAction Is Nothing Then
            raise_error(error_type.exclamation,
                        "lastAction is nothing in _action_check, callstack ", connector.callstack())
            Return todo(successReturn)
        Else
            'callbackManager.checkqueue is single threading model
            Return Function() As T
                       If lastAction() Is Nothing Then
                           raise_error(error_type.exclamation,
                                       "lastAction() is nothing in _action_check.Function(), callstack ",
                                       current().callstack())
                           Return successReturn
                       Else
                           If lastAction().finished() Then
                               If lastAction().end_result().true_() Then
                                   If thisAction Is Nothing OrElse do_(thisAction, False) Then
                                       Return successReturn
                                   Else
                                       Return failureReturn
                                   End If
                               Else
                                   If failureAction Is Nothing Then
                                       raise_error(error_type.warning,
                                                   lastAction().callstack(), ".end_result().nottrue()")
                                   Else
                                       failureAction()
                                   End If
                                   Return failureReturn
                               End If
                           Else
                               Return stayReturn
                           End If
                       End If
                   End Function
        End If
    End Function

    Private Shared Function action_check_thisAction(ByVal thisAction As Action) As Func(Of Boolean)
        Return If(thisAction Is Nothing,
                  todo(check_pass),
                  Function() As Boolean
                      void_(thisAction)
                      Return check_pass
                  End Function)
    End Function

    Private Shared Function empty_thisAction() As Func(Of Boolean)
        Return Nothing
    End Function

    Public Shared Function action_check(ByVal lastAction As Func(Of callback_action),
                                        ByVal thisAction As Func(Of Boolean),
                                        Optional ByVal failureAction As Action = Nothing) As Func(Of Boolean)
        Return _action_check(lastAction, thisAction, failureAction, check_pass, check_pass, check_stay)
    End Function

    Public Shared Function action_check(ByVal lastAction As callback_action,
                                        ByVal thisAction As Func(Of Boolean),
                                        Optional ByVal failureAction As Action = Nothing) As Func(Of Boolean)
        Return action_check(todo(lastAction), thisAction, failureAction)
    End Function

    Public Shared Function action_check(ByVal lastAction As Func(Of callback_action),
                                        ByVal thisAction As Action,
                                        Optional ByVal failureAction As Action = Nothing) As Func(Of Boolean)
        Return action_check(lastAction, action_check_thisAction(thisAction), failureAction)
    End Function

    Public Shared Function action_check(ByVal lastAction As callback_action,
                                        ByVal thisAction As Action,
                                        Optional ByVal failureAction As Action = Nothing) As Func(Of Boolean)
        Return action_check(todo(lastAction), thisAction, failureAction)
    End Function

    Public Shared Function action_check(ByVal lastAction As Func(Of callback_action)) As Func(Of Boolean)
        Return action_check(lastAction, empty_thisAction())
    End Function

    Public Shared Function action_check(ByVal lastAction As callback_action) As Func(Of Boolean)
        Return action_check(todo(lastAction))
    End Function

    Public Shared Function action_sub_check(ByVal lastAction As Func(Of callback_action),
                                            ByVal thisAction As Func(Of Boolean),
                                            Optional ByVal failureAction As Action = Nothing) As Func(Of ternary)
        Return _action_check(lastAction, thisAction, failureAction,
                             callback_action.step_check_pass,
                             callback_action.step_check_finish,
                             callback_action.step_check_stay)
    End Function

    Public Shared Function action_sub_check(ByVal lastAction As callback_action,
                                            ByVal thisAction As Func(Of Boolean),
                                            Optional ByVal failureAction As Action = Nothing) As Func(Of ternary)
        Return action_sub_check(todo(lastAction), thisAction, failureAction)
    End Function

    Public Shared Function action_sub_check(ByVal lastAction As Func(Of callback_action),
                                            ByVal thisAction As Action,
                                            Optional ByVal failureAction As Action = Nothing) As Func(Of ternary)
        Return action_sub_check(lastAction, action_check_thisAction(thisAction), failureAction)
    End Function

    Public Shared Function action_sub_check(ByVal lastAction As callback_action,
                                            ByVal thisAction As Action,
                                            Optional ByVal failureAction As Action = Nothing) As Func(Of ternary)
        Return action_sub_check(todo(lastAction), thisAction, failureAction)
    End Function

    Public Shared Function action_sub_check(ByVal lastAction As Func(Of callback_action)) As Func(Of ternary)
        Return action_sub_check(lastAction, empty_thisAction())
    End Function

    Public Shared Function action_sub_check(ByVal lastAction As callback_action) As Func(Of ternary)
        Return action_sub_check(todo(lastAction))
    End Function

    Private Shared Function _step_check(ByVal step_check_pass_action As _do(Of Int64, Boolean),
                                        ByVal ParamArray d() As Func(Of ternary)) As Func(Of Boolean)
        assert(Not step_check_pass_action Is Nothing)
        If d Is Nothing OrElse d.Length() = 0 Then
            Return todo(check_pass)
        Else
            Dim i As Int64 = 0
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
        End If
    End Function

    Public Shared Function step_check(ByVal ParamArray d() As Func(Of ternary)) As Func(Of Boolean)
        Return _step_check(Function(ByRef i As Int64) As Boolean
                               i += 1
                               If i = d.Length() Then
                                   Return check_pass
                               Else
                                   Return check_stay
                               End If
                           End Function,
                           d)
    End Function

    Public Shared Function rotate_check(ByVal ParamArray d() As Func(Of ternary)) As Func(Of Boolean)
        Return _step_check(Function(ByRef i As Int64) As Boolean
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
        Else
            Return Function() As Boolean
                       For i As Int64 = 0 To d.Length() - 1
                           If do_(d(i), check_pass) = expect Then
                               Return expect
                           End If
                       Next

                       Return Not expect
                   End Function
        End If
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
