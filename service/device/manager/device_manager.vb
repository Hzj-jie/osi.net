
Option Strict On

Imports osi.root.connector

Public Class device_manager(Of T)
    Inherits manager(Of idevice(Of T))

    Private Sub New()
    End Sub

    Public Shared Function retire(ByVal key As String, Optional ByRef o As idevice(Of T) = Nothing) As Boolean
        If [erase](key, o) AndAlso assert(o IsNot Nothing) Then
            o.close()
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function retire(Of OT As idevice(Of T))(ByVal key As String, ByRef o As OT) As Boolean
        Dim p As idevice(Of T) = Nothing
        Return retire(key, p) AndAlso
               cast(p, o)
    End Function
End Class
