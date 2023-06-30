
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Public Class multiton_alloc(Of T, PARA_T)
    Inherits __do(Of allocator(Of T, PARA_T), PARA_T, multiton(Of T, PARA_T))

    Public Overrides Function at(ByRef i As allocator(Of T, PARA_T), ByRef j As PARA_T) As multiton(Of T, PARA_T)
        Return make_multiton(i, j)
    End Function
End Class

Public Class multiton_invoke(Of T, PARA_T)
    Inherits __do(Of multiton(Of T, PARA_T), T, Boolean)

    Public Overrides Function at(ByRef i As multiton(Of T, PARA_T), ByRef j As T) As Boolean
        assert(Not i Is Nothing)
        Return i.allocate(j)
    End Function
End Class

Public Module _multiton
    Public Function make_multiton(Of T, PARA_T)(ByVal alloc As allocator(Of T, PARA_T),
                                                ByVal parameter As PARA_T) As multiton(Of T, PARA_T)
        Return New multiton(Of T, PARA_T)(alloc, parameter)
    End Function
End Module

Public Class multiton(Of T, PARA_T)
    Private ReadOnly alloc As allocator(Of T, PARA_T)
    Private ReadOnly parameter As PARA_T

    Public Sub New(ByVal alloc As allocator(Of T, PARA_T), ByVal parameter As PARA_T)
        assert(Not alloc Is Nothing)
        Me.alloc = alloc
        Me.parameter = parameter
    End Sub

    Public Function allocate(ByRef o As T) As Boolean
        Return alloc.allocate(parameter, o)
    End Function
End Class
