
#If ASYNC_ALLOCATOR Then
Imports osi.root.delegates
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

' Client registers an allocator as a function to generate the target. Such as,
' register(Func(v As var, input As some_parameter) As target_type ... )
' resolve(v (As var), input (As some_parameter), (ByRef) output (As target_type)) (As Boolean)
Public Module _async_parameter_allocator
    Public Function parameter_allocator(Of T, PT1) _
                                       (ByVal i As Func(Of PT1, ref(Of T), event_comb),
                                        ByRef o As allocator(Of Func(Of ref(Of T), event_comb), PT1)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As Func(Of ref(Of T), event_comb)
                                   Return Function(p As ref(Of T)) As event_comb
                                              Return i(v, p)
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1) _
                                       (ByVal i As Func(Of PT1, ref(Of T), event_comb)) _
                                       As allocator(Of Func(Of ref(Of T), event_comb), PT1)
        Dim o As allocator(Of Func(Of ref(Of T), event_comb), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2) _
                                       (ByVal i As Func(Of PT1, PT2, ref(Of T), event_comb),
                                        ByRef o As allocator(Of Func(Of PT2, ref(Of T), event_comb), PT1)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As Func(Of PT2, ref(Of T), event_comb)
                                   Return Function(p As PT2, r As ref(Of T)) As event_comb
                                              Return i(v, p, r)
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2) _
                                       (ByVal i As Func(Of PT1, PT2, ref(Of T), event_comb)) _
                                       As allocator(Of Func(Of PT2, ref(Of T), event_comb), PT1)
        Dim o As allocator(Of Func(Of PT2, ref(Of T), event_comb), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3) _
                                       (ByVal i As Func(Of PT1, PT2, PT3, ref(Of T), event_comb),
                                        ByRef o As allocator(Of Func(Of PT2, PT3, ref(Of T), event_comb), PT1)) _
                                       As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As Func(Of PT2, PT3, ref(Of T), event_comb)
                                   Return Function(p1 As PT2, p2 As PT3, r As ref(Of T)) As event_comb
                                              Return i(v, p1, p2, r)
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3) _
                                       (ByVal i As Func(Of PT1, PT2, PT3, ref(Of T), event_comb)) _
                                       As allocator(Of Func(Of PT2, PT3, ref(Of T), event_comb), PT1)
        Dim o As allocator(Of Func(Of PT2, PT3, ref(Of T), event_comb), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4) _
                        (ByVal i As _func(Of PT1, PT2, PT3, PT4, ref(Of T), event_comb),
                         ByRef o As allocator(Of Func(Of PT2, PT3, PT4, ref(Of T), event_comb), PT1)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(Function(v As PT1) As Func(Of PT2, PT3, PT4, ref(Of T), event_comb)
                                   Return Function(p1 As PT2, p2 As PT3, p3 As PT4, r As ref(Of T)) As event_comb
                                              Return i(v, p1, p2, p3, r)
                                          End Function
                               End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4) _
                        (ByVal i As _func(Of PT1, PT2, PT3, PT4, ref(Of T), event_comb)) _
                        As allocator(Of Func(Of PT2, PT3, PT4, ref(Of T), event_comb), PT1)
        Dim o As allocator(Of Func(Of PT2, PT3, PT4, ref(Of T), event_comb), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4, PT5) _
                        (ByVal i As _func(Of PT1, PT2, PT3, PT4, PT5, ref(Of T), event_comb),
                         ByRef o As allocator(Of _func(Of PT2, PT3, PT4, PT5, ref(Of T), event_comb), PT1)) _
                        As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = make_allocator(
                    Function(v As PT1) As _func(Of PT2, PT3, PT4, PT5, ref(Of T), event_comb)
                        Return Function(p As PT2, p2 As PT3, p3 As PT4, p4 As PT5, r As ref(Of T)) As event_comb
                                   Return i(v, p, p2, p3, p4, r)
                               End Function
                    End Function)
            Return True
        End If
    End Function

    Public Function parameter_allocator(Of T, PT1, PT2, PT3, PT4, PT5) _
                        (ByVal i As _func(Of PT1, PT2, PT3, PT4, PT5, ref(Of T), event_comb)) _
                        As allocator(Of _func(Of PT2, PT3, PT4, PT5, ref(Of T), event_comb), PT1)
        Dim o As allocator(Of _func(Of PT2, PT3, PT4, PT5, ref(Of T), event_comb), PT1) = Nothing
        assert(parameter_allocator(i, o))
        Return o
    End Function
End Module
#End If
