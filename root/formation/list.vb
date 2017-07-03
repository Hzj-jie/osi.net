
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class list(Of T)
    Implements IComparable(Of list(Of T)), IComparable, ICloneable

    Friend Class node
        Inherits pointernode(Of T)

        Private Const _pointer_count As UInt32 = 2

        Private Enum pointer_index As UInt32
            last = 0
            [next] = 1
        End Enum

        Public Function last() As node
            Return direct_cast(Of node)(pointer(pointer_index.last))
        End Function

        Public Function [next]() As node
            Return direct_cast(Of node)(pointer(pointer_index.next))
        End Function

        Public Sub appendlast(ByVal that As node)
            debug_assert(Not Object.ReferenceEquals(Me, that), "should not append last to a node itself.")
            pointer(pointer_index.last) = that
            If Not that Is Nothing Then
                that.pointer(pointer_index.next) = Me
            End If
        End Sub

        Public Sub appendnext(ByVal that As node)
            debug_assert(Not Object.ReferenceEquals(Me, that), "should not append next to a node itself.")
            pointer(pointer_index.next) = that
            If Not that Is Nothing Then
                that.pointer(pointer_index.last) = Me
            End If
        End Sub

        Public Sub New()
            MyBase.New(_pointer_count)
        End Sub

        Public Sub New(ByVal new_data As T)
            MyBase.New(_pointer_count, new_data)
        End Sub
    End Class

    Private Shared ReadOnly _end As iterator = Nothing
    Private _front As node
    Private _back As node
    Private _size As UInt32

    Public Function begin() As iterator
        If empty() Then
            Return _end
        Else
            Return New iterator(_front)
        End If
    End Function

    Public Function rbegin() As iterator
        If empty() Then
            Return _end
        Else
            Return New iterator(_back)
        End If
    End Function

    Public Function [end]() As iterator
        Return _end
    End Function

    Public Function rend() As iterator
        Return _end
    End Function

    Public Property front() As T
        Get
            Return +_front
        End Get
        Set(ByVal value As T)
            assert(Not empty())
            _front.data() = value
        End Set
    End Property

    Public Property back() As T
        Get
            Return +_back
        End Get
        Set(ByVal value As T)
            assert(Not empty())
            _back.data() = value
        End Set
    End Property

    Private Function find(ByVal index As UInt32) As node
        Dim rtn As node = Nothing
        If Not available_index(index) Then
            rtn = Nothing
        Else
            If index <= (_size >> 1) Then
                rtn = _front
                While index > uint32_0
                    rtn = rtn.next
                    index -= uint32_1
                End While
            Else
                rtn = _back
                index = _size - index - uint32_1
                While index > uint32_0
                    rtn = rtn.last
                    index -= uint32_1
                End While
            End If
        End If

        Return rtn
    End Function

    Public Function [goto](ByVal index As UInt32) As iterator
        Return New iterator(find(index))
    End Function

    Public Function find(ByVal v As T) As iterator
        Dim it As iterator = Nothing
        it = begin()
        While it <> [end]()
            If compare(v, +it) = 0 Then
                Return it
            End If

            it += 1
        End While

        Return [end]()
    End Function

    Public Function rfind(ByVal v As T) As iterator
        Dim it As iterator = Nothing
        it = rbegin()
        While it <> rend()
            If compare(v, +it) = 0 Then
                Return it
            End If

            it -= 1
        End While

        Return rend()
    End Function

    Private Function available_index(ByVal index As UInt32) As Boolean
        Return index >= uint32_0 And index < size()
    End Function

    Public Function size() As UInt32
        Return _size
    End Function

    Default Public Property at(ByVal index As UInt32) As T
        Get
            Dim rtn As node = Nothing
            rtn = find(index)
            Return +rtn
        End Get
        Set(ByVal value As T)
            Dim rtn As node = Nothing
            rtn = find(index)
            If Not rtn Is Nothing Then
                rtn.data = value
            End If
        End Set
    End Property

    Public Function empty() As Boolean
        Return size() = 0
    End Function

    Public Function push_back(ByVal new_data As T) As Boolean
        Dim add As node = Nothing
        add = New node(new_data)
        If Not _back Is Nothing Then
            _back.appendnext(add)
        Else
            _front = add
        End If
        _back = add
        _size += uint32_1

        Return True
    End Function

    Public Function push_front(ByVal new_data As T) As Boolean
        Dim add As node = Nothing
        add = New node(new_data)
        If Not _front Is Nothing Then
            _front.appendlast(add)
        Else
            _back = add
        End If
        _front = add
        _size += uint32_1

        Return True
    End Function

    Public Function pop_back() As Boolean
        If empty() Then
            Return False
        Else
            [erase](_back)
            Return True
        End If
    End Function

    Public Function pop_front() As Boolean
        If empty() Then
            Return False
        Else
            [erase](_front)
            Return True
        End If
    End Function

    Public Function insert(ByVal it As iterator, ByVal newdata As T) As iterator
        Return insert(it.node(), newdata)
    End Function

    Private Function insert(ByVal it As node, ByVal newData As T) As iterator
        Dim work As node = Nothing
        If it Is Nothing Then
            Return [end]()
        Else
            work = New node(newData)
            'it.next will be changed after work.appendlast(it)
            work.appendnext(it.next)
            work.appendlast(it)
            If Object.ReferenceEquals(it, _back) Then
                _back = work
            End If
            _size += uint32_1
            it = it.next()
            debug_assert(Not it Is Nothing, "it.next() is nothing after insert.")

            Return New iterator(it)
        End If
    End Function

    Public Function insert(ByVal index As UInt32, ByVal new_data As T) As iterator
        Return insert(find(index), new_data)
    End Function

    Public Function insert(ByVal newData As T) As iterator
        push_back(newData)
        Return rbegin()
    End Function

    Private Function [erase](ByVal it As node) As iterator
        If it Is Nothing Then
            Return [end]()
        Else
            Dim del As node = it
            If Not it.last Is Nothing Then
                it.last.appendnext(it.next)
            ElseIf Not it.next Is Nothing Then
                it.next.appendlast(it.last)
            End If
            If Object.ReferenceEquals(it, _front) Then
                _front = it.next
            End If
            If Object.ReferenceEquals(it, _back) Then
                _back = it.last
            End If
            _size -= uint32_1
            del.appendlast(Nothing)
            del.appendnext(Nothing)
            If it.next Is Nothing Then
                Return [end]()
            Else
                Return New iterator(it.next)
            End If
        End If
    End Function

    Public Function [erase](ByVal it As iterator) As iterator
        Return [erase](it.node())
    End Function

    Public Function [erase](ByVal index As UInt32) As iterator
        Return [erase](find(index))
    End Function

    Public Function [erase](ByVal s As T) As iterator
        Return [erase](find(s))
    End Function

    Public Sub clear()
        _front = Nothing
        _back = Nothing
        _size = 0
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim rtn As list(Of T) = Nothing
        Dim it As list(Of T).node
        rtn = New list(Of T)()
        it = _front
        While Not it Is Nothing
            rtn.push_back(it.data)
            it = it.next
        End While

        Return rtn
    End Function

    Shared Sub New()
        _end = iterator.end
    End Sub

    Public Sub New()
        clear()
    End Sub

    Public Function contains(ByVal v As T) As Boolean
        Return find(v) <> [end]()
    End Function

    Public Function CompareTo(ByVal that As list(Of T)) As Int32 Implements IComparable(Of list(Of T)).CompareTo
        assert(Not that Is Nothing, "that should not be nothing after inherits from IComparable2.")

        Dim i As iterator = Nothing
        Dim j As iterator = Nothing

        i = begin()
        j = that.begin()
        While i <> [end]() AndAlso j <> that.end()
            assert(Not i Is Nothing AndAlso Not j Is Nothing)
            Dim cmp As Int32 = 0
            cmp = i.node().CompareTo(j.node())
            If cmp <> 0 Then
                Return cmp
            End If

            i += 1
            j += 1
        End While

        If i = [end]() AndAlso j = that.end() Then
            Return 0
        ElseIf i = [end]() Then
            Return -1
        Else
            Return 1
        End If
    End Function

    Public Function CompareTo(ByVal that As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of list(Of T))(that, False))
    End Function

    Public Shared Operator =(ByVal this As list(Of T), ByVal that As list(Of T)) As Boolean
        Return operatorEqual(this, that)
    End Operator

    Public Shared Operator <>(ByVal this As list(Of T), ByVal that As list(Of T)) As Boolean
        Return operatorUnequal(this, that)
    End Operator

    Public Shared Operator =(ByVal this As list(Of T), ByVal that As Object) As Boolean
        Return operatorEqual(this, that)
    End Operator

    Public Shared Operator <>(ByVal this As list(Of T), ByVal that As Object) As Boolean
        Return operatorUnequal(this, that)
    End Operator

    Public Overloads Function ToString(ByVal seperator As String) As String
        Dim it As iterator = Nothing
        Dim r As StringBuilder = Nothing
        r = New StringBuilder()
        it = begin()
        While it <> [end]()
            r.Append(+it)
            If it <> [end]() Then
                r.Append(seperator)
            End If
            it += 1
        End While
        Return Convert.ToString(r)
    End Function

    Public Overrides Function ToString() As String
        Return ToString(character.tab)
    End Function
End Class
