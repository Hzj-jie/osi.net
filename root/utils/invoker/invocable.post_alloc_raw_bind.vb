
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _invocable_post_alloc_raw_bind
    <Extension()> Public Function post_alloc_raw_bind(Of delegate_t) _
                                                     (ByVal invoker As invocable(Of delegate_t),
                                                      ByRef o As Func(Of Object(), Object)) As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If Not invoker.post_binding() Then
            Return False
        End If

        o = Function(ByVal parameters() As Object) As Object
                Return invoker.invoke(invoker.target_type().allocate(), parameters)
            End Function
        Return True
    End Function

    <Extension()> Public Function post_alloc_raw_bind(Of delegate_t) _
                                                     (ByVal invoker As invocable(Of delegate_t)) _
                                                     As Func(Of Object(), Object)
        Dim o As Func(Of Object(), Object) = Nothing
        assert(post_alloc_raw_bind(invoker, o))
        Return o
    End Function

    <Extension()> Public Function post_alloc_raw_bind(Of delegate_t, T) _
                                                     (ByVal invoker As invocable(Of delegate_t),
                                                      ByRef o As Func(Of Object(), T)) As Boolean
        Dim r As Func(Of Object(), Object) = Nothing
        If Not post_alloc_raw_bind(invoker, r) Then
            Return False
        End If
        assert(Not invoker Is Nothing)
        If Not GetType(T).[is](invoker.method_info().ReturnType()) Then
            Return False
        End If
        o = Function(ByVal parameters() As Object) As T
                Return direct_cast(Of T)(r(parameters))
            End Function
        Return True
    End Function

    <Extension()> Public Function post_alloc_raw_bind(Of delegate_t, T) _
                                                     (ByVal invoker As invocable(Of delegate_t)) As Func(Of Object(), T)
        Dim o As Func(Of Object(), T) = Nothing
        assert(post_alloc_raw_bind(invoker, o))
        Return o
    End Function

    <Extension()> Public Function pre_or_post_alloc_raw_bind(Of delegate_t) _
                                                            (ByVal invoker As invocable(Of delegate_t),
                                                             ByRef o As Func(Of Object(), Object)) As Boolean
        If invoker Is Nothing Then
            Return False
        End If
        If invoker.pre_binding() Then
            o = Function(ByVal parameters() As Object) As Object
                    Return invoker.invoke(Nothing, parameters)
                End Function
            Return True
        End If
        Return post_alloc_raw_bind(invoker, o)
    End Function

    <Extension()> Public Function pre_or_post_alloc_raw_bind(Of delegate_t) _
                                                            (ByVal invoker As invocable(Of delegate_t)) _
                                                            As Func(Of Object(), Object)
        Dim o As Func(Of Object(), Object) = Nothing
        assert(pre_or_post_alloc_raw_bind(invoker, o))
        Return o
    End Function

    <Extension()> Public Function pre_or_post_alloc_raw_bind(Of delegate_t, T) _
                                                            (ByVal invoker As invocable(Of delegate_t),
                                                             ByRef o As Func(Of Object(), T)) As Boolean
        Dim r As Func(Of Object(), Object) = Nothing
        If Not pre_or_post_alloc_raw_bind(invoker, r) Then
            Return False
        End If
        assert(Not invoker Is Nothing)
        If Not GetType(T).[is](invoker.method_info().ReturnType()) Then
            Return False
        End If
        o = Function(ByVal parameters() As Object) As T
                Return direct_cast(Of T)(r(parameters))
            End Function
        Return True
    End Function

    <Extension()> Public Function pre_or_post_alloc_raw_bind(Of delegate_t, T) _
                                                            (ByVal invoker As invocable(Of delegate_t)) _
                                                            As Func(Of Object(), T)
        Dim o As Func(Of Object(), T) = Nothing
        assert(pre_or_post_alloc_raw_bind(invoker, o))
        Return o
    End Function
End Module
