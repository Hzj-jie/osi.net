
Imports osi.root.delegates
Imports osi.root.connector

Partial Public NotInheritable Class queue_runner
    Public Shared Function check(ByVal c As Func(Of Boolean),
                                 ByVal cb As Action,
                                 ByRef f As Func(Of Boolean)) As Boolean
        If c Is Nothing OrElse cb Is Nothing Then
            Return False
        Else
            f = Function() As Boolean
                    If do_(c, True) Then
                        cb()
                        Return False
                    Else
                        Return True
                    End If
                End Function
            Return True
        End If
    End Function

    Public Shared Function check(ByVal c As Func(Of Boolean),
                                 ByVal cb As Action,
                                 ByVal timeout_ms As Int64,
                                 ByRef f As Func(Of Boolean)) As Boolean
        If timeout_ms < 0 Then
            Return check(c, cb, f)
        ElseIf c Is Nothing Then
            Return False
        Else
            Dim till As Int64 = 0
            till = nowadays.milliseconds() + timeout_ms
            Return check(Function() As Boolean
                             Return c() OrElse
                                    (nowadays.milliseconds() >= till)
                         End Function,
                         cb,
                         f)
        End If
    End Function

    Public Shared Function check(ByVal c As Func(Of Boolean),
                                 ByVal cb As Action) As Func(Of Boolean)
        Dim f As Func(Of Boolean) = Nothing
        assert(check(c, cb, f))
        Return f
    End Function

    Public Shared Function check(ByVal c As Func(Of Boolean),
                                 ByVal cb As Action,
                                 ByVal timeout_ms As Int64) As Func(Of Boolean)
        Dim f As Func(Of Boolean) = Nothing
        assert(check(c, cb, timeout_ms, f))
        Return f
    End Function
End Class
