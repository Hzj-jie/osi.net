
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.delegates

Public Module _invoker
    <Extension()> Public Function post_alloc_invoke(Of delegate_t) _
                                                   (ByVal invoker As invoker(Of delegate_t),
                                                    ByRef result As Object,
                                                    ByVal ParamArray parameters() As Object) As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If Not invoker.post_binding() Then
            Return False
        End If
        result = invoker.invoke(invoker.target_type().allocate(), parameters)
        Return True
    End Function

    <Extension()> Public Function pre_or_post_alloc_invoke(Of delegate_t) _
                                                          (ByVal invoker As invoker(Of delegate_t),
                                                           ByRef result As Object,
                                                           ByVal ParamArray parameters() As Object) As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If invoker.pre_binding() Then
            result = invoker.invoke(Nothing, parameters)
            Return True
        End If
        Return post_alloc_invoke(invoker, result, parameters)
    End Function

    <Extension()> Public Function post_alloc_invoke(Of delegate_t) _
                                                   (ByVal invoker As invoker(Of delegate_t),
                                                    ByVal ParamArray parameters() As Object) As Object
        Dim o As Object = Nothing
        assert(post_alloc_invoke(invoker, o, parameters))
        Return o
    End Function

    <Extension()> Public Function pre_or_post_alloc_invoke(Of delegate_t) _
                                                          (ByVal invoker As invoker(Of delegate_t),
                                                           ByVal ParamArray parameters() As Object) As Object
        Dim o As Object = Nothing
        assert(pre_or_post_alloc_invoke(invoker, o, parameters))
        Return o
    End Function

    <Extension()> Public Function post_alloc_bind(ByVal invoker As invoker(Of Action),
                                                  ByRef o As Action) As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If Not invoker.post_binding() Then
            Return False
        End If

        o = Sub()
                invoker(invoker.target_type().allocate())()
            End Sub
        Return True
    End Function

    <Extension()> Public Function post_alloc_bind(ByVal invoker As invoker(Of Action)) As Action
        Dim r As Action = Nothing
        assert(post_alloc_bind(invoker, r))
        Return r
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(ByVal invoker As invoker(Of Action),
                                                         ByRef o As Action) As Boolean
        Return pre_or_post_alloc_bind(invoker, AddressOf post_alloc_bind, o)
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(ByVal invoker As invoker(Of Action)) As Action
        Dim o As Action = Nothing
        assert(pre_or_post_alloc_bind(invoker, o))
        Return o
    End Function

    <Extension()> Public Function post_alloc_bind(Of T)(ByVal invoker As invoker(Of Action(Of T)),
                                                        ByRef o As Action(Of T)) As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If Not invoker.post_binding() Then
            Return False
        End If

        o = Sub(ByVal i As T)
                invoker(invoker.target_type().allocate())(i)
            End Sub
        Return True
    End Function

    <Extension()> Public Function post_alloc_bind(Of T)(ByVal invoker As invoker(Of Action(Of T))) As Action(Of T)
        Dim r As Action(Of T) = Nothing
        assert(post_alloc_bind(invoker, r))
        Return r
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T)(ByVal invoker As invoker(Of Action(Of T)),
                                                               ByRef o As Action(Of T)) As Boolean
        Return pre_or_post_alloc_bind(invoker, AddressOf post_alloc_bind, o)
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T) _
                                                        (ByVal invoker As invoker(Of Action(Of T))) As Action(Of T)
        Dim o As Action(Of T) = Nothing
        assert(pre_or_post_alloc_bind(invoker, o))
        Return o
    End Function

    <Extension()> Public Function post_alloc_bind(Of T, RT)(ByVal invoker As invoker(Of Func(Of T, RT)),
                                                            ByRef o As Func(Of T, RT)) As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If Not invoker.post_binding() Then
            Return False
        End If

        o = Function(ByVal i As T) As RT
                Return invoker(invoker.target_type().allocate())(i)
            End Function
        Return True
    End Function

    <Extension()> Public Function post_alloc_bind(Of T, RT) _
                                                 (ByVal invoker As invoker(Of Func(Of T, RT))) As Func(Of T, RT)
        Dim r As Func(Of T, RT) = Nothing
        assert(post_alloc_bind(invoker, r))
        Return r
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T, RT)(ByVal invoker As invoker(Of Func(Of T, RT)),
                                                                   ByRef o As Func(Of T, RT)) As Boolean
        Return pre_or_post_alloc_bind(invoker, AddressOf post_alloc_bind, o)
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T, RT) _
                                                        (ByVal invoker As invoker(Of Func(Of T, RT))) As Func(Of T, RT)
        Dim o As Func(Of T, RT) = Nothing
        assert(pre_or_post_alloc_bind(invoker, o))
        Return o
    End Function

    <Extension()> Public Function post_alloc_bind(Of T, T2, RT) _
                                                 (ByVal invoker As invoker(Of _do_val_ref(Of T, T2, RT)),
                                                  ByRef o As _do_val_ref(Of T, T2, RT)) _
                                                 As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If Not invoker.post_binding() Then
            Return False
        End If

        o = Function(ByVal i As T, ByRef r As T2) As RT
                Return invoker(invoker.target_type().allocate())(i, r)
            End Function
        Return True
    End Function

    <Extension()> Public Function post_alloc_bind(Of T, T2, RT) _
                                                 (ByVal invoker As invoker(Of _do_val_ref(Of T, T2, RT))) _
                                                 As _do_val_ref(Of T, T2, RT)
        Dim r As _do_val_ref(Of T, T2, RT) = Nothing
        assert(post_alloc_bind(invoker, r))
        Return r
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T, T2, RT) _
                                                        (ByVal invoker As invoker(Of _do_val_ref(Of T, T2, RT)),
                                                         ByRef o As _do_val_ref(Of T, T2, RT)) As Boolean
        Return pre_or_post_alloc_bind(invoker, AddressOf post_alloc_bind, o)
    End Function

    <Extension()> Public Function pre_or_post_alloc_bind(Of T, T2, RT) _
                                                        (ByVal invoker As invoker(Of _do_val_ref(Of T, T2, RT))) _
                                                        As _do_val_ref(Of T, T2, RT)
        Dim o As _do_val_ref(Of T, T2, RT) = Nothing
        assert(pre_or_post_alloc_bind(invoker, o))
        Return o
    End Function

    Private Function pre_or_post_alloc_bind(Of T)(ByVal invoker As invoker(Of T),
                                                  ByVal post_alloc_bind As _do_val_ref(Of invoker(Of T), T, Boolean),
                                                  ByRef o As T) As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If invoker.pre_binding() Then
            Return assert(invoker.pre_bind(o))
        End If
        Return assert(post_alloc_bind(invoker, o))
    End Function
End Module
