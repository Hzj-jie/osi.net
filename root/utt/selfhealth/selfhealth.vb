
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Public NotInheritable Class selfhealth
    Private Shared ReadOnly selectors As vector(Of String) =
            vector.emplace_of(GetType(failure_case).AssemblyQualifiedName())
    Private Shared self_health As Boolean = False

    Public Shared Function in_stage() As Boolean
        Return self_health
    End Function

    Public Shared Function run() As Boolean
        utt_raise_error("start selfhealth check")
        self_health = True
        host.run(selectors)
        self_health = False

        ' TODO: Test expectation.
        If assertion.equal(assertion.failure_count(), failure_case.expected_failure_count) OrElse
           envs.utt_no_assert Then
            assertion.clear_failure()
            assertion.equal(assertion.failure_count(), 0)
        Else
            Return False
        End If

        utt_raise_error("finished selfhealth check")
        Return True
    End Function

    Private Sub New()
    End Sub
End Class
