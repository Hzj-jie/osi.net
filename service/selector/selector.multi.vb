
Imports osi.root.connector
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.utils

Public Class lazier(Of T, TYPE_T As IComparable(Of TYPE_T), PARA_T As IComparable(Of PARA_T))
    Inherits selector(Of T, 
                         TYPE_T, 
                         PARA_T, 
                         multiton(Of T, PARA_T), 
                         multiton_alloc(Of T, PARA_T), 
                         multiton_invoke(Of T, PARA_T), 
                         lazier(Of T, PARA_T), 
                         lazier_alloc(Of T, PARA_T))
End Class

Public Class selector(Of T, TYPE_T As IComparable(Of TYPE_T), PARA_T As IComparable(Of PARA_T))
    Inherits selector(Of T, 
                         TYPE_T, 
                         PARA_T, 
                         singleton(Of T, PARA_T), 
                         singleton_alloc(Of T, PARA_T), 
                         singleton_invoke(Of T, PARA_T), 
                         selector(Of T, PARA_T), 
                         selector_alloc(Of T, PARA_T))
End Class

Public Class selector(Of T,
                         TYPE_T As IComparable(Of TYPE_T),
                         PARA_T As IComparable(Of PARA_T),
                         CONTAINER_T,
                         CONTAINER_T_ALLOC As __do(Of allocator(Of T, PARA_T), PARA_T, CONTAINER_T),
                         CONTAINER_T_INVOKE As __do(Of CONTAINER_T, T, Boolean),
                         SELECTOR_T As selector(Of T, PARA_T, CONTAINER_T, CONTAINER_T_ALLOC, CONTAINER_T_INVOKE),
                         SELECTOR_T_ALLOC As __do(Of allocator(Of T, PARA_T), SELECTOR_T))
    Private Shared ReadOnly selector_allocator As SELECTOR_T_ALLOC
    Private ReadOnly m As unique_strong_map(Of TYPE_T, SELECTOR_T)

    Shared Sub New()
        selector_allocator = alloc(Of SELECTOR_T_ALLOC)()
    End Sub

    Public Sub New()
        m = New unique_strong_map(Of TYPE_T, SELECTOR_T)()
    End Sub

    Public Function size() As UInt32
        Return m.size()
    End Function

    Public Function empty() As Boolean
        Return m.empty()
    End Function

    Public Sub clear()
        m.clear()
    End Sub

    Public Function register(ByVal i As TYPE_T, ByVal a As allocator(Of T, PARA_T)) As Boolean
        If a Is Nothing Then
            Return False
        Else
            Return m.set(i, selector_allocator(a))
        End If
    End Function

    Public Function registered(ByVal i As TYPE_T) As Boolean
        Return m.exist(i)
    End Function

    Public Function [erase](ByVal t As TYPE_T) As Boolean
        Return m.erase(t)
    End Function

    Public Function [erase](ByVal ts As vector(Of TYPE_T)) As Boolean
        Return m.erase(ts)
    End Function

    Public Function [erase](ByVal t As TYPE_T, ByVal p As PARA_T) As Boolean
        Dim s As SELECTOR_T = Nothing
        Return m.get(t, s) AndAlso assert(s IsNot Nothing) AndAlso s.erase(p)
    End Function

    Public Function [erase](ByVal t As TYPE_T, ByVal ps As vector(Of PARA_T)) As Boolean
        Dim s As SELECTOR_T = Nothing
        Return m.get(t, s) AndAlso assert(s IsNot Nothing) AndAlso s.erase(ps)
    End Function

    Public Function [select](ByVal i As TYPE_T, ByVal p As PARA_T, ByRef o As T) As Boolean
        Dim s As SELECTOR_T = Nothing
        If m.get(i, s) Then
            assert(s IsNot Nothing)
            Return s.select(p, o)
        Else
            Return False
        End If
    End Function
End Class
