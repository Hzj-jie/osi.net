
Imports osi.root.connector
Imports osi.root.formation

Public Module selfhealth
    Private self_health As Boolean = False

    Public Function self_health_stage() As Boolean
        Return self_health
    End Function

    Private Function set_self_health(ByVal b As Boolean) As Boolean
        Dim found As Boolean = False
        Return foreach(Function(ByRef x) As Boolean
                           If TypeOf x.case Is failure_case Then
                               If found Then
                                   Return False
                               Else
                                   x.case.as(Of failure_case).self_health = b
                                   found = True
                               End If
                           End If
                           Return True
                       End Function) AndAlso found
    End Function

    Public Function run() As Boolean
        Dim selectors As vector(Of String) = Nothing
        selectors = New vector(Of String)()
        selectors.push_back(GetType(failure_case).Name())

        Dim r As Int32 = 0
        r = rnd_int(1, 10)
        For i As Int32 = 0 To r - 1
            If Not set_self_health(True) Then
                Return False
            End If
            self_health = True
            host.run(selectors)
            self_health = False
        Next

        If (assert_equal(failure_count(), r * failure_case.expected_failure_count) AndAlso
            set_self_health(False)) OrElse
           envs.utt_no_assert Then
            clear_failure()
            Return True
        Else
            Return False
        End If
    End Function
End Module
