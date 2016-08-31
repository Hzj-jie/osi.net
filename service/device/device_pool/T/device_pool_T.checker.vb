
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure

Partial Public Class device_pool(Of T)
    Protected Overridable Function enable_checker() As Boolean
        Return True
    End Function

    Public Function attach_checker(ByVal c As checker) As Boolean
        If enable_checker() Then
            Return atomic.set_if_nothing(_checker, c)
        Else
            Return False
        End If
    End Function

    Public Function attach_checker(Optional ByVal wait_ms As Int64 = constants.default_checker_interval_ms) As Boolean
        Dim c As checker = Nothing
        c = New checker(Me, wait_ms)
        If attach_checker(c) Then
            Return True
        Else
            c.stop()
            Return False
        End If
    End Function

    Public Function clear_checker() As Boolean
        Dim c As checker = Nothing
        If atomic.clear_if_not_nothing(_checker, c) AndAlso assert(Not c Is Nothing) Then
            c.stop()
            Return True
        Else
            Return False
        End If
    End Function

    Public Class checker
        Inherits disposer

        Private ReadOnly p As device_pool(Of T)
        Private exp As singleentry

        Public Sub New(ByVal p As device_pool(Of T), ByVal interval_ms As Int64)
            assert(Not p Is Nothing)
            assert(interval_ms > 0)
            Me.p = p
            begin_lifetime_event_comb(expiration_controller.[New](AddressOf expired),
                                      Function() As Boolean
                                          Dim i As UInt32 = 0
                                          i = p.free_count()
                                          Dim c As idevice(Of T) = Nothing
                                          While i > 0 AndAlso p.[get](c)
                                              i -= uint32_1
                                              c.check()
                                              p.release(c)
                                          End While
                                          Return waitfor(interval_ms)
                                      End Function)
        End Sub

        Public Sub [stop]()
            exp.mark_in_use()
        End Sub

        Private Function expired() As Boolean
            Return p.expired() OrElse exp.in_use()
        End Function

        Protected Overrides Sub disposer()
            [stop]()
        End Sub
    End Class
End Class
