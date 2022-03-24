
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

' A scope implementation for bstyle and b2style
Public MustInherit Class scope_b(Of T As scope_b(Of T))
    Inherits scope(Of T)

    Private ReadOnly incs As includes_t
    Private ReadOnly fc As call_hierarchy_t

    Protected Sub New(ByVal parent As T)
        MyBase.New(parent)
        If parent Is Nothing Then
            incs = New includes_t()
            fc = New call_hierarchy_t("main", AddressOf current_function_name)
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

    Public Shadows Function call_hierarchy() As call_hierarchy_t
        If is_root() Then
            assert(Not fc Is Nothing)
            Return fc
        End If
        assert(fc Is Nothing)
        Return (+root).call_hierarchy()
    End Function

    Protected MustOverride Function current_function_name() As [optional](Of String)
End Class
