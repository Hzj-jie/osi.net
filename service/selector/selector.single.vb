
Imports osi.root
Imports osi.root.template
Imports osi.root.connector
Imports osi.root.formation

Public Module _selector
    Public Function make_selector(Of T, PARA_T As IComparable(Of PARA_T))(ByVal i As allocator(Of T, PARA_T)) _
                                                                         As selector(Of T, PARA_T)
        Return New selector(Of T, PARA_T)(i)
    End Function

    Public Function make_lazier(Of T, PARA_T As IComparable(Of PARA_T))(ByVal i As allocator(Of T, PARA_T)) _
                                                                       As lazier(Of T, PARA_T)
        Return New lazier(Of T, PARA_T)(i)
    End Function
End Module

Public Class selector_alloc(Of T, PARA_T As IComparable(Of PARA_T))
    Inherits __do(Of allocator(Of T, PARA_T), selector(Of T, PARA_T))

    Public Overrides Function at(ByRef k As allocator(Of T, PARA_T)) As selector(Of T, PARA_T)
        Return make_selector(k)
    End Function
End Class

Public Class selector(Of T, PARA_T As IComparable(Of PARA_T))
    Inherits selector(Of T, 
                         PARA_T, 
                         singleton(Of T, PARA_T), 
                         singleton_alloc(Of T, PARA_T), 
                         singleton_invoke(Of T, PARA_T))

    Public Sub New(ByVal alloc As allocator(Of T, PARA_T))
        MyBase.New(alloc)
    End Sub
End Class

Public Class lazier_alloc(Of T, PARA_T As IComparable(Of PARA_T))
    Inherits __do(Of allocator(Of T, PARA_T), lazier(Of T, PARA_T))

    Public Overrides Function at(ByRef k As allocator(Of T, PARA_T)) As lazier(Of T, PARA_T)
        Return make_lazier(k)
    End Function
End Class

Public Class lazier(Of T, PARA_T As IComparable(Of PARA_T))
    Inherits selector(Of T, 
                         PARA_T, 
                         multiton(Of T, PARA_T), 
                         multiton_alloc(Of T, PARA_T), 
                         multiton_invoke(Of T, PARA_T))

    Public Sub New(ByVal alloc As allocator(Of T, PARA_T))
        MyBase.New(alloc)
    End Sub
End Class

Public Class selector(Of T,
                         PARA_T As IComparable(Of PARA_T),
                         CONTAINER_T,
                         CONTAINER_T_ALLOC As __do(Of allocator(Of T, PARA_T), PARA_T, CONTAINER_T),
                         CONTAINER_T_INVOKE As __do(Of CONTAINER_T, T, Boolean))
    Private Shared ReadOnly container_allocator As CONTAINER_T_ALLOC
    Private Shared ReadOnly container_invoker As CONTAINER_T_INVOKE
    Private ReadOnly m As unique_strong_map(Of PARA_T, CONTAINER_T)
    Private ReadOnly alloc As allocator(Of T, PARA_T)

    Shared Sub New()
        container_allocator = connector.alloc(Of CONTAINER_T_ALLOC)()
        container_invoker = connector.alloc(Of CONTAINER_T_INVOKE)()
    End Sub

    Public Sub New(ByVal alloc As allocator(Of T, PARA_T))
        assert(Not alloc Is Nothing)
        Me.alloc = alloc
        Me.m = New unique_strong_map(Of PARA_T, CONTAINER_T)()
    End Sub

    Public Function size() As UInt32
        Return m.size()
    End Function

    Public Function empty() As Boolean
        Return m.empty()
    End Function

    Public Function [erase](ByVal p As PARA_T) As Boolean
        Return m.erase(p)
    End Function

    Public Function [erase](ByVal ps As vector(Of PARA_T)) As Boolean
        Return m.erase(ps)
    End Function

    Public Function [select](ByVal p As PARA_T, ByRef o As T) As Boolean
        Dim s As CONTAINER_T = Nothing
        s = m.generate(p, Function() container_allocator(alloc, p))
        assert(Not s Is Nothing)
        Return container_invoker.at(s, o)
    End Function
End Class
