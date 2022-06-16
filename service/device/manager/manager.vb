
Option Strict On

Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.formation
Imports osi.service.argument
Imports utils = osi.root.utils

Public Class manager(Of T)
    Protected Sub New()
    End Sub

    Private Shared ReadOnly m As unique_strong_map(Of String, T)

    Shared Sub New()
        assert(DirectCast(Nothing, Boolean) = False)
        assert(DirectCast(Nothing, Object) Is Nothing)
        assert(DirectCast(Nothing, Int64) = 0)
        m = New unique_strong_map(Of String, T)()
    End Sub

    Public Shared Function [get](ByVal key As String, ByRef o As T) As Boolean
        If key.null_or_empty() Then
            Return False
        Else
            Return m.get(key, o)
        End If
    End Function

    Public Shared Function [get](ByVal key As String) As T
        Dim o As T = Nothing
        assert([get](key, o))
        Return o
    End Function

    Public Shared Function [get](Of OT As T)(ByVal key As String, ByRef o As OT) As Boolean
        Dim d As T = Nothing
        Return [get](key, d) AndAlso cast(d, o)
    End Function

    Public Shared Function [get](Of OT As T)(ByVal key As String) As OT
        Dim o As OT = Nothing
        assert([get](key, o))
        Return o
    End Function

    Public Shared Function [erase](ByVal key As String, Optional ByRef o As T = Nothing) As Boolean
        If key.null_or_empty() Then
            Return False
        Else
            Return m.erase(key, o)
        End If
    End Function

    Public Shared Function [erase](Of OT As T)(ByVal key As String, ByRef o As OT) As Boolean
        Dim p As T = Nothing
        Return [erase](key, p) AndAlso
               cast(p, o)
    End Function

    Public Shared Function register(ByVal key As String, ByVal p As T) As Boolean
        If key.null_or_empty() OrElse p Is Nothing Then
            Return False
        Else
            Return m.set(key, p)
        End If
    End Function

    Public Shared Function register(ByVal v As var, ByVal p As T) As Boolean
        If v Is Nothing Then
            Return False
        Else
            Const name As String = "name"
            v.bind(name)
            Return register(v(name), p)
        End If
    End Function
End Class
