
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class var
    Private Function bind(ByVal i As String) As Boolean
        If String.IsNullOrEmpty(i) Then
            Return False
        Else
            Dim it As map(Of String, vector(Of String)).iterator = Nothing
            it = raw.begin()
            While it <> raw.end()
                If strsame(i, (+it).first, strlen((+it).first), c.case_sensitive) Then
                    If c.case_sensitive Then
                        binded(i).push_back((+it).second)
                    Else
                        binded(strtolower(i)).push_back((+it).second)
                    End If
                    raw.erase(it)
                    Return True
                End If
                it += 1
            End While
            Return False
        End If
    End Function

    Public Function bind(ByVal ParamArray s() As String) As Boolean
#If 0 Then
        binded.clear()
#End If
        Dim r As Boolean = True
        For i As Int32 = 0 To array_size_i(s) - 1
            If Not bind(s(i)) Then
                r = False
            End If
        Next
        Return r
    End Function
End Class
