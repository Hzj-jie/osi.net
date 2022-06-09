
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Public NotInheritable Class filtered_var
    Public Shared Function [New](ByVal v As var, ByVal filter As String) As var
        assert(Not v Is Nothing)
        assert(Not filter.null_or_empty())
        Return v.filtered(filter)
    End Function

    Private Sub New()
    End Sub
End Class

Partial Public Class var
    Private Shared Function filtered(ByVal m As map(Of String, vector(Of String)),
                                     ByVal filter As String) As map(Of String, vector(Of String))
        assert(Not m Is Nothing)
        Dim r As map(Of String, vector(Of String)) = Nothing
        r = New map(Of String, vector(Of String))()
        Dim it As map(Of String, vector(Of String)).iterator = Nothing
        it = m.begin()
        While it <> m.end()
            If strstartwith((+it).first, filter) Then
                r.insert(strmid((+it).first, strlen(filter)), (+it).second)
            End If
            it += 1
        End While
        Return r
    End Function

    Public Function filtered(ByVal filter As String) As var
        Return New var(copy(c), filtered(raw, filter), filtered(binded, filter), copy(others))
    End Function
End Class