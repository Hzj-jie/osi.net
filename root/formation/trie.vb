
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template

Partial Public Class trie(Of KEY_T, VALUE_T, _CHILD_COUNT As _int64, _KEY_TO_INDEX As _to_uint32(Of KEY_T))
    Implements ICloneable

    Private Shared ReadOnly child_count As UInt32 = assert_which.of(+(alloc(Of _CHILD_COUNT)())).can_cast_to_uint32()
    Private Shared ReadOnly key_to_index As _KEY_TO_INDEX = assert_not_nothing_return(alloc(Of _KEY_TO_INDEX)())
    Private ReadOnly root As node

    Public NotInheritable Class node
        Implements ICloneable

        Public has_value As Boolean
        Public value As VALUE_T

        Protected Friend _length As UInt32
        Protected Friend father As node
        Protected Friend child() As node

        Public Function length() As UInt32
            Return _length
        End Function

        Private Sub initial(ByVal index_count As UInt32)
            has_value = False
            ReDim child(CInt(index_count - uint32_1))
            value = alloc(Of VALUE_T)()
            father = Nothing
        End Sub

        Public Sub New(ByVal index_count As UInt32)
            initial(index_count)
        End Sub

        Public Sub New()
        End Sub

        Public Sub clear()
            arrays.clear(child)
        End Sub

        Public Function first() As KEY_T()
            Dim rtn(CInt(length() - uint32_1)) As KEY_T
            Dim w As node = Nothing
            w = Me
            For i As Int64 = length() - 1 To 0 Step -1
                rtn(CInt(i)) = key_to_index.reverse(CUInt(w.father_index()))
                w = w.father
            Next
            assert(w.is_root())
            Return rtn
        End Function

        Public Function is_root() As Boolean
            Return father Is Nothing
        End Function

        Public Function father_index() As Int64
            Dim rtn As UInt32
            If find_father_index(Me, rtn) Then
                Return rtn
            End If
            Return npos
        End Function

        Public Shared Operator +(ByVal this As node) As VALUE_T
            If this.has_value Then
                Return this.value
            End If
            Return Nothing
        End Operator

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim rtn As node = Nothing
            rtn = allocate_instance_of(Me)
            rtn.initial(CUInt(child.Length()))
            copy(rtn.value, value)
            copy(rtn.has_value, has_value)
            copy(rtn._length, length)

            Return rtn
        End Function
    End Class

    Protected Shared ReadOnly _end As iterator = iterator.end

    Public Function begin() As iterator
        Return New iterator(root)
    End Function

    Public Function rbegin() As iterator
        Dim it As node = root
        If max(it) Then
            Return New iterator(it)
        End If
        Return _end
    End Function

    Public Function [end]() As iterator
        Return _end
    End Function

    Public Function rend() As iterator
        Return _end
    End Function

    Private Shared Function find_father_index(ByVal it As node, ByRef index As UInt32) As Boolean
        If it.father Is Nothing Then
            Return False
        End If
        Dim i As UInt32
        For i = 0 To CUInt(it.father.child.Length()) - uint32_1
            If Object.ReferenceEquals(it.father.child(CInt(i)), it) Then
                index = i
                Exit For
            End If
        Next

        assert(i < it.father.child.Length(), "cannot get a child of father reference equals to the node.")
        Return True
    End Function

    Private Shared Function max(ByRef it As node) As Boolean
        If it Is Nothing Then
            Return False
        End If
        While Not it.child(CInt(child_count - uint32_1)) Is Nothing
            it = it.child(CInt(child_count - uint32_1))
        End While

        Return True
    End Function

    Private Shared Function min(ByRef it As node) As Boolean
        If it Is Nothing Then
            Return False
        End If
        While Not it.child(0) Is Nothing
            it = it.child(0)
        End While

        Return True
    End Function

    Public Sub New()
        root = New node(child_count)
    End Sub

    Default Public Property data(ByVal k() As KEY_T) As VALUE_T
        Get
            Return (++find(k, True, False))
        End Get
        Set(ByVal value As VALUE_T)
            Dim w As iterator = Nothing
            w = find(k, True, False)
            w.get().has_value = True
            copy(w.get().value, value)
        End Set
    End Property

    Public Function findfront(ByVal k() As KEY_T,
                              ByVal start As UInt32,
                              ByVal find_front_action As Action(Of node)) As iterator
        Return find(k, False, True, start, find_front_action)
    End Function

    Public Function findfront(ByVal k() As KEY_T, ByVal start As UInt32) As iterator
        Return findfront(k, start, Nothing)
    End Function

    Public Function findfront(ByVal k() As KEY_T, ByVal find_front_action As Action(Of node)) As iterator
        Return findfront(k, 0, find_front_action)
    End Function

    Public Function findfront(ByVal k() As KEY_T) As iterator
        Return findfront(k, 0, Nothing)
    End Function

    Public Function find(ByVal k() As KEY_T, ByVal start As UInt32) As iterator
        Return find(k, False, False, start)
    End Function

    Public Function find(ByVal k() As KEY_T) As iterator
        Return find(k, 0)
    End Function

    Private Function find(ByVal k() As KEY_T,
                          ByVal auto_insert As Boolean,
                          ByVal find_front As Boolean,
                          Optional ByVal start As UInt32 = 0,
                          Optional ByVal find_front_action As Action(Of node) = Nothing) As iterator
        Dim i As Int32 = 0
        Dim w As node = Nothing
        Dim l As node = Nothing
        Dim index As Int32 = 0

        Dim update_working_node As Action = Sub()
                                                If Not w Is Nothing AndAlso w.has_value Then
                                                    l = w
                                                    If Not find_front_action Is Nothing Then
                                                        find_front_action(l)
                                                    End If
                                                End If
                                            End Sub
        w = root
        update_working_node()
        For i = CInt(start) To array_size_i(k) - 1
            If w Is Nothing Then
                Exit For
            End If
            index = CInt(key_to_index(k(i)))
            assert(index >= 0 AndAlso index < child_count)
            If w.child(index) Is Nothing Then
                If auto_insert Then
                    w.child(index) = New node(child_count)
                    w.child(index)._length = CUInt(i + 1)
                    w.child(index).father = w
                ElseIf find_front Then
                    Exit For
                End If
            End If
            w = w.child(index)
            update_working_node()
        Next

        If find_front Then
            assert(l Is Nothing OrElse l.has_value)
            If l Is Nothing Then
                Return [end]()
            End If
            Return New iterator(l)
        End If
        If w Is Nothing Then
            Return [end]()
        End If
        Return New iterator(w)
    End Function

    Public Function insert(ByVal k() As KEY_T) As pair(Of iterator, Boolean)
        Dim w As iterator = Nothing
        w = find(k, True, False)
        Return pair.emplace_of(w, Not w.get().has_value)
    End Function

    Public Function insert(ByVal k() As KEY_T, ByVal v As VALUE_T) As pair(Of iterator, Boolean)
        Return emplace(k, copy_no_error(v))
    End Function

    Public Function emplace(ByVal k() As KEY_T, ByVal v As VALUE_T) As pair(Of iterator, Boolean)
        Dim r As pair(Of iterator, Boolean) = Nothing
        r = insert(k)
        If r.second Then
            With +(r.first)
                .has_value = True
                .value = v
            End With
        End If
        Return r
    End Function

    Public Function [erase](ByVal k() As KEY_T) As Boolean
        Dim w As iterator
        w = find(k, False, False)
        If w = [end]() OrElse Not w.get().has_value Then
            Return False
        End If
        w.get().has_value = False
        w.get().value = Nothing
        Return True
    End Function

    Public Function remove(ByVal k() As KEY_T) As Boolean
        Return [erase](k)
    End Function

    Private Shared Sub copy_node(ByRef dest As node, ByVal source As node)
        If source Is Nothing Then
            Return
        End If
        copy(dest, source)
        Dim i As Int32
        For i = 0 To CInt(child_count) - 1
            If Not source.child(i) Is Nothing Then
                copy_node(dest.child(i), source.child(i))
                dest.child(i).father = dest
            End If
        Next
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim rtn As trie(Of KEY_T, VALUE_T, _CHILD_COUNT, _KEY_TO_INDEX) = Nothing
        rtn = allocate_instance_of(Me)
        copy_node(rtn.root, root)
        Return rtn
    End Function

    Public Sub clear()
        root.clear()
    End Sub
