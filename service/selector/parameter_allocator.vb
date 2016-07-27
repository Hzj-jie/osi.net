
Imports osi.root.delegates
Imports osi.root.connector

' Client registers an allocator as a function to generate the target. Such as,
' register(Func(v As var, input As some_parameter) As target_type ... )
' resolve(v (As var), input (As some_parameter), (ByRef) output (As target_type)) (As Boolean)
Public Module _parameter_allocator
    Public Function parameter_allocator(Of T, PT1, PT2) _
                                       (ByVal i As _do_val_val_ref(Of PT1, PT2, T, Boolean),
                                        ByRef o As allocator(Of _do_val_ref(Of PT2, T, Boolean), PT1)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As _do_val_ref(Of PT2, T, Boolean)
                                   Return Function(p As PT2, ByRef r As T) As Boolean
                                              Return i(v, p, r)
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2) _
                                       (ByVal i As _do_val_val_ref(Of PT1, PT2, T, Boolean)) _
                                       As allocator(Of _do_val_ref(Of PT2, T, Boolean), PT1)
        Dim o As allocator(Of _do_val_ref(Of PT2, T, Boolean), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2) _
                                       (ByVal i As Func(Of PT1, PT2, T),
                                        ByRef o As allocator(Of _do_val_ref(Of PT2, T, Boolean), PT1)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As _do_val_ref(Of PT2, T, Boolean)
                                   Return Function(p As PT2, ByRef r As T) As Boolean
                                              r = i(v, p)
                                              Return True
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2) _
                                       (ByVal i As Func(Of PT1, PT2, T)) _
                                       As allocator(Of _do_val_ref(Of PT2, T, Boolean), PT1)
        Dim o As allocator(Of _do_val_ref(Of PT2, T, Boolean), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3) _
                                       (ByVal i As _do_val_val_val_ref(Of PT1, PT2, PT3, T, Boolean),
                                        ByRef o As allocator(Of _do_val_val_ref(Of PT2, PT3, T, Boolean), PT1)) _
                                       As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As _do_val_val_ref(Of PT2, PT3, T, Boolean)
                                   Return Function(p As PT2, p2 As PT3, ByRef r As T) As Boolean
                                              Return i(v, p, p2, r)
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3) _
                                       (ByVal i As _do_val_val_val_ref(Of PT1, PT2, PT3, T, Boolean)) _
                                       As allocator(Of _do_val_val_ref(Of PT2, PT3, T, Boolean), PT1)
        Dim o As allocator(Of _do_val_val_ref(Of PT2, PT3, T, Boolean), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3) _
                                       (ByVal i As Func(Of PT1, PT2, PT3, T),
                                        ByRef o As allocator(Of _do_val_val_ref(Of PT2, PT3, T, Boolean), PT1)) _
                                       As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As _do_val_val_ref(Of PT2, PT3, T, Boolean)
                                   Return Function(p As PT2, p2 As PT3, ByRef r As T) As Boolean
                                              r = i(v, p, p2)
                                              Return True
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3) _
                                       (ByVal i As Func(Of PT1, PT2, PT3, T)) _
                                       As allocator(Of _do_val_val_ref(Of PT2, PT3, T, Boolean), PT1)
        Dim o As allocator(Of _do_val_val_ref(Of PT2, PT3, T, Boolean), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4) _
                        (ByVal i As _do_val_val_val_val_ref(Of PT1, PT2, PT3, PT4, T, Boolean),
                         ByRef o As allocator(Of _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean), PT1)) _
                        As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean)
                                   Return Function(p As PT2, p2 As PT3, p3 As PT4, ByRef r As T) As Boolean
                                              Return i(v, p, p2, p3, r)
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4) _
                                       (ByVal i As _do_val_val_val_val_ref(Of PT1, PT2, PT3, PT4, T, Boolean)) _
                                       As allocator(Of _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean), PT1)
        Dim o As allocator(Of _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4) _
                        (ByVal i As Func(Of PT1, PT2, PT3, PT4, T),
                         ByRef o As allocator(Of _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean), PT1)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean)
                                   Return Function(p As PT2, p2 As PT3, p3 As PT4, ByRef r As T) As Boolean
                                              r = i(v, p, p2, p3)
                                              Return True
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4) _
                        (ByVal i As Func(Of PT1, PT2, PT3, PT4, T)) _
                        As allocator(Of _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean), PT1)
        Dim o As allocator(Of _do_val_val_val_ref(Of PT2, PT3, PT4, T, Boolean), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4, PT5) _
                        (ByVal i As _do_val_val_val_val_val_ref(Of PT1, PT2, PT3, PT4, PT5, T, Boolean),
                         ByRef o As allocator(Of _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean), PT1)) _
                        As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean)
                                   Return Function(p As PT2, p2 As PT3, p3 As PT4, p4 As PT5, ByRef r As T) As Boolean
                                              Return i(v, p, p2, p3, p4, r)
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4, PT5) _
                        (ByVal i As _do_val_val_val_val_val_ref(Of PT1, PT2, PT3, PT4, PT5, T, Boolean)) _
                        As allocator(Of _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean), PT1)
        Dim o As allocator(Of _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4, PT5) _
                        (ByVal i As _func(Of PT1, PT2, PT3, PT4, PT5, T),
                         ByRef o As allocator(Of _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean), PT1)) _
                        As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean)
                                   Return Function(p As PT2, p2 As PT3, p3 As PT4, p4 As PT5, ByRef r As T) As Boolean
                                              r = i(v, p, p2, p3, p4)
                                              Return True
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4, PT5) _
                        (ByVal i As _func(Of PT1, PT2, PT3, PT4, PT5, T)) _
                        As allocator(Of _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean), PT1)
        Dim o As allocator(Of _do_val_val_val_val_ref(Of PT2, PT3, PT4, PT5, T, Boolean), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function
End Module
