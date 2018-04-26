
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _invoker
    <Extension()> Public Function post_allocate_bind(ByVal invoker As invoker(Of Action)) As Action
        assert(Not invoker Is Nothing)
        assert(invoker.post_binding())
        Dim r As Action = Nothing
        Return Sub()
                   invoker(invoker.target_type().allocate())()
               End Sub
    End Function

    <Extension()> Public Function post_allocate_bind(Of T)(ByVal invoker As invoker(Of Action(Of T))) As Action(Of T)
        assert(Not invoker Is Nothing)
        assert(invoker.post_binding())
        Dim r As Action(Of T) = Nothing
        Return Sub(ByVal i As T)
                   invoker(invoker.target_type().allocate())(i)
               End Sub
    End Function

    <Extension()> Public Function post_allocate_bind(Of T, RT) _
                                                    (ByVal invoker As invoker(Of Func(Of T, RT))) As Func(Of T, RT)
        assert(Not invoker Is Nothing)
        assert(invoker.post_binding())
        Dim r As Action(Of T) = Nothing
        Return Function(ByVal i As T) As RT
                   Return invoker(invoker.target_type().allocate())(i)
               End Function
    End Function
End Module
