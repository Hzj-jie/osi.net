
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector

Public Module _allocator
    Public Function make_allocator(Of T, PARA_T)(ByVal i As _do_val_ref(Of PARA_T, T, Boolean),
                                                 ByRef o As allocator(Of T, PARA_T)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = New allocator(Of T, PARA_T)(i)
            Return True
        End If
    End Function

    Public Function make_allocator(Of T, PARA_T)(ByVal i As _do_val_ref(Of PARA_T, T, Boolean)) _
                                                As allocator(Of T, PARA_T)
        Return New allocator(Of T, PARA_T)(i)
    End Function

    Public Function make_allocator(Of T, PARA_T)(ByVal i As Func(Of PARA_T, T),
                                                 ByRef o As allocator(Of T, PARA_T)) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o = New allocator(Of T, PARA_T)(i)
            Return True
        End If
    End Function

    Public Function make_allocator(Of T, PARA_T)(ByVal i As Func(Of PARA_T, T)) _
                                                As allocator(Of T, PARA_T)
        Return New allocator(Of T, PARA_T)(i)
    End Function
End Module

Public Class allocator(Of T, PARA_T)
    Private ReadOnly alloc As _do_val_ref(Of PARA_T, T, Boolean)

    Public Sub New(ByVal alloc As _do_val_ref(Of PARA_T, T, Boolean))
        assert(Not alloc Is Nothing)
        Me.alloc = alloc
    End Sub

    Public Sub New(ByVal alloc As Func(Of PARA_T, T))
        Me.New(Function(i As PARA_T, ByRef o As T) As Boolean
                   assert(Not alloc Is Nothing)
                   o = alloc(i)
                   Return True
               End Function)
    End Sub

    Public Function allocate(ByVal p As PARA_T, ByRef o As T) As Boolean
        Return alloc(p, o)
    End Function
End Class
