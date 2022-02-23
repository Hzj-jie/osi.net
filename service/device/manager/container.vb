
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock

Public Class container(Of T)
    Inherits disposer

    Private ReadOnly pool As idevice_pool(Of T)
    Private ReadOnly dev As idevice(Of T)
    Private ReadOnly d As T

    Private Sub New(ByVal pool As idevice_pool(Of T),
                    ByVal dev As idevice(Of T),
                    ByVal d As T)
        assert((d Is Nothing) Xor (dev Is Nothing))
        assert(pool Is Nothing OrElse dev IsNot Nothing)
        Me.pool = pool
        Me.dev = dev
        If d Is Nothing Then
            Me.d = dev.get()
        Else
            Me.d = d
        End If
    End Sub

    Public Shared Function create(ByVal key As String, ByRef o As container(Of T)) As Boolean
        Dim pool As idevice_pool(Of T) = Nothing
        Dim dev As idevice(Of T) = Nothing
        Dim d As T = Nothing
        If device_pool_manager(Of T).get(key, dev, pool) OrElse
           device_manager(Of T).get(key, dev) OrElse
           manager(Of T).get(key, d) Then
            o = New container(Of T)(pool, dev, d)
            Return True
        Else
            Return False
        End If
    End Function

    Public Function device_pool() As idevice_pool(Of T)
        Return pool
    End Function

    Public Function device() As idevice(Of T)
        Return dev
    End Function

    Public Function instance() As T
        Return d
    End Function

    Public Shared Shadows Operator +(ByVal this As container(Of T)) As T
        Return If(this Is Nothing, Nothing, this.instance())
    End Operator

    Public Sub release()
        MyBase.dispose()
    End Sub

    Public Function released() As Boolean
        Return MyBase.disposed()
    End Function

    Protected Overrides Sub disposer()
        If pool IsNot Nothing Then
            pool.release(dev)
        End If
    End Sub
End Class
