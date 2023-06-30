
Option Strict On

Imports osi.service.argument

Public Class manager
    Private Sub New()
    End Sub

    Public Shared Function [get](Of T)(ByVal key As String, ByRef o As T) As Boolean
        Return manager(Of T).get(key, o)
    End Function

    Public Shared Function [erase](Of T)(ByVal key As String,
                                         Optional ByRef o As T = Nothing) As Boolean
        Return manager(Of T).erase(key, o)
    End Function

    Public Shared Function register(Of T)(ByVal key As String, ByVal d As T) As Boolean
        Return manager(Of T).register(key, d)
    End Function

    Public Shared Function register(Of T)(ByVal v As var, ByVal p As T) As Boolean
        Return manager(Of T).register(v, p)
    End Function
End Class
