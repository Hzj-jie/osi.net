
Imports osi.root.constants
Imports osi.root.connector
Imports counter = osi.root.utils.counter

Partial Public Class device_pool
    Public Event closing() Implements idevice_pool.closing

    Public Sub close() Implements idevice_pool.close
        If exp.mark_in_use() Then
            RaiseEvent closing()
            close_devices()
        End If
        GC.SuppressFinalize(Me)
    End Sub

    Public Function expired() As Boolean Implements idevice_pool.expired
        Return exp.in_use()
    End Function

    Public Function identity() As String Implements idevice_pool.identity
        Return id
    End Function

    Public Function max_count() As UInt32 Implements idevice_pool.max_count
        Return mc
    End Function

    Public Function total_count() As UInt32 Implements idevice_pool.total_count
        Dim r As Int32 = 0
        r = (+count)
        Return If(r < 0, uint32_0, CUInt(r))
    End Function

    Public Function free_count() As UInt32 Implements idevice_pool.free_count
        If expired() Then
            Return uint32_0
        Else
            Dim r As Int32 = 0
            r = get_free_count()
            counter.increase(FREE_COUNT_COUNTER, r)
            Return r
        End If
    End Function
End Class
