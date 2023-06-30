
Imports osi.root.formation
Imports osi.root.connector

Partial Public Class pre_generated_device_pool(Of T)
    Inherits device_pool(Of T)

    Private ReadOnly q As qless2(Of idevice(Of T))

    Protected Sub New(ByVal p As idevice_exporter(Of T), ByVal max_count As UInt32, ByVal identity As String)
        MyBase.New(max_count, identity)
        q = New qless2(Of idevice(Of T))()
        assert(Not p Is Nothing)
        AddHandler p.new_device_exported, Sub(d As idevice(Of T), ByRef export_result As Boolean)
                                              export_result = insert_new_device(d)
                                          End Sub
        AddHandler closing, Sub()
                                p.stop()
                            End Sub
        p.start()
    End Sub

    Protected NotOverridable Overrides Function get_device(ByRef r As idevice(Of T)) As Boolean
        Return q.pop(r)
    End Function

    Protected NotOverridable Overrides Function release_device(ByVal c As idevice(Of T)) As Boolean
        q.emplace(c)
        Return True
    End Function

    Protected NotOverridable Overrides Function get_free_count() As UInt32
        Return q.size()
    End Function

    Private Function insert_new_device(ByVal c As idevice(Of T)) As Boolean
        assert(Not c Is Nothing)
        If Not expired() AndAlso c.is_valid() AndAlso increase_total_count() Then
            AddHandler c.closing, AddressOf decrease_total_count
            q.emplace(c)
            assert(Not limited_max_count() OrElse q.size() <= max_count())
            raise_new_device_inserted(c)
            Return True
        Else
            Return False
        End If
    End Function

    Protected NotOverridable Overrides Function device_creator() As idevice_creator(Of T)
        Return MyBase.device_creator()
    End Function

    Protected NotOverridable Overrides Sub close_devices()
        close_existing_devices(q)
    End Sub
End Class
