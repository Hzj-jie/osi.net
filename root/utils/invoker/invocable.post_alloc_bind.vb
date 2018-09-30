
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.delegates

Public Module _invocable_post_alloc_bind
    <Extension()> Public Function post_alloc_bind(ByVal invocable As invocable(Of Action),
                                                  ByRef o As Action) As Boolean
        If invocable Is Nothing Then
            Return False
        End If
        If Not invocable.post_binding() Then
            Return False
        End If

        o = Sub()
                invocable(invocable.target_type().allocate())()
            End Sub
        Return True
    End Function

    <Extension()> Public Function post_alloc_bind(ByVal invocable As invocable(Of Action)) As Action
        Dim r As Action = Nothing
        assert(post_alloc_bind(invocable, r))
        Return r
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(ByVal invocable As invocable(Of Action),
                                                         ByRef o As Action) As Boolean
        Return pre_or_post_alloc_bind(invocable, AddressOf post_alloc_bind, o)
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(ByVal invocable As invocable(Of Action)) As Action
        Dim o As Action = Nothing
        assert(pre_or_post_alloc_bind(invocable, o))
        Return o
    End Function

    <Extension()> Public Function post_alloc_bind(Of T)(ByVal invocable As invocable(Of Action(Of T)),
                                                        ByRef o As Action(Of T)) As Boolean
        If invocable Is Nothing Then
            Return False
        End If
        If Not invocable.post_binding() Then
            Return False
        End If

        o = Sub(ByVal i As T)
                invocable(invocable.target_type().allocate())(i)
            End Sub
        Return True
    End Function

    <Extension()> Public Function post_alloc_bind(Of T)(ByVal invocable As invocable(Of Action(Of T))) As Action(Of T)
        Dim r As Action(Of T) = Nothing
        assert(post_alloc_bind(invocable, r))
        Return r
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T)(ByVal invocable As invocable(Of Action(Of T)),
                                                               ByRef o As Action(Of T)) As Boolean
        Return pre_or_post_alloc_bind(invocable, AddressOf post_alloc_bind, o)
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T) _
                                                        (ByVal invocable As invocable(Of Action(Of T))) As Action(Of T)
        Dim o As Action(Of T) = Nothing
        assert(pre_or_post_alloc_bind(invocable, o))
        Return o
    End Function

    <Extension()> Public Function post_alloc_bind(Of T, RT)(ByVal invocable As invocable(Of Func(Of T, RT)),
                                                            ByRef o As Func(Of T, RT)) As Boolean
        If invocable Is Nothing Then
            Return False
        End If
        If Not invocable.post_binding() Then
            Return False
        End If

        o = Function(ByVal i As T) As RT
                Return invocable(invocable.target_type().allocate())(i)
            End Function
        Return True
    End Function

    <Extension()> Public Function post_alloc_bind(Of T, RT) _
                                                 (ByVal invocable As invocable(Of Func(Of T, RT))) As Func(Of T, RT)
        Dim r As Func(Of T, RT) = Nothing
        assert(post_alloc_bind(invocable, r))
        Return r
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T, RT)(ByVal invocable As invocable(Of Func(Of T, RT)),
                                                                   ByRef o As Func(Of T, RT)) As Boolean
        Return pre_or_post_alloc_bind(invocable, AddressOf post_alloc_bind, o)
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T, RT) _
                                                        (ByVal invocable As invocable(Of Func(Of T, RT))) As Func(Of T, RT)
        Dim o As Func(Of T, RT) = Nothing
        assert(pre_or_post_alloc_bind(invocable, o))
        Return o
    End Function

    <Extension()> Public Function post_alloc_bind(Of T, T2, RT) _
                                                 (ByVal invocable As invocable(Of _do_val_ref(Of T, T2, RT)),
                                                  ByRef o As _do_val_ref(Of T, T2, RT)) _
                                                 As Boolean
        If invocable Is Nothing Then
            Return False
        End If
        If Not invocable.post_binding() Then
            Return False
        End If

        o = Function(ByVal i As T, ByRef r As T2) As RT
                Return invocable(invocable.target_type().allocate())(i, r)
            End Function
        Return True
    End Function

    <Extension()> Public Function post_alloc_bind(Of T, T2, RT) _
                                                 (ByVal invocable As invocable(Of _do_val_ref(Of T, T2, RT))) _
                                                 As _do_val_ref(Of T, T2, RT)
        Dim r As _do_val_ref(Of T, T2, RT) = Nothing
        assert(post_alloc_bind(invocable, r))
        Return r
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T, T2, RT) _
                                                        (ByVal invocable As invocable(Of _do_val_ref(Of T, T2, RT)),
                                                         ByRef o As _do_val_ref(Of T, T2, RT)) As Boolean
        Return pre_or_post_alloc_bind(invocable, AddressOf post_alloc_bind, o)
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T, T2, RT) _
                                                        (ByVal invocable As invocable(Of _do_val_ref(Of T, T2, RT))) _
                                                        As _do_val_ref(Of T, T2, RT)
        Dim o As _do_val_ref(Of T, T2, RT) = Nothing
        assert(pre_or_post_alloc_bind(invocable, o))
        Return o
    End Function

    Private Function pre_or_post_alloc_bind(Of T)(ByVal invocable As invocable(Of T),
                                                  ByVal post_alloc_bind As _do_val_ref(Of invocable(Of T), T, Boolean),
                                                  ByRef o As T) As Boolean
        If invocable Is Nothing Then
            Return False
        End If
        Return invocable.pre_bind(o) OrElse
               post_alloc_bind(invocable, o)
    End Function
End Module
