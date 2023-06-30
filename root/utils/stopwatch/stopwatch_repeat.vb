
Partial Public Class stopwatch
    Public Shared Function repeat(ByVal waitms As UInt32, ByVal d As Func(Of Boolean)) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Dim e As [event] = Nothing
            Dim r As Boolean = False
            e = New [event](waitms,
                            Sub()
                                r = d()
                            End Sub)
            Return queue_runner.push(Function() As Boolean
                                         r = False
                                         If e.do() Then
                                             Return True
                                         Else
                                             If r Then
                                                 e.restart()
                                             End If
                                             Return r
                                         End If
                                     End Function)
        End If
    End Function

    Public Shared Function repeat(ByVal waitms As UInt32, ByVal d As Action) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Return repeat(waitms, Function() As Boolean
                                      d()
                                      Return True
                                  End Function)
        End If
    End Function

    Public Shared Function repeat(ByVal waitms As Int32, ByVal d As Action) As Boolean
        Return repeat(_uint32(waitms), d)
    End Function

    Public Shared Function repeat(ByVal waitms As Int64, ByVal d As Action) As Boolean
        Return repeat(_uint32(waitms), d)
    End Function

    Public Shared Function repeat(ByVal waitms As Int32, ByVal d As Func(Of Boolean)) As Boolean
        Return repeat(_uint32(waitms), d)
    End Function

    Public Shared Function repeat(ByVal waitms As Int64, ByVal d As Func(Of Boolean)) As Boolean
        Return repeat(_uint32(waitms), d)
    End Function
End Class
