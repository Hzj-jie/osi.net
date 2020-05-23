﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Public Module selfhealth
    Private self_health As Boolean = False

    Public Function self_health_stage() As Boolean
        Return self_health
    End Function

    Private Function set_self_health(ByVal b As Boolean) As Boolean
        Dim found As Boolean = False
        host.foreach(Sub(ByVal x As case_info)
                         If TypeOf x.case Is failure_case Then
                             found = True
                         End If
                     End Sub)
        Return found
    End Function

    Public Function run() As Boolean
        Dim selectors As vector(Of String) = Nothing
        selectors = New vector(Of String)()
        selectors.push_back(GetType(failure_case).AssemblyQualifiedName())

        Dim r As Int32 = 0
        r = rnd_int(1, 10)
        For i As Int32 = 0 To r - 1
            If Not set_self_health(True) Then
                Return False
            End If
            self_health = True
            host.run(selectors)
            self_health = False

            ' TODO: Test expectation.
            If (assertion.equal(assertion.failure_count(), failure_case.expected_failure_count) AndAlso
                set_self_health(False)) OrElse
               envs.utt_no_assert Then
                assertion.clear_failure()
            End If
            Return False
        Next

        Return True
    End Function
End Module
