
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.lock

Partial Public Class device_pool(Of T)
    Protected Overloads Sub close_existing_devices(ByVal q As qless2(Of idevice(Of T)))
        assert(Not q Is Nothing)
        While total_count() > uint32_0 OrElse Not q.empty()
            Dim c As idevice(Of T) = Nothing
            While q.pop(c) AndAlso assert(Not c Is Nothing)
                raise_device_removed(c)
                c.close()
            End While
            timeslice_sleep_wait_when(Function() q.empty() AndAlso total_count() > uint32_0)
        End While
        assert(q.empty())
    End Sub

    Protected Function create_new_device(ByRef r As idevice(Of T)) As Boolean
        If increase_total_count() Then
            If device_creator().create_valid_device(r) Then
                AddHandler r.closing, AddressOf decrease_total_count
                raise_new_device_created(r)
                Return True
            Else
                decrease_total_count()
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Protected Sub close_existing_device(ByVal c As idevice(Of T))
        raise_device_removed(c)
        c.close()
    End Sub
End Class
