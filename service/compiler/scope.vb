
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.constructor

Public Class scope(Of T As scope(Of T))
    <ThreadStatic> Private Shared this As T
    Protected ReadOnly parent As T
    ' This variable has no functionality, but only ensures the start_scope() won't be executed multiple times.
    Private child As T = Nothing

    Protected Sub New(ByVal parent As T)
        Me.parent = parent
        If parent Is Nothing Then
            assert(this Is Nothing)
        Else
            assert(object_compare(this, parent) = 0)
        End If
        this = direct_cast(Of T)(Me)
    End Sub

    Public Function start_scope() As T
        assert(child Is Nothing)
        child = inject_constructor(Of T).invoke(Me)
        Return child
    End Function

    Public Function end_scope() As T
        when_end_scope()
        assert(object_compare(this, Me) = 0)
        If Not parent Is Nothing Then
            assert(object_compare(parent.child, Me) = 0)
            parent.child = Nothing
        End If
        this = parent
        Return parent
    End Function

    Public Shared Function current() As T
        Return this
    End Function

    Protected Overridable Sub when_end_scope()
    End Sub

    Public Function is_root() As Boolean
        Return parent Is Nothing
    End Function
End Class
