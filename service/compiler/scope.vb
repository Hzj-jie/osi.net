
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.constructor

Public Class scope(Of T As scope(Of T))
    <ThreadStatic> Private Shared in_thread As T
    Protected ReadOnly parent As T
    ' This variable has no functionality, but only ensures the start_scope() won't be executed multiple times.
    Private child As T = Nothing
    Public ReadOnly root As lazier(Of T) = lazier.of(AddressOf get_root)

    Protected Sub New(ByVal parent As T)
        Me.parent = parent
        If parent Is Nothing Then
            assert(in_thread Is Nothing)
        Else
            assert(object_compare(in_thread, parent) = 0)
        End If
        in_thread = this()
    End Sub

    Public Function start_scope() As T
        assert(child Is Nothing)
        child = inject_constructor(Of T).invoke(Me)
        Return child
    End Function

    Public Function end_scope() As T
        when_end_scope()
        assert(object_compare(in_thread, Me) = 0)
        If Not parent Is Nothing Then
            assert(object_compare(parent.child, Me) = 0)
            parent.child = Nothing
        End If
        in_thread = parent
        Return parent
    End Function

    Public Shared Function current() As T
        Return in_thread
    End Function

    Private Function get_root() As T
        Dim s As T = this()
        While Not s.is_root()
            s = s.parent
        End While
        Return s
    End Function

    Public Function this() As T
        Return direct_cast(Of T)(Me)
    End Function

    Protected Overridable Sub when_end_scope()
    End Sub

    Public Function is_root() As Boolean
        Return parent Is Nothing
    End Function
End Class
