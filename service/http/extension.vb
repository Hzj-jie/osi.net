
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _extension
    <Extension()> Public Function parse_charset(ByVal content_type As String,
                                                ByRef result As String) As Boolean
        Return Not content_type.null_or_empty() AndAlso
               strsep(content_type,
                      Nothing,
                      result,
                      constants.headers.values.content_type.charset_prefix)
    End Function

    ' Return true if there is a 'charset=' in content_type, and the encoding is supported by the system.
    <Extension()> Public Function parse_encoding(ByVal content_type As String,
                                                 ByRef e As Text.Encoding) As Boolean
        Return parse_charset(content_type, content_type) AndAlso
               try_get_encoding(content_type, e)
    End Function
End Module
