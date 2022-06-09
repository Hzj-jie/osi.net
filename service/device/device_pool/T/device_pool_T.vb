
Imports osi.root.connector

Partial Public Class device_pool(Of T)
    Inherits device_pool
    Implements idevice_pool(Of T)

    Public Event new_device_created(ByVal d As idevice(Of T)) Implements idevice_pool(Of T).new_device_created
    Public Event new_device_inserted(ByVal d As idevice(Of T)) Implements idevice_pool(Of T).new_device_inserted
    Public Event device_removed(ByVal d As idevice(Of T)) Implements idevice_pool(Of T).device_removed
    Private ReadOnly _closer As closer
    Private _checker As checker

    Protected Sub New(ByVal max_count As UInt32, ByVal identity As String)
        MyBase.New(max_count,
                   strcat(If(identity.null_or_empty(), type_info(Of T).name, identity), "_DEVICE_POOL"))
        _closer = New closer(Me)
    End Sub

    Protected Sub raise_new_device_created(ByVal d As idevice(Of T))
        _closer.insert(d)
        RaiseEvent new_device_created(d)
    End Sub

    Protected Sub raise_new_device_inserted(ByVal d As idevice(Of T))
        _closer.insert(d)
        RaiseEvent new_device_inserted(d)
    End Sub

    Protected Sub raise_device_removed(ByVal d As idevice(Of T))
        RaiseEvent device_removed(d)
    End Sub

    Public Function [get](ByRef r As idevice(Of T)) As Boolean Implements idevice_pool(Of T).get
        If expired() Then
            Return False
        Else
            While get_device(r)
                If assert(Not r Is Nothing) AndAlso r.is_valid() Then
                    Return True
                Else
                    assert(Not release(r))
                End If
            End While
            Return False
        End If
    End Function

    Public Function release(ByVal c As idevice(Of T)) As Boolean Implements idevice_pool(Of T).release
        If c Is Nothing Then
            Return False
        Else
            If c.is_valid() AndAlso release_device(c) Then
                Return True
            Else
                close_existing_device(c)
                Return False
            End If
        End If
    End Function
End Class
