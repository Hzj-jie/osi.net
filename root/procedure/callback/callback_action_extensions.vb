
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _callback_action_extensions
    <Extension()> Public Function success_finished(ByVal action As callback_action) As Boolean
        Return Not action Is Nothing AndAlso
               action.finished() AndAlso
               action.begin_result().true_() AndAlso
               action.check_result().true_() AndAlso
               action.end_result().true_()
    End Function
End Module