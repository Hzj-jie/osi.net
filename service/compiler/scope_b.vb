
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

' A scope implementation for bstyle and b2style
Partial Public MustInherit Class scope_b(Of CH As {call_hierarchy_t, New}, T As scope_b(Of CH, T))
    Inherits scope(Of T)

    Private ReadOnly incs As includes_t
    Private ReadOnly fc As CH

    Protected Sub New(ByVal parent As T)
        MyBase.New(parent)
        If parent Is Nothing Then
            incs = New includes_t()
            fc = New CH()
        End If
    End Sub

    Public Function includes() As includes_t
        If is_root() Then
            assert(Not incs Is Nothing)
            Return incs
        End If
        assert(incs Is Nothing)
        Return (+root).includes()
    End Function

    Public Function call_hierarchy() As CH
        If is_root() Then
            assert(Not fc Is Nothing)
            Return fc
        End If
        assert(fc Is Nothing)
        Return (+root).call_hierarchy()
    End Function
End Class
