
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public Class scope_b(Of T As scope_b(Of T))
    Public NotInheritable Class includes_t
        Private ReadOnly _included As New unordered_set(Of String)()

        Public Function included(ByVal f As String) As Boolean
            Return _included.emplace(f).second()
        End Function
    End Class

    Public Function includes() As includes_t
        If is_root() Then
            assert(Not incs Is Nothing)
            Return incs
        End If
        assert(incs Is Nothing)
        Return (+root).includes()
    End Function
End Class
