
Imports osi.root.lock
Imports osi.root.constants
Imports osi.root.connector

Public Class unmanaged_pool(Of T)
    Protected Overridable Function create() As T
        Return alloc(Of T)()
    End Function

    Private ReadOnly q As slimqless2(Of T)
    Private ReadOnly max_size As UInt32
    Private ReadOnly size As atomic_int

    Public Sub New(ByVal max_size As UInt32)
        Me.q = New slimqless2(Of T)()
        Me.max_size = max_size
        Me.size = New atomic_int()
    End Sub

    Public Sub New()
        Me.New(default_pool_max_size)
    End Sub

    Public Function [get](ByRef o As T) As Boolean
        If q.pop(o) Then
            Return True
        Else
            If size.increment() <= max_size Then
                o = create()
                assert(Not o Is Nothing)
                Return True
            Else
                assert(size.decrement() >= 0)
                Return False
            End If
        End If
    End Function

    Public Sub release(ByVal i As T)
        assert(Not i Is Nothing)
        q.emplace(i)
    End Sub
End Class
