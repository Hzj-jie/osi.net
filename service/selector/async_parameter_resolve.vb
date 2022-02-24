
#If ASYNC_ALLOCATOR Then
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Public Module _async_parameter_resolve
    Private Function parameter_resolve(Of T, PT1, FT)(ByVal r As _do_val_ref(Of PT1, FT, Boolean),
                                                      ByVal exec As Func(Of FT, ref(Of T), event_comb),
                                                      ByVal p1 As PT1,
                                                      ByVal o As ref(Of T)) As event_comb
        assert(Not r Is Nothing)
        assert(Not exec Is Nothing)
        Dim f As FT = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If r(p1, f) Then
                                      ec = exec(f, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function parameter_resolve(Of T, PT1) _
                                     (ByVal r As _do_val_ref(Of PT1, Func(Of ref(Of T), event_comb), Boolean),
                                      ByVal p1 As PT1,
                                      ByVal o As ref(Of T)) As event_comb
        assert(Not r Is Nothing)
        Return parameter_resolve(r,
                                 Function(x As Func(Of ref(Of T), event_comb), y As ref(Of T)) As event_comb
                                     assert(Not x Is Nothing)
                                     Return x(y)
                                 End Function,
                                 p1,
                                 o)
    End Function

    Public Function parameter_resolve(Of T, PT1, PT2) _
                                     (ByVal r As _do_val_ref(Of PT1, Func(Of PT2, ref(Of T), event_comb), Boolean),
                                      ByVal p1 As PT1,
                                      ByVal p2 As PT2,
                                      ByVal o As ref(Of T)) As event_comb
        assert(Not r Is Nothing)
        Return parameter_resolve(r,
                                 Function(x As Func(Of PT2, ref(Of T), event_comb),
                                          y As ref(Of T)) As event_comb
                                     assert(Not x Is Nothing)
                                     Return x(p2, y)
                                 End Function,
                                 p1,
                                 o)
    End Function

    Public Function parameter_resolve(Of T, PT1, PT2, PT3) _
                                     (ByVal r As _do_val_ref(Of PT1, 
                                                                Func(Of PT2, PT3, ref(Of T), event_comb),
                                                                Boolean),
                                      ByVal p1 As PT1,
                                      ByVal p2 As PT2,
                                      ByVal p3 As PT3,
                                      ByVal o As ref(Of T)) As event_comb
        assert(Not r Is Nothing)
        Return parameter_resolve(r,
                                 Function(x As Func(Of PT2, PT3, ref(Of T), event_comb),
                                          y As ref(Of T)) As event_comb
                                     assert(Not x Is Nothing)
                                     Return x(p2, p3, y)
                                 End Function,
                                 p1,
                                 o)
    End Function

    Public Function parameter_resolve(Of T, PT1, PT2, PT3, PT4) _
                                     (ByVal r As _do_val_ref(Of PT1, 
                                                                Func(Of PT2, PT3, PT4, ref(Of T), event_comb),
                                                                Boolean),
                                      ByVal p1 As PT1,
                                      ByVal p2 As PT2,
                                      ByVal p3 As PT3,
                                      ByVal p4 As PT4,
                                      ByVal o As ref(Of T)) As event_comb
        assert(Not r Is Nothing)
        Return parameter_resolve(r,
                                 Function(x As Func(Of PT2, PT3, PT4, ref(Of T), event_comb),
                                          y As ref(Of T)) As event_comb
                                     assert(Not x Is Nothing)
                                     Return x(p2, p3, p4, y)
                                 End Function,
                                 p1,
                                 o)
    End Function

    Public Function parameter_resolve(Of T, PT1, PT2, PT3, PT4, PT5) _
                        (ByVal r As _do_val_ref(Of PT1, 
                                                   _func(Of PT2, PT3, PT4, PT5, ref(Of T), event_comb),
                                                  Boolean),
                         ByVal p1 As PT1,
                         ByVal p2 As PT2,
                         ByVal p3 As PT3,
                         ByVal p4 As PT4,
                         ByVal p5 As PT5,
                         ByVal o As ref(Of T)) As event_comb
        assert(Not r Is Nothing)
        Return parameter_resolve(r,
                                 Function(x As _func(Of PT2, PT3, PT4, PT5, ref(Of T), event_comb),
                                          y As ref(Of T)) As event_comb
                                     assert(Not x Is Nothing)
                                     Return x(p2, p3, p4, p5, y)
                                 End Function,
                                 p1,
                                 o)
    End Function
End Module
#End If
