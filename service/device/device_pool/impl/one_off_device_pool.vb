
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils

Public NotInheritable Class one_off_device_pool
    Private Sub New()
    End Sub

    Public Shared Function [New](Of T)(ByVal i As idevice_creator(Of T),
                                       ByVal max_count As UInt32,
                                       ByVal identity As String) As one_off_device_pool(Of T)
        Return New one_off_device_pool(Of T)(i, max_count, identity)
    End Function

    Public Shared Function [New](Of T)(ByVal i As idevice_creator(Of T),
                                       ByVal max_count As UInt32) As one_off_device_pool(Of T)
        Return New one_off_device_pool(Of T)(i, max_count)
    End Function

    Public Shared Function [New](Of T)(ByVal i As idevice_creator(Of T),
                                       ByVal identity As String) As one_off_device_pool(Of T)
        Return New one_off_device_pool(Of T)(i, identity)
    End Function

    Public Shared Function [New](Of T)(ByVal i As idevice_creator(Of T)) As one_off_device_pool(Of T)
        Return New one_off_device_pool(Of T)(i)
    End Function
End Class

Public NotInheritable Class one_off_device_pool(Of T)
    Inherits device_pool(Of T)

    Private ReadOnly c As idevice_creator(Of T)

    Public Sub New(ByVal c As idevice_creator(Of T), ByVal max_count As UInt32, ByVal identity As String)
        MyBase.New(max_count, identity)
        assert(Not c Is Nothing)
        Me.c = c
    End Sub

    Public Sub New(ByVal c As idevice_creator(Of T), ByVal max_count As UInt32)
        Me.New(c, max_count, Nothing)
    End Sub

    Public Sub New(ByVal c As idevice_creator(Of T), ByVal identity As String)
        Me.New(c, uint32_0, identity)
    End Sub

    Public Sub New(ByVal c As idevice_creator(Of T))
        Me.New(c, uint32_0, Nothing)
    End Sub

    Protected Overrides Function enable_checker() As Boolean
        Return False
    End Function

    Protected Overrides Function get_free_count() As UInt32
        Return MyBase.get_free_count()
    End Function

    Protected Overrides Function get_device(ByRef r As idevice(Of T)) As Boolean
        Return create_new_device(r)
    End Function

    Protected Overrides Sub close_devices()
        MyBase.close_devices()
    End Sub

    Protected Overrides Function release_device(ByVal c As idevice(Of T)) As Boolean
        close_existing_device(c)
        Return True
    End Function

    Protected Overrides Function device_creator() As idevice_creator(Of T)
        Return c
    End Function
End Class
