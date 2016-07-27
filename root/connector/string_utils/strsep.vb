
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _strsep
    Private Function str_sep(ByVal input As String,
                             ByRef f As String,
                             ByRef s As String,
                             ByVal sep As String,
                             ByVal indexof As Func(Of String, String, Boolean, Int64),
                             ByVal carecase As Boolean) As Boolean
        assert(Not indexof Is Nothing)
        Dim i As Int64 = 0
        i = indexof(input, sep, carecase)
        If i = npos Then
            Return False
        Else
            Dim f1 As String = Nothing
            Dim s1 As String = Nothing
            f1 = strleft(input, i)
            s1 = strmid(input, i + strlen(sep))
            'no matter whether f is input or s is input, it's safe
            f = f1
            s = s1
            Return True
        End If
    End Function

    <Extension()> Public Function strsep(ByVal input As String,
                                         ByRef f As String,
                                         ByRef s As String,
                                         ByVal sep As String,
                                         Optional ByVal carecase As Boolean = True) As Boolean
        Return str_sep(input, f, s, sep, AddressOf strindexof, carecase)
    End Function

    <Extension()> Public Function strrsep(ByVal input As String,
                                          ByRef f As String,
                                          ByRef s As String,
                                          ByVal sep As String,
                                          Optional ByVal carecase As Boolean = True) As Boolean
        Return str_sep(input, f, s, sep, AddressOf strlastindexof, carecase)
    End Function
End Module
