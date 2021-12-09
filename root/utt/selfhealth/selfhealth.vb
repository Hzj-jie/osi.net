
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

    Public Function run() As Boolean
        Dim selectors As New vector(Of String)()
        selectors.push_back(GetType(failure_case).AssemblyQualifiedName())

        Dim r As Int32 = rnd_int(1, 10)
        For i As Int32 = 0 To r - 1
            self_health = True
            host.run(selectors)
            self_health = False

            ' TODO: Test expectation.
            If assertion.equal(assertion.failure_count(), failure_case.expected_failure_count) OrElse
               envs.utt_no_assert Then
                assertion.clear_failure()
            Else
                Return False
            End If
        Next

        Return True
    End Function
End Module
