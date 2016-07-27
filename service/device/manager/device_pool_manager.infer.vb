
Option Strict On

Imports osi.service.argument

Public Class device_pool_manager
    Private Sub New()
    End Sub

    Public Shared Function [get](Of T)(ByVal key As String, ByRef o As idevice_pool(Of T)) As Boolean
        Return device_pool_manager(Of T).get(key, o)
    End Function

    Public Shared Function [get](Of T, OT As idevice_pool(Of T))(ByVal key As String, ByRef o As OT) As Boolean
        Return device_pool_manager(Of T).get(key, o)
    End Function

    Public Shared Function [erase](Of T)(ByVal key As String,
                                         Optional ByRef o As idevice_pool(Of T) = Nothing) As Boolean
        Return device_pool_manager(Of T).erase(key, o)
    End Function

    Public Shared Function [erase](Of T, OT As idevice_pool(Of T))(ByVal key As String, ByRef o As OT) As Boolean
        Return device_pool_manager(Of T).erase(key, o)
    End Function

    Public Shared Function register(Of T)(ByVal key As String, ByVal d As idevice_pool(Of T)) As Boolean
        Return device_pool_manager(Of T).register(key, d)
    End Function

    Public Shared Function register(Of T)(ByVal v As var, ByVal p As idevice_pool(Of T)) As Boolean
        Return device_pool_manager(Of T).register(v, p)
    End Function

    Public Shared Function retire(Of T)(ByVal key As String,
                                        Optional ByRef o As idevice_pool(Of T) = Nothing) As Boolean
        Return device_pool_manager(Of T).retire(key, o)
    End Function

    Public Shared Function retire(Of T, OT As idevice_pool(Of T))(ByVal key As String, ByRef o As OT) As Boolean
        Return device_pool_manager(Of T).retire(key, o)
    End Function

    Public Shared Function expired(Of T)(ByVal key As String,
                                         Optional ByRef o As idevice_pool(Of T) = Nothing) As Boolean
        Return device_pool_manager(Of T).expired(key, o)
    End Function

    Public Shared Function total_count(Of T)(ByVal key As String,
                                             Optional ByRef o As idevice_pool(Of T) = Nothing) As UInt32
        Return device_pool_manager(Of T).total_count(key, o)
    End Function

    Public Shared Function free_count(Of T)(ByVal key As String,
                                            Optional ByRef o As idevice_pool(Of T) = Nothing) As UInt32
        Return device_pool_manager(Of T).free_count(key, o)
    End Function

    Public Shared Function [get](Of T)(ByVal key As String,
                                       ByRef o As idevice(Of T),
                                       Optional ByRef p As idevice_pool(Of T) = Nothing) As Boolean
        Return device_pool_manager(Of T).get(key, o, p)
    End Function

    Public Shared Function [get](Of T, PT As idevice_pool(Of T)) _
                                (ByVal key As String,
                                 ByRef o As idevice(Of T),
                                 ByRef p As PT) As Boolean
        Return device_pool_manager(Of T).get(key, o, p)
    End Function

    Public Shared Function release(Of T)(ByVal key As String,
                                         ByVal i As idevice(Of T),
                                         Optional ByRef p As idevice_pool(Of T) = Nothing) As Boolean
        Return device_pool_manager(Of T).release(key, i, p)
    End Function

    Public Shared Function release(Of T, PT As idevice_pool(Of T)) _
                                  (ByVal key As String,
                                   ByVal i As idevice(Of T),
                                   ByRef p As PT) As Boolean
        Return device_pool_manager(Of T).release(key, i, p)
    End Function
End Class
