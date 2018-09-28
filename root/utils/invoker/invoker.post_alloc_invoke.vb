
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _invoker_post_alloc_invoke
    <Extension()> Public Function post_alloc_invoke(Of delegate_t) _
                                                   (ByVal invoker As invoker(Of delegate_t),
                                                    ByRef result As Object,
                                                    ByVal ParamArray parameters() As Object) As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If Not invoker.instance_invokeable() Then
            Return False
        End If
        result = invoker.invoke(invoker.target_type().allocate(), parameters)
        Return True
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

    <Extension()> Public Function pre_or_post_alloc_invoke(Of delegate_t) _
                                                          (ByVal invoker As invoker(Of delegate_t),
                                                           ByVal ParamArray parameters() As Object) As Object
        Dim o As Object = Nothing
        assert(pre_or_post_alloc_invoke(invoker, o, parameters))
        Return o
    End Function

    <Extension()> Public Function post_alloc_invoke(Of delegate_t, T) _
                                                   (ByVal invoker As invoker(Of delegate_t),
                                                    ByRef result As T,
                                                    ByVal ParamArray parameters() As Object) As Boolean
        Dim o As Object = Nothing
        Return post_alloc_invoke(invoker, o, parameters) AndAlso
               direct_cast(o, result)
    End Function

    <Extension()> Public Function post_alloc_invoke(Of delegate_t, T) _
                                                   (ByVal invoker As invoker(Of delegate_t),
                                                    ByVal ParamArray parameters() As Object) As T
        Dim o As T = Nothing
        assert(post_alloc_invoke(invoker, o, parameters))
        Return o
    End Function

    <Extension()> Public Function pre_or_post_alloc_invoke(Of delegate_t, T) _
                                                          (ByVal invoker As invoker(Of delegate_t),
                                                           ByRef result As T,
                                                           ByVal ParamArray parameters() As Object) As Boolean
        Dim o As Object = Nothing
        Return pre_or_post_alloc_invoke(invoker, o, parameters) AndAlso
               direct_cast(o, result)
    End Function

    <Extension()> Public Function pre_or_post_alloc_invoke(Of delegate_t, T) _
                                                          (ByVal invoker As invoker(Of delegate_t),
                                                           ByVal ParamArray parameters() As Object) As T
        Dim o As T = Nothing
        assert(pre_or_post_alloc_invoke(invoker, o, parameters))
        Return o
    End Function
End Module
