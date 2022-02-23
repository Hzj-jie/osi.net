
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.delegates

Public Module _event_comb_success_behavior
    <Extension()> Public Function success_behavior(ByVal ec As event_comb,
                                                   ByVal suc As Func(Of Boolean),
                                                   ByVal fal As Action) As Boolean
        If ec IsNot Nothing AndAlso ec.success_finished() AndAlso do_(suc, False) Then
            Return True
        Else
            void_(fal)
            Return False
        End If
    End Function

    <Extension()> Public Function success_behavior(ByVal ec As event_comb,
                                                   ByVal d As Func(Of Boolean)) As Boolean
        Return success_behavior(ec, d, Nothing)
    End Function

    <Extension()> Public Function success_behavior(ByVal ec As event_comb,
                                                   ByVal suc As Action,
                                                   ByVal fal As Action) As Boolean
        Return success_behavior(ec,
                                Function() As Boolean
                                    void_(suc)
                                    Return True
                                End Function,
                                fal)
    End Function

    <Extension()> Public Function success_behavior(ByVal ec As event_comb, ByVal d As Action) As Boolean
        Return success_behavior(ec, Function() As Boolean
                                        void_(d)
                                        Return True
                                    End Function)
    End Function
End Module
