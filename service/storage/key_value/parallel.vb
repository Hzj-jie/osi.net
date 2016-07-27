
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils

'almost same as distributor, but do not want to merge them, since the difference makes the code even longer
Public Module _parallel
    Public Function parallel(ByVal d As Func(Of event_comb),
                             ByVal t As Func(Of event_comb),
                             Optional ByVal e As Func(Of Boolean) = Nothing) As event_comb
        assert(Not d Is Nothing)
        assert(Not t Is Nothing)
        Dim ec1 As event_comb = Nothing
        Dim ec2 As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec1 = d()
                                  ec2 = t()
                                  Return waitfor(ec1, ec2) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec1.end_result() AndAlso
                                         ec2.end_result() AndAlso
                                         If(e Is Nothing, True, e()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function parallel(Of RT)(ByVal d As Func(Of pointer(Of RT), event_comb),
                                    ByVal t As Func(Of pointer(Of RT), event_comb),
                                    ByVal consistent As Func(Of RT, RT, Boolean),
                                    ByVal r As pointer(Of RT)) As event_comb
        assert(Not d Is Nothing)
        assert(Not t Is Nothing)
        assert(Not consistent Is Nothing)
        Dim r1 As pointer(Of RT) = Nothing
        Dim r2 As pointer(Of RT) = Nothing
        Return parallel(Function() As event_comb
                            r1 = New pointer(Of RT)()
                            Return d(r1)
                        End Function,
                        Function() As event_comb
                            r2 = New pointer(Of RT)()
                            Return t(r2)
                        End Function,
                        Function() As Boolean
                            Return (consistent(+r1, +r2) AndAlso eva(r, +r1))
                        End Function)
    End Function

    Public Function parallel(ByVal d As Func(Of pointer(Of Boolean), event_comb),
                             ByVal t As Func(Of pointer(Of Boolean), event_comb),
                             ByVal r As pointer(Of Boolean)) As event_comb
        Return parallel(d,
                        t,
                        Function(i, j) i = j,
                        r)
    End Function

    Public Function parallel(ByVal d As Func(Of pointer(Of Int64), event_comb),
                             ByVal t As Func(Of pointer(Of Int64), event_comb),
                             ByVal r As pointer(Of Int64)) As event_comb
        Return parallel(d,
                        t,
                        Function(x, y) x = y,
                        r)
    End Function

    Public Function [or](ByVal d As Func(Of pointer(Of Boolean), event_comb),
                         ByVal t As Func(Of pointer(Of Boolean), event_comb),
                         ByVal r As pointer(Of Boolean)) As event_comb
        Dim r1 As pointer(Of Boolean) = Nothing
        Dim r2 As pointer(Of Boolean) = Nothing
        Return parallel(Function() As event_comb
                            r1 = New pointer(Of Boolean)()
                            Return d(r1)
                        End Function,
                        Function() As event_comb
                            r2 = New pointer(Of Boolean)()
                            Return t(r2)
                        End Function,
                        Function() As Boolean
                            Return eva(r, (+r1) OrElse (+r2))
                        End Function)
    End Function
End Module
