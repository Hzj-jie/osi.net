
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Public Class device_pool_manager(Of T)
    Inherits manager(Of idevice_pool(Of T))

    Private Sub New()
    End Sub

    Public Shared Function retire(ByVal key As String, Optional ByRef o As idevice_pool(Of T) = Nothing) As Boolean
        If [erase](key, o) Then
            assert(o IsNot Nothing)
            o.close()
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function retire(Of OT As idevice_pool(Of T))(ByVal key As String, ByRef o As OT) As Boolean
        Dim p As idevice_pool(Of T) = Nothing
        Return retire(key, p) AndAlso
               cast(p, o)
    End Function

    Public Shared Function expired(ByVal key As String, Optional ByRef o As idevice_pool(Of T) = Nothing) As Boolean
        Return [get](key, o) AndAlso assert(o IsNot Nothing) AndAlso o.expired()
    End Function

    Public Shared Function expired(Of OT As idevice_pool(Of T))(ByVal key As String, ByRef o As OT) As Boolean
        Return [get](key, o) AndAlso assert(o IsNot Nothing) AndAlso o.expired()
    End Function

    Public Shared Function total_count(ByVal key As String, Optional ByRef o As idevice_pool(Of T) = Nothing) As UInt32
        If [get](key, o) Then
            assert(o IsNot Nothing)
            Return o.total_count()
        Else
            Return uint32_0
        End If
    End Function

    Public Shared Function total_count(Of OT As idevice_pool(Of T))(ByVal key As String, ByRef o As oT) As UInt32
        If [get](key, o) Then
            assert(o IsNot Nothing)
            Return o.total_count()
        Else
            Return uint32_0
        End If
    End Function

    Public Shared Function free_count(Of OT As idevice_pool(Of T))(ByVal key As String, ByRef o As OT) As UInt32
        If [get](key, o) Then
            assert(o IsNot Nothing)
            Return o.free_count()
        Else
            Return uint32_0
        End If
    End Function

    Public Overloads Shared Function [get](ByVal key As String,
                                           ByRef o As idevice(Of T),
                                           Optional ByRef p As idevice_pool(Of T) = Nothing) As Boolean
        Return [get](key, p) AndAlso
               assert(p IsNot Nothing) AndAlso
               p.get(o)
    End Function

    Public Overloads Shared Function [get](Of PT As idevice_pool(Of T)) _
                                          (ByVal key As String,
                                           ByRef o As idevice(Of T),
                                           ByRef p As PT) As Boolean
        Return [get](key, p) AndAlso
               assert(p IsNot Nothing) AndAlso
               p.get(o)
    End Function

    Public Shared Function release(ByVal key As String,
                                   ByVal i As idevice(Of T),
                                   Optional ByRef p As idevice_pool(Of T) = Nothing) As Boolean
        Return [get](key, p) AndAlso
               assert(p IsNot Nothing) AndAlso
               p.release(i)
    End Function

    Public Shared Function release(Of PT As idevice_pool(Of T)) _
                                  (ByVal key As String,
                                   ByVal i As idevice(Of T),
                                   ByRef p As PT) As Boolean
        Return [get](key, p) AndAlso
               assert(p IsNot Nothing) AndAlso
               p.release(i)
    End Function
End Class
