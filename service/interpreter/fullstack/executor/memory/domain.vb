
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.utils

Namespace fullstack.executor
    Public Module _domain
        <Extension()> Public Function create_disposer(ByVal i As domain) As disposer(Of domain)
            assert(Not i Is Nothing)
            Return New disposer(Of domain)(i,
                                           disposer:=Sub(v As domain)
                                                         assert(object_compare(i, v) = 0)
                                                         i.over()
                                                     End Sub)
        End Function

        <Extension()> Public Function create_disposer(ByVal i As domain,
                                                             ByRef o As domain) As disposer(Of domain)
            o = New domain(i)
            Return create_disposer(o)
        End Function

        <Extension()> Public Function create_disposer(ByVal i As variables,
                                                      ByRef o As domain) As disposer(Of domain)
            o = New domain(i)
            Return create_disposer(o)
        End Function
    End Module

    Public Class domain
        Private ReadOnly p As domain
        Private ReadOnly vs As variables
        Private ReadOnly base As UInt32
        Private ReadOnly inc As atomic_int

        Private Sub New(ByVal vs As variables, ByVal parent As domain)
            If vs Is Nothing Then
                assert(Not parent Is Nothing)
                vs = parent.vs
            Else
                assert(parent Is Nothing)
            End If
            assert(Not vs Is Nothing)
            Me.vs = vs
            Me.p = parent
            Me.base = vs.size()
            Me.inc = New atomic_int()
        End Sub

        Public Sub New(ByVal vs As variables)
            Me.New(vs, Nothing)
        End Sub

        Public Sub New(ByVal parent As domain)
            Me.New(Nothing, parent)
        End Sub

        Public Sub define(ByVal v As variable)
            assert(Not v Is Nothing)
            inc.increment()
            vs.push(v)
        End Sub

        Public Function parent() As domain
            assert(Not p Is Nothing)
            Return p
        End Function

        Public Function ancestor(ByVal level As UInt32) As domain
            Dim d As domain = Nothing
            d = Me
            While level > 0
                d = d.parent()
                level -= 1
            End While
            Return d
        End Function

        Public Function increment() As Int32
            Return +inc
        End Function

        Private Function location(ByVal offset As UInt32) As UInt32
            Return base + offset
        End Function

        Public Function variable(ByVal level As UInt32, ByVal offset As UInt32) As variable
            Dim d As domain = Nothing
            d = ancestor(level)
            Return d.vs(d.location(offset))
        End Function

        Public Function last() As variable
            Return variable(0, increment() - 1)
        End Function

        Public Sub replace(ByVal level As UInt32, ByVal offset As UInt32, ByVal v As variable)
            Dim d As domain = Nothing
            d = ancestor(level)
            d.vs.replace(d.location(offset), v)
        End Sub

        Public Sub replace_last(ByVal v As variable)
            replace(0, increment() - 1, v)
        End Sub

        Public Sub over()
            For i As Int32 = 0 To increment() - 1
                vs.pop()
            Next
            inc.set(0)
        End Sub
    End Class
End Namespace
