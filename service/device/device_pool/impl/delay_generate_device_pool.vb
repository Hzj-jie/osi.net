
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.utils
Imports osi.root.lock

Public NotInheritable Class delay_generate_device_pool
    Private Sub New()
    End Sub

    Public Shared Function [New](Of T)(ByVal i As idevice_creator(Of T),
                                       ByVal max_count As UInt32,
                                       ByVal identity As String) As delay_generate_device_pool(Of T)
        Return New delay_generate_device_pool(Of T)(i, max_count, identity)
    End Function

    Public Shared Function [New](Of T)(ByVal i As idevice_creator(Of T),
                                       ByVal max_count As UInt32) As delay_generate_device_pool(Of T)
        Return New delay_generate_device_pool(Of T)(i, max_count)
    End Function

    Public Shared Function [New](Of T)(ByVal i As idevice_creator(Of T),
                                       ByVal identity As String) As delay_generate_device_pool(Of T)
        Return New delay_generate_device_pool(Of T)(i, identity)
    End Function

    Public Shared Function [New](Of T)(ByVal i As idevice_creator(Of T)) As delay_generate_device_pool(Of T)
        Return New delay_generate_device_pool(Of T)(i)
    End Function
End Class

Public NotInheritable Class delay_generate_device_pool(Of T)
    Inherits delay_generate_device_pool(Of T, _true)

    Public Sub New(ByVal c As idevice_creator(Of T), ByVal max_count As UInt32, ByVal identity As String)
        MyBase.New(c, max_count, identity)
    End Sub

    Public Sub New(ByVal c As idevice_creator(Of T), ByVal max_count As UInt32)
        MyBase.New(c, max_count)
    End Sub

    Public Sub New(ByVal c As idevice_creator(Of T), ByVal identity As String)
        MyBase.New(c, identity)
    End Sub

    Public Sub New(ByVal c As idevice_creator(Of T))
        MyBase.New(c)
    End Sub
End Class

Public Class delay_generate_device_pool(Of T, REUSABLE As _boolean)
    Inherits device_pool(Of T)

    Private Shared ReadOnly device_reusable As Boolean
    Private ReadOnly c As idevice_creator(Of T)
    Private ReadOnly q As qless2(Of idevice(Of T))

    Shared Sub New()
        device_reusable = +(alloc(Of REUSABLE)())
    End Sub

    Public Sub New(ByVal c As idevice_creator(Of T), ByVal max_count As UInt32, ByVal identity As String)
        MyBase.New(max_count, identity)
        assert(Not c Is Nothing)
        Me.c = c
        If device_reusable Then
            Me.q = New qless2(Of idevice(Of T))()
        End If
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

    Protected NotOverridable Overrides Function get_device(ByRef r As idevice(Of T)) As Boolean
        Return (device_reusable AndAlso assert(Not q Is Nothing) AndAlso q.pop(r)) OrElse
               create_new_device(r)
    End Function

    Protected NotOverridable Overrides Function release_device(ByVal c As idevice(Of T)) As Boolean
        If device_reusable Then
            assert(Not q Is Nothing)
            q.push(c)
            Return True
        Else
            Return False
        End If
    End Function

    Protected NotOverridable Overrides Sub close_devices()
        If device_reusable Then
            close_existing_devices(q)
        End If
    End Sub

    Protected NotOverridable Overrides Function get_free_count() As UInt32
        If device_reusable Then
            assert(Not q Is Nothing)
            Return q.size()
        Else
            Return 0
        End If
    End Function

    Protected Overrides Function device_creator() As idevice_creator(Of T)
        Return c
    End Function

    Protected NotOverridable Overrides Function auto_device_exporter() As iauto_device_exporter(Of T)
        assert(False)
        Return Nothing
    End Function
End Class
