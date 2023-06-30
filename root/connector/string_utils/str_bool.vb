
Imports System.Runtime.CompilerServices

Public Module _str_bool
    Public Function str_bool(ByVal s As String, ByRef o As Boolean) As Boolean
        If s.null_or_whitespace() Then
            Return False
        Else
            If Not Boolean.TryParse(s, o) Then
                s = s.Trim()
                o = strsame(s, "1", False) OrElse
                    strsame(s, "true", False) OrElse
                    strsame(s, "yes", False)
            End If
            Return True
        End If
    End Function

    Public Function str_bool(ByVal s As String) As Boolean
        Dim o As Boolean = False
        assert(str_bool(s, o))
        Return o
    End Function

    <Extension()> Public Function [true](ByVal s As String) As Boolean
        Dim o As Boolean = False
        Return str_bool(s, o) AndAlso
               o
    End Function

    <Extension()> Public Function [false](ByVal s As String) As Boolean
        Return Not s.true()
    End Function
End Module
