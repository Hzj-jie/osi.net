
Imports osi.root.template
Imports osi.root.connector

Public Class singleton_alloc(Of T, PARA_T)
    Inherits __do(Of allocator(Of T, PARA_T), PARA_T, singleton(Of T, PARA_T))

    Public Overrides Function at(ByRef i As allocator(Of T, PARA_T),
                                 ByRef j As PARA_T) As singleton(Of T, PARA_T)
        Return make_singleton(i, j)
    End Function
End Class

Public Class singleton_invoke(Of T, PARA_T)
    Inherits __do(Of singleton(Of T, PARA_T), T, Boolean)

    Public Overrides Function at(ByRef i As singleton(Of T, PARA_T), ByRef j As T) As Boolean
        assert(Not i Is Nothing)
        Return i.allocate(j)
    End Function
End Class

Public Module _singleton
    Public Function make_singleton(Of T, PARA_T)(ByVal alloc As allocator(Of T, PARA_T),
                                                 ByVal parameter As PARA_T) As singleton(Of T, PARA_T)
        Return New singleton(Of T, PARA_T)(alloc, parameter)
    End Function
End Module

Public Class singleton(Of T, PARA_T)
    Private ReadOnly l As sync_thread_safe_lazier(Of T)
    Private result As Boolean

    Public Sub New(ByVal alloc As allocator(Of T, PARA_T), ByVal parameter As PARA_T)
        assert(Not alloc Is Nothing)
        Me.l = New sync_thread_safe_lazier(Of T)(Function() As T
                                                     Dim r As T = Nothing
                                                     result = alloc.allocate(parameter, r)
                                                     Return r
                                                 End Function)
    End Sub

    Public Function allocate(ByRef o As T) As Boolean
        o = l.get()
        Return result
    End Function
End Class
