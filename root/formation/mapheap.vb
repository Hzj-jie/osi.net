
Imports osi.root.constants
Imports osi.root.connector

Partial Public Class mapheap(Of mapKey As IComparable(Of mapKey), heapKey As IComparable(Of heapKey))
    Implements ICloneable

    Private Class heap2
        Inherits heap(Of pair(Of heapKey, mapKey))

        Public _map As map(Of mapKey, Int64) = Nothing

        Public Overrides Function [erase](ByVal index As Int64) As Boolean
            debug_assert(Not _map Is Nothing, "_map is nothing.")
            debug_assert(Not at(index) Is Nothing, "index " + Convert.ToString(index) + " is not in heap.")
            Dim k As mapKey = Nothing
            k = at(index).second
            debug_assert(MyBase.erase(index), "mybase.erase(" + Convert.ToString(index) + ") returns false.")
            If index = size() Then
                'mybase will not call update here, if the removed index is the last one
                debug_assert(_map.erase(k), "_map.erase(" + Convert.ToString(k) + ") returns false.")
            End If

            debug_assert(size() = _map.size(), "heap.size() <> _map.size()")

            Return True
        End Function

        Public Overrides Function update(ByVal index As Int64, ByVal value As pair(Of heapKey, mapKey)) As Int64
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

    Public Shared ReadOnly boa As binder(Of Func(Of heapKey, heapKey, heapKey), binary_operator_add_protector)
    Private Shared ReadOnly _end As iterator = Nothing
    Private _heap As heap2 = Nothing
    Private _map As map(Of mapKey, Int64) = Nothing

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
        boa = New binder(Of Func(Of heapKey, heapKey, heapKey), binary_operator_add_protector)()
        _end = iterator.end

        If isdebugmode() AndAlso
           Not boa.has_value() AndAlso
           Not accumulatable(Of heapKey).v Then
            assert(Not accumulatable(Of heapKey).ex Is Nothing)
            raise_error(error_type.warning,
                          "cannot add a heapKey to another, heapKey = ",
                          GetType(heapKey).FullName(),
                          ", may cause data lost in accumulate function, ex ",
                          accumulatable(Of heapKey).ex.Message())
        End If
    End Sub

    Public Sub New()
        _map = New map(Of mapKey, Int64)
        _heap = New heap2()
        _heap._map = _map
    End Sub

    Public Function [erase](ByVal key As mapKey) As Boolean
        Dim heapPos As map(Of mapKey, Int64).iterator = Nothing
        heapPos = _map.find(key)
        If heapPos <> _map.end() Then
            _heap.erase((+(heapPos)).second)
            Return True
        Else
            Return False
        End If
    End Function

    Protected Function update(ByVal key As mapKey, ByVal value As heapKey _
                            , Optional ByVal accumulate As Boolean = True) As Boolean
        Dim heapPos As map(Of mapKey, Int64).iterator = Nothing
        heapPos = _map.find(key)
        If heapPos = _map.end() Then
            Return _heap.insert(make_pair(value, key)) <> npos
        Else
            Dim heapNode As pair(Of heapKey, mapKey) = Nothing
            heapNode = _heap((+heapPos).second)
            debug_assert(Not heapNode Is Nothing, "heapNode is nothing, _map is not concur with _heap.")
            If accumulate Then
                assert(boa.has_value() OrElse accumulatable(Of heapKey).v)
                If boa.has_value() Then
                    heapNode.first = (+boa)(heapNode.first, value)
                Else
                    heapNode.first = inc(heapNode.first, value)
                End If
            Else
                copy(heapNode.first, value)
            End If
            Return _heap.update((+heapPos).second, heapNode) <> npos
        End If
    End Function

    Public Function insert(ByVal key As mapKey, ByVal value As heapKey) As Boolean
        Return update(key, value, False)
    End Function

    Public Function accumulate(ByVal key As mapKey, ByVal value As heapKey) As Boolean
        Return update(key, value, True)
    End Function

    Public Sub pop_front(ByRef key As mapKey, ByRef value As heapKey)
        Dim heapNode As pair(Of heapKey, mapKey) = Nothing
        _heap.pop_front(heapNode)
        If Not heapNode Is Nothing Then
            key = heapNode.second
            value = heapNode.first
        Else
            key = Nothing
            value = Nothing
        End If
    End Sub

    Private Function find(ByVal it As map(Of mapKey, Int64).iterator) As iterator
        If it Is Nothing OrElse it = _map.end() Then
            Return [end]()
        Else
            Dim heapnode As pair(Of heapKey, mapKey) = Nothing
            debug_assert(_heap.take((+it).second, heapnode) _
                        , "cannot get _heap(" + Convert.ToString((+it).second) + ")")
            Return New iterator(heapnode)
        End If
    End Function

    Public Function find(ByVal key As mapKey) As iterator
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
        Dim rtn As mapheap(Of mapKey, heapKey) = Nothing
        rtn = alloc(Me)

        copy(rtn._heap, _heap)
        copy(rtn._map, _map)
        rtn._heap._map = rtn._map

        Return rtn
    End Function
End Class
