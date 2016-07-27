
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.formation

Partial Public Class device_pool(Of T)
    ' Store the reference of all devices a device-pool generated, close them when the device-pool closing.
    Private Class closer
        Private ReadOnly p As device_pool(Of T)
        Private ReadOnly c As collectionless(Of idevice(Of T))

        Public Sub New(ByVal p As device_pool(Of T))
            assert(Not p Is Nothing)
            Me.p = p
            Me.c = New collectionless(Of idevice(Of T))()
            AddHandler p.closing, AddressOf close
        End Sub

        Public Sub insert(ByVal d As idevice(Of T))
            assert(Not d Is Nothing)
            Dim i As UInt32 = uint32_0
            i = c.emplace(d)
            AddHandler d.closing, Sub()
                                      c.erase(i)
                                  End Sub
            ' Expect p.exp.mark_in_use() is called before RaiseEvent p.closing()
            If p.expired() Then
                d.close()
            End If
        End Sub

        Private Sub close()
            Dim i As UInt32 = uint32_0
            While i < c.pool_size()
                Dim d As idevice(Of T) = Nothing
                d = c(i)
                If Not d Is Nothing Then
                    ' TODO: do not need to call c.erase (Added by AddHandler d.closing ...), we do not need to add the
                    ' index to free-list anymore, c.clear() should be more efficient.
                    d.close()
                End If
                i += 1
            End While
            c.clear()
        End Sub
    End Class
End Class
