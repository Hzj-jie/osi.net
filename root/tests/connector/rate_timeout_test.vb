
Imports osi.root.connector
Imports osi.root.utt

Public Class rate_timeout_test
    Inherits [case]

    Private Shared Function case1() As Boolean
        Dim t As rate_timeout = Nothing
        t = New rate_timeout(0)
        t.update(0)
        sleep()
        assertion.is_false(t.timeout())
        t.update(100)
        sleep()
        assertion.is_false(t.timeout())
        Return True
    End Function

    Private Shared Function case2() As Boolean
        Dim t As rate_timeout = Nothing
        t = New rate_timeout(1)
        t.update(0)
        sleep_seconds()
        assertion.is_true(t.timeout())
        t.update(1)
        sleep_seconds(2)
        assertion.is_true(t.timeout())
        t.update(100)
        sleep_seconds()
        assertion.is_false(t.timeout())
        Return True
    End Function

    Public Overrides Function reserved_processors() As Int16
        Return 0
    End Function

    Public Overrides Function run() As Boolean
        Return case1() AndAlso
               case2()
    End Function
End Class
