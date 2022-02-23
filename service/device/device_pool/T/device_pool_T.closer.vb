
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public Class device_pool(Of T)
    ' Store the reference of all devices a device-pool generated, close them when the device-pool closing.
    Private NotInheritable Class closer
        Private ReadOnly p As device_pool(Of T)
        Private ReadOnly c As New collectionless(Of idevice(Of T))()

        Public Sub New(ByVal p As device_pool(Of T))
            assert(p IsNot Nothing)
            Me.p = p
            AddHandler p.closing, AddressOf close
        End Sub

        Public Sub insert(ByVal d As idevice(Of T))
            assert(d IsNot Nothing)
            Dim i As UInt32 = c.emplace(d)
            AddHandler d.closing,
                       Sub()
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
                ' TODO: do not need to call c.erase (Added by AddHandler d.closing ...), we do not need to add the
                ' index to free-list anymore, c.clear() should be more efficient.
                c.optional(i).if_present(Sub(d)
                                             d.close()
                                         End Sub)
                i += uint32_1
            End While
            ' Cannot clear. If d.closing is raised concurrently with p.closing, assertion will fail in
            ' collectionless.erase(uint32).
            ' c.clear()
        End Sub
    End Class
End Class