End Class

Public Class chartrie(Of VALUE_T)
    Inherits trie(Of Char, VALUE_T, _max_uint16, _char_to_uint32)

    Public Overloads Function find(ByVal s As String) As iterator
        Return MyBase.find(c_str(s))
    End Function

    Public Overloads Function find(ByVal s As String, ByVal start As UInt32) As iterator
        Return MyBase.find(c_str(s), start)
    End Function

    Public Overloads Function findfront(ByVal s As String) As iterator
        Return MyBase.findfront(c_str(s))
    End Function

    Public Overloads Function findfront(ByVal s As String,
                                        ByVal find_front_action As Action(Of node)) As iterator
        Return MyBase.findfront(c_str(s), find_front_action)
    End Function

    Public Overloads Function findfront(ByVal s As String,
                                        ByVal start As UInt32) As iterator
        Return MyBase.findfront(c_str(s), start)
    End Function

    Public Overloads Function findfront(ByVal s As String,
                                        ByVal start As UInt32,
                                        ByVal find_front_action As Action(Of node)) As iterator
        Return MyBase.findfront(c_str(s), start, find_front_action)
    End Function

    Public Overloads Function emplace(ByVal s As String, ByVal v As VALUE_T) As pair(Of iterator, Boolean)
        Return MyBase.emplace(c_str(s), v)
    End Function

    Public Overloads Function insert(ByVal s As String, ByVal v As VALUE_T) As pair(Of iterator, Boolean)
        Return MyBase.insert(c_str(s), v)
    End Function

    Public Overloads Function insert(ByVal s As String) As pair(Of iterator, Boolean)
        Return MyBase.insert(c_str(s))
    End Function

    Public Overloads Function [erase](ByVal s As String) As Boolean
        Return MyBase.erase(c_str(s))
    End Function

    Public Overloads Function remove(ByVal s As String) As Boolean
        Return MyBase.remove(c_str(s))
    End Function

    Default Public Overloads Property data(ByVal s As String) As VALUE_T
        Get
            Return MyBase.data(c_str(s))
        End Get
        Set(ByVal value As VALUE_T)
            MyBase.data(c_str(s)) = value
        End Set
    End Property

    Public Shared Function first(ByVal it As iterator) As String
        If it.is_end() Then
            Return Nothing
        End If
        Return Convert.ToString(it.get().first())
    End Function
