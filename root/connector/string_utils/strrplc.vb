
Imports System.Runtime.CompilerServices

Public Module _strrplc
    <Extension()> Public Function strrplc(ByVal str As String,
                                          ByVal index As UInt32,
                                          ByVal len As UInt32,
                                          ByVal s As String,
                                          ByRef o As String) As Boolean
        If index + len > strlen(str) Then
            Return False
        Else
            If index + len = strlen(str) Then
                o = strleft(str, index) + s
            Else
                o = strleft(str, index) + s + strmid(str, index + len)
            End If
            Return True
        End If
    End Function

    <Extension()> Public Function strrplc(ByRef str As String,
                                          ByVal index As UInt32,
                                          ByVal len As UInt32,
                                          ByVal s As String) As String
        Dim o As String = Nothing
        assert(strrplc(str, index, len, s, o))
        str = o
        Return str
    End Function

    <Extension()> Public Function strrplc(ByVal str As String,
                                          ByVal index As UInt32,
                                          ByVal s As String,
                                          ByRef o As String) As Boolean
        Return strrplc(str, index, strlen(s), s, o)
    End Function

    <Extension()> Public Function strrplc(ByRef str As String, ByVal index As UInt32, ByVal s As String) As String
        Dim o As String = Nothing
        assert(strrplc(str, index, s, o))
        str = o
        Return str
    End Function

    <Extension()> Public Function strrplc(ByRef s As String, ByVal [from] As String, ByVal [to] As String) As String
        If Not String.IsNullOrEmpty(s) AndAlso Not String.IsNullOrEmpty([from]) Then
            s = s.Replace([from], [to])
        End If
        Return s
    End Function

    <Extension()> Public Function strrplc(ByRef s As String, ByVal [from] As Char, ByVal [to] As Char) As String
        If Not String.IsNullOrEmpty(s) Then
            s = s.Replace([from], [to])
        End If
        Return s
    End Function
End Module
