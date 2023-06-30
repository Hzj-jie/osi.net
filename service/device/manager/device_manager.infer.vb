
Option Strict On

Imports osi.service.argument

Public Class device_manager
    Private Sub New()
    End Sub

    Public Shared Function [get](Of T)(ByVal key As String, ByRef o As idevice(Of T)) As Boolean
        Return device_manager(Of T).get(key, o)
    End Function

    Public Shared Function [erase](Of T)(ByVal key As String,
                                         Optional ByRef o As idevice(Of T) = Nothing) As Boolean
        Return device_manager(Of T).erase(key, o)
    End Function

    Public Shared Function [erase](Of T, OT As idevice(Of T))(ByVal key As String, ByRef o As OT) As Boolean
        Return device_manager(Of T).erase(key, o)
    End Function

    Public Shared Function register(Of T)(ByVal key As String, ByVal d As idevice(Of T)) As Boolean
        Return device_manager(Of T).register(key, d)
    End Function

    Public Shared Function register(Of T)(ByVal v As var, ByVal p As idevice(Of T)) As Boolean
        Return device_manager(Of T).register(v, p)
    End Function

    Public Shared Function retire(Of T)(ByVal key As String,
                                        Optional ByRef o As idevice(Of T) = Nothing) As Boolean
        Return device_manager(Of T).retire(key, o)
    End Function

    Public Shared Function retire(Of T, OT As idevice(Of T))(ByVal key As String, ByRef o As OT) As Boolean
        Return device_manager(Of T).retire(key, o)
    End Function
End Class
