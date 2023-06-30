
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class mapheap(Of MAP_KEY As IComparable(Of MAP_KEY), HEAP_KEY As IComparable(Of HEAP_KEY))
    Implements ICloneable, ICloneable(Of mapheap(Of MAP_KEY, HEAP_KEY))

    Private NotInheritable Class heap2
        Inherits heap(Of pair(Of HEAP_KEY, MAP_KEY))

        Private ReadOnly m As map(Of MAP_KEY, Int64) = Nothing

        Public Sub New(ByVal m As map(Of MAP_KEY, Int64))
            assert(Not m Is Nothing)
            Me.m = m
        End Sub

        <copy_constructor>
        Public Sub New(ByVal h As heap2, ByVal m As map(Of MAP_KEY, Int64))
            MyBase.New(h)
            assert(Not m Is Nothing)
            Me.m = m
        End Sub

        Public Overrides Function [erase](ByVal index As Int64) As Boolean
            assert(Not at(index) Is Nothing, "index " + Convert.ToString(index) + " is not in heap.")
            Dim k As MAP_KEY = Nothing
            k = at(index).second
            assert(MyBase.erase(index), "mybase.erase(" + Convert.ToString(index) + ") returns false.")
            If index = size() Then
                'mybase will not call update here, if the removed index is the last one
                assert(m.erase(k), "_map.erase(" + Convert.ToString(k) + ") returns false.")
            End If
            assert(size() = m.size(), "heap.size() <> _map.size()")
            Return True
        End Function

        Public Overrides Function update(ByVal index As Int64, ByVal value As pair(Of HEAP_KEY, MAP_KEY)) As Int64
            assert(index <> npos, "should not call update with index = npos.")
            assert(Not value Is Nothing, "value is nothing.")
            If Not at(index) Is Nothing Then
                assert(m.erase(at(index).second) _
                            , "_map.erase(" + Convert.ToString(at(index).second) + ") returns false.")
            End If
            Dim rtn As Int64 = 0
            rtn = MyBase.update(index, value)
            assert(rtn <> npos, "cannot update index " + Convert.ToString(index))
            m(value.second) = rtn
            assert(size() = m.size(), "heap.size() <> _map.size()")
            Return rtn
        End Function

        Protected Overrides Function swap(ByVal first As Int64, ByVal second As Int64) As Boolean
            assert(Not m Is Nothing, "_map is nothing before swap")
            If Not MyBase.swap(first, second) Then
                Return False
            End If
            m(at(first).second) = first
            If first <> second Then
                m(at(second).second) = second
            End If
            Return True
        End Function
    End Class

    Private Shared ReadOnly _end As iterator = iterator.end
    Private ReadOnly h As heap2
    Private ReadOnly m As map(Of MAP_KEY, Int64) = Nothing

    Public Function begin() As iterator
        Return find(m.begin())
    End Function

    Public Function rbegin() As iterator
        Return find(m.rbegin())
    End Function

    Public Function [end]() As iterator
        Return _end
    End Function

    Public Function rend() As iterator
        Return _end
    End Function

#If DEBUG Then
    Shared Sub New()
        binary_operator(Of HEAP_KEY).log_addable()
    End Sub
#End If

    Public Sub New()
        Me.m = New map(Of MAP_KEY, Int64)
        Me.h = New heap2(Me.m)
    End Sub

    <copy_constructor>
    Private Sub New(ByVal m As map(Of MAP_KEY, Int64), ByVal h As heap2)
        assert(Not m Is Nothing)
        assert(Not h Is Nothing)
        Me.m = m
        Me.h = h
    End Sub

    Public Function [erase](ByVal key As MAP_KEY) As Boolean
        Dim heapPos As map(Of MAP_KEY, Int64).iterator = Nothing
        heapPos = m.find(key)
        If heapPos = m.end() Then
            Return False
        End If
        h.erase((+(heapPos)).second)
        Return True
    End Function

    Private Function update(ByVal key As MAP_KEY,
                            ByVal value As HEAP_KEY,
                            ByVal accumulate As Boolean) As Boolean
        Dim heap_pos As map(Of MAP_KEY, Int64).iterator = Nothing
        heap_pos = m.find(key)
        If heap_pos = m.end() Then
            Return h.insert(pair.emplace_of(value, key)) <> npos
        End If
        Dim heap_node As pair(Of HEAP_KEY, MAP_KEY) = Nothing
        heap_node = h((+heap_pos).second)
        assert(Not heap_node Is Nothing, "heapNode is nothing, _map is not concur with _heap.")
        If accumulate Then
            heap_node.first = binary_operator.add(heap_node.first, value)
        Else
            heap_node.first = value
        End If
        Return h.update((+heap_pos).second, heap_node) <> npos
    End Function

    Public Function emplace(ByVal key As MAP_KEY, ByVal value As HEAP_KEY) As Boolean
        Return update(key, value, False)
    End Function

    Public Function insert(ByVal key As MAP_KEY, ByVal value As HEAP_KEY) As Boolean
        Return emplace(copy_no_error(key), copy_no_error(value))
    End Function

    Public Function accumulate(ByVal key As MAP_KEY, ByVal value As HEAP_KEY) As Boolean
        Return update(key, value, True)
    End Function

    Public Sub pop_front(ByRef key As MAP_KEY, ByRef value As HEAP_KEY)
        Dim heapNode As pair(Of HEAP_KEY, MAP_KEY) = Nothing
        h.pop_front(heapNode)
        If Not heapNode Is Nothing Then
            key = heapNode.second
            value = heapNode.first
        Else
            key = Nothing
            value = Nothing
        End If
    End Sub

    Private Function find(ByVal it As map(Of MAP_KEY, Int64).iterator) As iterator
        If it.is_null() OrElse it = m.end() Then
            Return [end]()
        End If
        Dim heapnode As pair(Of HEAP_KEY, MAP_KEY) = Nothing
        assert(h.take((+it).second, heapnode), "cannot get _heap(", (+it).second, ")")
        Return New iterator(heapnode)
    End Function

    Public Function find(ByVal key As MAP_KEY) As iterator
        Return find(m.find(key))
    End Function

    Public Sub clear()
        m.clear()
        h.clear()
    End Sub

    Public Function size() As Int64
        assert(h.size() = m.size(), "_heap.size() <> _map.size()")
        Return h.size()
    End Function

    Public Function empty() As Boolean
        assert(h.empty() = m.empty(), "_heap.empty() <> _map.empty()")
        Return h.empty()
    End Function

    Public Overrides Function ToString() As String
        Return strcat(h, newline.incode(), m)
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As mapheap(Of MAP_KEY, HEAP_KEY) _
        Implements ICloneable(Of mapheap(Of MAP_KEY, HEAP_KEY)).Clone
        Dim m As map(Of MAP_KEY, Int64) = Nothing
        m = copy_no_error(Me.m)
        Return New mapheap(Of MAP_KEY, HEAP_KEY)(m, New heap2(h, m))
    End Function
End Class
