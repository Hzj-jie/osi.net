
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template
Imports osi.root.connector
Imports osi.root.constants

Partial Public Class trie(Of keyT, valueT, _child_count As _int64, _key_to_index As _to_uint32(Of keyT))
    Implements ICloneable

    Private Shared ReadOnly child_count As UInt32 = Nothing
    Private Shared ReadOnly key_to_index As _key_to_index = Nothing
    Private ReadOnly root As node = Nothing

    Public Class node
        Implements ICloneable

        Public has_value As Boolean
        Public value As valueT

        Protected Friend _length As UInt32
        Protected Friend father As node
        Protected Friend child() As node

        Public Function length() As UInt32
            Return _length
        End Function

        Private Sub initial(ByVal index_count As UInt32)
            has_value = False
            ReDim child(CInt(index_count - uint32_1))
            value = alloc(Of valueT)()
            father = Nothing
        End Sub

        Public Sub New(ByVal index_count As UInt32)
            initial(index_count)
        End Sub

        Public Sub New()
        End Sub

        Public Sub clear()
            memclr(child)
        End Sub

        Public Function first() As keyT()
            Dim rtn(CInt(length() - uint32_1)) As keyT
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
            Else
                Return npos
            End If
        End Function

        Public Shared Operator +(ByVal this As node) As valueT
            If this.has_value Then
                Return this.value
            Else
                Return Nothing
            End If
        End Operator

        Public Function Clone() As Object Implements System.ICloneable.Clone
            Dim rtn As node = Nothing
            rtn = alloc(Me)
            rtn.initial(CUInt(child.Length()))
            copy(rtn.value, value)
            copy(rtn.has_value, has_value)
            copy(rtn._length, length)

            Return rtn
        End Function
    End Class

    Protected Shared _end As iterator = Nothing

    Public Function begin() As iterator
        Return New iterator(root)
    End Function

    Public Function rbegin() As iterator
        Dim it As node = root
        If max(it) Then
            Return New iterator(it)
        Else
            Return _end
        End If
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
        Else
            Dim i As UInt32
            For i = 0 To CUInt(it.father.child.Length()) - uint32_1
                If Object.ReferenceEquals(it.father.child(CInt(i)), it) Then
                    index = i
                    Exit For
                End If
            Next

            assert(i < it.father.child.Length(),
                     "cannot get a child of father reference equals to the node.")
            Return True
        End If
    End Function

    Private Shared Function max(ByRef it As node) As Boolean
        If it Is Nothing Then
            Return False
        Else
            While Not it.child(CInt(child_count - uint32_1)) Is Nothing
                it = it.child(CInt(child_count - uint32_1))
            End While

            Return True
        End If
    End Function

    Private Shared Function min(ByRef it As node) As Boolean
        If it Is Nothing Then
            Return False
        Else
            While Not it.child(0) Is Nothing
                it = it.child(0)
            End While

            Return True
        End If
    End Function

    Shared Sub New()
        _end = iterator.end
        Dim __child_count As Int64 = 0
        __child_count = +(alloc(Of _child_count)())
        assert(__child_count > 0 AndAlso __child_count <= max_uint32)
        child_count = CUInt(__child_count)
        key_to_index = alloc(Of _key_to_index)()
        assert(Not key_to_index Is Nothing)
    End Sub

    Public Sub New()
        root = New node(child_count)
    End Sub

    Default Public Property data(ByVal k() As keyT) As valueT
        Get
            Return (++find(k, True, False))
        End Get
        Set(ByVal value As valueT)
            Dim w As iterator = Nothing
            w = find(k, True, False)
            w.get().has_value = True
            copy(w.get().value, value)
        End Set
    End Property

    Public Function findfront(ByVal k() As keyT,
                              ByVal start As UInt32,
                              ByVal find_front_action As Action(Of node)) As iterator
        Return find(k, False, True, start, find_front_action)
    End Function

    Public Function findfront(ByVal k() As keyT, ByVal start As UInt32) As iterator
        Return findfront(k, start, Nothing)
    End Function

    Public Function findfront(ByVal k() As keyT, ByVal find_front_action As Action(Of node)) As iterator
        Return findfront(k, 0, find_front_action)
    End Function

    Public Function findfront(ByVal k() As keyT) As iterator
        Return findfront(k, 0, Nothing)
    End Function

    Public Function find(ByVal k() As keyT, ByVal start As UInt32) As iterator
        Return find(k, False, False, start)
    End Function

    Public Function find(ByVal k() As keyT) As iterator
        Return find(k, 0)
    End Function

    Private Function find(ByVal k() As keyT,
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
            Else
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
            End If
        Next

        If find_front Then
            assert(l Is Nothing OrElse l.has_value)
            If l Is Nothing Then
                Return [end]()
            Else
                Return New iterator(l)
            End If
        Else
            If w Is Nothing Then
                Return [end]()
            Else
                Return New iterator(w)
            End If
        End If
    End Function

    Public Function insert(ByVal k() As keyT) As iterator
        Dim w As iterator
        w = find(k, True, False)
        w.get().has_value = False
        Return w
    End Function

    Public Function insert(ByVal k() As keyT, ByVal v As valueT) As iterator
        Dim w As iterator = Nothing
        w = insert(k)
        With (+w)
            .has_value = True
            copy(.value, v)
        End With
        Return w
    End Function

    Public Function [erase](ByVal k() As keyT) As Boolean
        Dim w As iterator
        w = find(k, False, False)
        If w = [end]() OrElse Not w.get().has_value Then
            Return False
        Else
            w.get().has_value = False
            w.get().value = Nothing
            Return True
        End If
    End Function

    Public Function remove(ByVal k() As keyT) As Boolean
        Return [erase](k)
    End Function

    Private Shared Sub copy_node(ByRef dest As node, ByVal source As node)
        If Not source Is Nothing Then
            copy(dest, source)
            Dim i As Int32
            For i = 0 To CInt(child_count) - 1
                If Not source.child(i) Is Nothing Then
                    copy_node(dest.child(i), source.child(i))
                    dest.child(i).father = dest
                End If
            Next
        End If
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim rtn As trie(Of keyT, valueT, _child_count, _key_to_index) = Nothing
        alloc(Me)
        copy_node(rtn.root, root)
        Return rtn
    End Function

    Public Sub clear()
        root.clear()
    End Sub
End Class

Public Class chartrie(Of valueT)
    Inherits trie(Of Char, valueT, _max_uint16, _char_to_uint32)

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

    Public Overloads Function insert(ByVal s As String, ByVal v As valueT) As iterator
        Return MyBase.insert(c_str(s), v)
    End Function

    Public Overloads Function insert(ByVal s As String) As iterator
        Return MyBase.insert(c_str(s))
    End Function

    Public Overloads Function [erase](ByVal s As String) As Boolean
        Return MyBase.erase(c_str(s))
    End Function

    Public Overloads Function remove(ByVal s As String) As Boolean
        Return MyBase.remove(c_str(s))
    End Function

    Default Public Overloads Property data(ByVal s As String) As valueT
        Get
            Return MyBase.data(c_str(s))
        End Get
        Set(ByVal value As valueT)
            MyBase.data(c_str(s)) = value
        End Set
    End Property

    Public Shared Function first(ByVal it As iterator) As String
        If it Is Nothing OrElse it.is_end() Then
            Return Nothing
        Else
            Return Convert.ToString(it.get().first())
        End If
    End Function
End Class

Public Class bytetrie(Of valueT)
    Inherits trie(Of Byte, valueT, _max_uint8, _byte_to_uint32)
End Class

Public Class stringtrie(Of valueT)
    Inherits bytetrie(Of valueT)

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

    Public Shadows Function insert(ByVal s As String, ByVal v As valueT) As iterator
        Return MyBase.insert(str_bytes(s), v)
    End Function

    Public Shadows Function insert(ByVal s As String) As iterator
        Return MyBase.insert(str_bytes(s))
    End Function

    Public Shadows Function [erase](ByVal s As String) As Boolean
        Return MyBase.erase(str_bytes(s))
    End Function

    Public Shadows Function remove(ByVal s As String) As Boolean
        Return MyBase.remove(str_bytes(s))
    End Function

    Default Public Shadows Property data(ByVal s As String) As valueT
        Get
            Return MyBase.data(str_bytes(s))
        End Get
        Set(ByVal value As valueT)
            MyBase.data(str_bytes(s)) = value
        End Set
    End Property

    Public Shared Function first(ByVal it As iterator) As String
        If it Is Nothing OrElse it.is_end() Then
            Return Nothing
        Else
            Return bytes_str(it.get().first())
        End If
    End Function
End Class
