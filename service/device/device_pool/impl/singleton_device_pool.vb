
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public NotInheritable Class singleton_device_pool
    Private Sub New()
    End Sub

    Public Shared Function [New](Of T)(ByVal d As idevice(Of T), ByVal identity As String) As singleton_device_pool(Of T)
        Return New singleton_device_pool(Of T)(d, identity)
    End Function

    Public Shared Function [New](Of T)(ByVal d As idevice(Of T)) As singleton_device_pool(Of T)
        Return New singleton_device_pool(Of T)(d)
    End Function
End Class

Public NotInheritable Class singleton_device_pool(Of T)
    Inherits device_pool(Of T)

    Private ReadOnly d As idevice(Of T)

    Public Sub New(ByVal d As idevice(Of T), ByVal identity As String)
        MyBase.New(uint32_0, identity)
        assert(d IsNot Nothing)
        Me.d = d
        assert(increase_total_count())
    End Sub

    Public Sub New(ByVal d As idevice(Of T))
        Me.New(d, assert_which.of(d).is_not_null().GetType().Name())
    End Sub

    Protected Overrides Function enable_checker() As Boolean
        Return False
    End Function

    Protected Overrides Function get_free_count() As UInt32
        If d.is_valid() Then
            Return uint32_1
        Else
            zero_total_count()
            Return uint32_0
        End If
    End Function

    Protected Overrides Function get_device(ByRef r As idevice(Of T)) As Boolean
        If d.is_valid() Then
            r = d
            Return True
        Else
            Return False
        End If
    End Function

    Protected Overrides Function release_device(ByVal c As idevice(Of T)) As Boolean
        If object_compare(c, d) = 0 AndAlso d.is_valid() Then
            Return True
        Else
            zero_total_count()
            Return False
        End If
    End Function

    Protected Overrides Sub close_devices()
        d.close()
    End Sub
End Class
