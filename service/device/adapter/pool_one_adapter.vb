
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.selector

Public Class pool_one_adapter(Of T)
    Private ReadOnly p As device_pool(Of T)
    Private ReadOnly get_new_instance As Func(Of idevice(Of T))
    Private ins As sync_thread_safe_lazier(Of idevice(Of T))

    Public Sub New(ByVal p As device_pool(Of T))
        assert(Not p Is Nothing)
        Me.p = p
        Me.get_new_instance = Function() As idevice(Of T)
                                  Dim o As idevice(Of T) = Nothing
                                  If p.get(o) Then
                                      Return o
                                  Else
                                      Return Nothing
                                  End If
                              End Function
        [set]()
    End Sub

    Private Sub [set]()
        ins = New sync_thread_safe_lazier(Of idevice(Of T))(get_new_instance)
    End Sub

    Public Function [get]() As idevice(Of T)
        assert(Not ins Is Nothing)
        Return ins.get()
    End Function

    Public Sub drop()
        drop(ins.get())
    End Sub

    Public Function drop(ByVal instance As idevice(Of T)) As Boolean
        Dim r As Boolean = False
        r = p.release(instance)
        [set]()
        Return r
    End Function
End Class
