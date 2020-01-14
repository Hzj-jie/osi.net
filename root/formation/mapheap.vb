
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public Class mapheap(Of MAP_KEY As IComparable(Of MAP_KEY), HEAP_KEY As IComparable(Of HEAP_KEY))
    Implements ICloneable

    Private Class heap2
        Inherits heap(Of pair(Of HEAP_KEY, MAP_KEY))

        Public _map As map(Of MAP_KEY, Int64) = Nothing

        Public Overrides Function [erase](ByVal index As Int64) As Boolean
            debug_assert(Not _map Is Nothing, "_map is nothing.")
            debug_assert(Not at(index) Is Nothing, "index " + Convert.ToString(index) + " is not in heap.")
            Dim k As MAP_KEY = Nothing
            k = at(index).second
            debug_assert(MyBase.erase(index), "mybase.erase(" + Convert.ToString(index) + ") returns false.")
            If index = size() Then
                'mybase will not call update here, if the removed index is the last one
                debug_assert(_map.erase(k), "_map.erase(" + Convert.ToString(k) + ") returns false.")
            End If

            debug_assert(size() = _map.size(), "heap.size() <> _map.size()")

            Return True
        End Function

        Public Overrides Function update(ByVal index As Int64, ByVal value As pair(Of HEAP_KEY, MAP_KEY)) As Int64
            debug_assert(index <> npos, "should not call update with index = npos.")
            debug_assert(Not value Is Nothing, "value is nothing.")
            If Not at(index) Is Nothing Then
                debug_assert(_map.erase(at(index).second) _
                            , "_map.erase(" + Convert.ToString(at(index).second) + ") returns false.")
            End If
            Dim rtn As Int64
            rtn = MyBase.update(index, value)
            debug_assert(rtn <> npos, "cannot update index " + Convert.ToString(index))
            _map(value.second) = rtn

            debug_assert(size() = _map.size(), "heap.size() <> _map.size()")

            Return rtn
        End Function


        Protected Overrides Function swap(ByVal first As Int64, ByVal second As Int64) As Boolean
            debug_assert(Not _map Is Nothing, "_map is nothing before swap")
            If MyBase.swap(first, second) Then
                _map(at(first).second) = first
                If first <> second Then
                    _map(at(second).second) = second
                End If
                Return True
            Else
                Return False
            End If
        End Function
    End Class

    Private Shared ReadOnly _end As iterator = Nothing
    Private _heap As heap2 = Nothing
    Private _map As map(Of MAP_KEY, Int64) = Nothing

    Public Function begin() As iterator
        Return find(_map.begin())
    End Function

    Public Function rbegin() As iterator
        Return find(_map.rbegin())
    End Function

    Public Function [end]() As iterator
        Return _end
    End Function

    Public Function rend() As iterator
        Return _end
    End Function

    Shared Sub New()
        _end = iterator.end

        If isdebugmode() Then
            binary_operator(Of HEAP_KEY).log_addable()
        End If
    End Sub

    Public Sub New()
        _map = New map(Of MAP_KEY, Int64)
        _heap = New heap2()
        _heap._map = _map
    End Sub

    Public Function [erase](ByVal key As MAP_KEY) As Boolean
        Dim heapPos As map(Of MAP_KEY, Int64).iterator = Nothing
        heapPos = _map.find(key)
        If heapPos <> _map.end() Then
            _heap.erase((+(heapPos)).second)
            Return True
        Else
            Return False
        End If
    End Function

    Protected Function update(ByVal key As MAP_KEY,
                              ByVal value As HEAP_KEY,
                              Optional ByVal accumulate As Boolean = True) As Boolean
        Dim heap_pos As map(Of MAP_KEY, Int64).iterator = Nothing
        heap_pos = _map.find(key)
        If heap_pos = _map.end() Then
            Return _heap.insert(pair.emplace_of(value, key)) <> npos
        Else
            Dim heap_node As pair(Of HEAP_KEY, MAP_KEY) = Nothing
            heap_node = _heap((+heap_pos).second)
            debug_assert(Not heap_node Is Nothing, "heapNode is nothing, _map is not concur with _heap.")
            If accumulate Then
                heap_node.first = binary_operator.add(heap_node.first, value)
            Else
                heap_node.first = value
            End If
            Return _heap.update((+heap_pos).second, heap_node) <> npos
        End If
    End Function

    Public Function emplace(ByVal key As MAP_KEY, ByVal value As HEAP_KEY) As Boolean
        Return update(key, value, False)
    End Function

    Public Function insert(ByVal key As MAP_KEY, ByVal value As HEAP_KEY) As Boolean
        Return update(copy_no_error(key), copy_no_error(value), False)
    End Function

    Public Function accumulate(ByVal key As MAP_KEY, ByVal value As HEAP_KEY) As Boolean
        Return update(key, value, True)
    End Function

    Public Sub pop_front(ByRef key As MAP_KEY, ByRef value As HEAP_KEY)
        Dim heapNode As pair(Of HEAP_KEY, MAP_KEY) = Nothing
        _heap.pop_front(heapNode)
        If Not heapNode Is Nothing Then
            key = heapNode.second
            value = heapNode.first
        Else
            key = Nothing
            value = Nothing
        End If
    End Sub

    Private Function find(ByVal it As map(Of MAP_KEY, Int64).iterator) As iterator
        If it.is_null() OrElse it = _map.end() Then
            Return [end]()
        Else
            Dim heapnode As pair(Of HEAP_KEY, MAP_KEY) = Nothing
            debug_assert(_heap.take((+it).second, heapnode) _
                        , "cannot get _heap(" + Convert.ToString((+it).second) + ")")
            Return New iterator(heapnode)
        End If
    End Function

    Public Function find(ByVal key As MAP_KEY) As iterator
        Return find(_map.find(key))
    End Function

    Public Sub clear()
        _map.clear()
        _heap.clear()
    End Sub

    Public Function size() As Int64
        debug_assert(_heap.size() = _map.size(), "_heap.size() <> _map.size()")
        Return _heap.size()
    End Function

    Public Function empty() As Boolean
        debug_assert(_heap.empty() = _map.empty(), "_heap.empty() <> _map.empty()")
        Return _heap.empty()
    End Function

    Public Overrides Function ToString() As String
        Return Convert.ToString(_heap) + newline.incode() + Convert.ToString(_map)
    End Function

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim rtn As mapheap(Of MAP_KEY, HEAP_KEY) = Nothing
        rtn = allocate_instance_of(Me)

        copy(rtn._heap, _heap)
        copy(rtn._map, _map)
        rtn._heap._map = rtn._map

        Return rtn
    End Function
End Class
