
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.utils

Public Module _extension
    <Extension()> Public Function parse_charset(ByVal content_type As String,
                                                ByRef result As String) As Boolean
        Return Not String.IsNullOrEmpty(content_type) AndAlso
               strsep(content_type,
                      Nothing,
                      result,
                      constants.headers.values.content_type.charset_prefix)
    End Function

    'return nothing if charset has not been found in the content_type
    <Extension()> Public Function parse_charset(ByVal content_type As String) As String
        Dim o As String = Nothing
        If parse_charset(content_type, o) Then
            Return o
        Else
            Return Nothing
        End If
    End Function

    'return true if there is a 'charset=' in content_type, and the encoding is supported by the system,
    'otherwise false, but it will always set the e to default_encoding if failed
    <Extension()> Public Function parse_encoding(ByVal content_type As String,
                                                 ByRef e As Text.Encoding) As Boolean
        Return (parse_charset(content_type, content_type) AndAlso
                try_get_encoding(content_type, e)) OrElse
               (Not eva(e, default_encoding))
    End Function
End Module