End Class

Public Class bytetrie(Of VALUE_T)
    Inherits trie(Of Byte, VALUE_T, _max_uint8, _byte_to_uint32)
End Class

Public Class stringtrie(Of VALUE_T)
    Inherits bytetrie(Of VALUE_T)

    Private Shared Function string_start(ByVal s As String, ByVal start As UInt32) As UInt32
        Return str_byte_count(s, 0, start)
    End Function

    Public Shadows Function find(ByVal s As String) As iterator
        Return MyBase.find(str_bytes(s))
    End Function

    Public Shadows Function find(ByVal s As String, ByVal start As UInt32) As iterator
        Return MyBase.find(str_bytes(s), string_start(s, start))
    End Function

    Public Shadows Function findfront(ByVal s As String) As iterator
        Return MyBase.findfront(str_bytes(s))
    End Function

    Public Shadows Function findfront(ByVal s As String,
                                      ByVal find_front_action As Action(Of node)) As iterator
        Return MyBase.findfront(str_bytes(s), find_front_action)
    End Function

    Public Shadows Function findfront(ByVal s As String,
                                      ByVal start As UInt32) As iterator
        Return MyBase.findfront(str_bytes(s), string_start(s, start))
    End Function

    Public Shadows Function findfront(ByVal s As String,
                                      ByVal start As UInt32,
                                      ByVal find_front_action As Action(Of node)) As iterator
        Return MyBase.findfront(str_bytes(s), string_start(s, start), find_front_action)
    End Function

    Public Shadows Function emplace(ByVal s As String, ByVal v As VALUE_T) As pair(Of iterator, Boolean)
        Return MyBase.emplace(str_bytes(s), v)
    End Function

    Public Shadows Function insert(ByVal s As String, ByVal v As VALUE_T) As pair(Of iterator, Boolean)
        Return MyBase.insert(str_bytes(s), v)
    End Function

    Public Shadows Function insert(ByVal s As String) As pair(Of iterator, Boolean)
        Return MyBase.insert(str_bytes(s))
    End Function

    Public Shadows Function [erase](ByVal s As String) As Boolean
        Return MyBase.erase(str_bytes(s))
    End Function

    Public Shadows Function remove(ByVal s As String) As Boolean
        Return MyBase.remove(str_bytes(s))
    End Function

    Default Public Shadows Property data(ByVal s As String) As VALUE_T
        Get
            Return MyBase.data(str_bytes(s))
        End Get
        Set(ByVal value As VALUE_T)
            MyBase.data(str_bytes(s)) = value
        End Set
    End Property

    Public Shared Function first(ByVal it As iterator) As String
        If it.is_end() Then
            Return Nothing
        End If
        Return bytes_str(it.get().first())
    End Function
End Class
