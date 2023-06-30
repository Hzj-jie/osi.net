
Imports osi.root.delegates
Imports osi.root.connector

Public Module _parameter_resolve
    Private Function parameter_resolve(Of T, PT1, FT)(ByVal r As _do_val_ref(Of PT1, FT, Boolean),
                                                      ByVal exec As _do_val_ref(Of FT, T, Boolean),
                                                      ByVal p1 As PT1,
                                                      ByRef o As T) As Boolean
        assert(Not r Is Nothing)
        assert(Not exec Is Nothing)
        Dim f As FT = Nothing
        If r(p1, f) Then
            Return exec(f, o)
        Else
            Return False
        End If
    End Function

    Public Function parameter_resolve(Of T, PT1, PT2) _
                                     (ByVal r As _do_val_ref(Of PT1, _do_val_ref(Of PT2, T, Boolean), Boolean),
                                      ByVal p1 As PT1,
                                      ByVal p2 As PT2,
                                      ByRef o As T) As Boolean
        assert(Not r Is Nothing)
        Return parameter_resolve(r,
                                 Function(x As _do_val_ref(Of PT2, T, Boolean), ByRef y As T) As Boolean
                                     assert(Not x Is Nothing)
                                     Return x(p2, y)
                                 End Function,
                                 p1,
                                 o)
    End Function

    Public Function parameter_resolve(Of T, PT1, PT2, PT3) _
                                     (ByVal r As _do_val_ref(Of PT1, _do_val_val_ref(Of PT2, PT3, T, Boolean), Boolean),
                                      ByVal p1 As PT1,
                                      ByVal p2 As PT2,
                                      ByVal p3 As PT3,
                                      ByRef o As T) As Boolean
        assert(Not r Is Nothing)
        Return parameter_resolve(r,
                                 Function(x As _do_val_val_ref(Of PT2, PT3, T, Boolean), ByRef y As T) As Boolean
                                     assert(Not x Is Nothing)
                                     Return x(p2, p3, y)
                                 End Function,
                                 p1,
                                 o)
    End Function

    Public Function parameter_resolve(Of T, PT1, PT2, PT3, PT4) _
                                     (ByVal r As _do_val_ref(Of PT1, 
                                                                _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean),
                                                                Boolean),
                                      ByVal p1 As PT1,
                                      ByVal p2 As PT2,
                                      ByVal p3 As PT3,
                                      ByVal p4 As PT4,
                                      ByRef o As T) As Boolean
        assert(Not r Is Nothing)
        Return parameter_resolve(r,
                                 Function(x As _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean),
                                          ByRef y As T) As Boolean
                                     assert(Not x Is Nothing)
                                     Return x(p2, p3, p4, y)
                                 End Function,
                                 p1,
                                 o)
    End Function

    Public Function parameter_resolve(Of T, PT1, PT2, PT3, PT4, PT5) _
                        (ByVal r As _do_val_ref(Of PT1, 
                                                   _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean),
                                                  Boolean),
                         ByVal p1 As PT1,
                         ByVal p2 As PT2,
                         ByVal p3 As PT3,
                         ByVal p4 As PT4,
                         ByVal p5 As PT5,
                         ByRef o As T) As Boolean
        assert(Not r Is Nothing)
        Return parameter_resolve(r,
                                 Function(x As _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean),
                                          ByRef y As T) As Boolean
                                     assert(Not x Is Nothing)
                                     Return x(p2, p3, p4, p5, y)
                                 End Function,
                                 p1,
                                 o)
    End Function
End Module
