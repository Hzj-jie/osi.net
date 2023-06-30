
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

' TODO: Implement typed_node.builder to ensure the immutability of typed_node.
Partial Public NotInheritable Class typed_node
    Public Const root_type As UInt32 = uint32_0
    Public Const root_type_name As String = "ROOT"
    ' TODO: type should be removed in favor of type_name, now it's used only in tests.
    Public ReadOnly type As UInt32
    ' TODO: Limit the use of type_name.
    Public ReadOnly type_name As String
    Public ReadOnly word_start As UInt32
    Public ReadOnly word_end As UInt32 'exclusive
    Public ReadOnly subnodes As New vector(Of typed_node)(0)
    Private ReadOnly ref As vector(Of typed_word)
    Private parent As typed_node

    Public Sub New(ByVal ref As vector(Of typed_word),
                   ByVal type As UInt32,
                   ByVal type_name As String,
                   ByVal word_start As UInt32,
                   ByVal word_end As UInt32)
        assert(Not ref Is Nothing)
        assert(word_start <= word_end)  ' start == end means empty-matching
        assert(word_end <= ref.size())
        assert(Not type_name.null_or_whitespace())
        Me.ref = ref
        Me.type = type
        Me.type_name = type_name
        Me.word_start = word_start
        Me.word_end = word_end

#If NOT_IMPLEMENTED Then
        If leaf() Then
            assert(word_count() = 1)
        End If
#End If
    End Sub

    Public Shared Function of_root(ByVal ref As vector(Of typed_word)) As typed_node
        assert(Not ref Is Nothing)
        Return New typed_node(ref, root_type, root_type_name, uint32_0, ref.size())
    End Function

    Public Sub attach_to(ByVal parent As typed_node)
        assert(Not parent Is Nothing)
        assert(Me.parent Is Nothing)
        Me.parent = parent
        parent.subnodes.emplace_back(Me)
    End Sub

    Public Sub attach(ByVal nodes As vector(Of typed_node))
        assert(Not nodes Is Nothing)
        If nodes.empty() Then
            Return
        End If
        For i As UInt32 = 0 To nodes.size() - uint32_1
            assert(Not nodes(i) Is Nothing)
            nodes(i).attach_to(Me)
        Next
    End Sub

    Public Function root() As Boolean
        Return parent Is Nothing
    End Function

    Public Function child_index(ByVal c As typed_node) As UInt32
        assert(Not c Is Nothing)
        assert(object_compare(Me, c.parent) = 0)
        assert(Not subnodes.empty())
        For i As UInt32 = 0 To subnodes.size() - uint32_1
            If object_compare(subnodes(i), c) = 0 Then
                Return i
            End If
        Next
        assert(False)
        Return max_uint32
    End Function

    Public Function child(ByVal id As UInt32) As typed_node
        assert(subnodes.available_index(id), type_name)
        Return subnodes(id)
    End Function

    Public Function child() As typed_node
        assert(child_count() = 1, type_name)
        Return child(0)
    End Function

    Public Shared Function child(ByVal n As typed_node) As typed_node
        assert(Not n Is Nothing)
        Return n.child()
    End Function

    Public Function children() As stream(Of typed_node)
        Return subnodes.stream()
    End Function

    Public Function last_child() As typed_node
        assert(child_count() > 0, type_name)
        Return child(child_count() - uint32_1)
    End Function

    Public Function child_count() As UInt32
        Return subnodes.size()
    End Function

    Public Function children_of(ByVal type_name As String) As vector(Of typed_node)
        assert(Not leaf())
        assert(Not type_name.null_or_whitespace())
        Dim r As New vector(Of typed_node)()
        For i As UInt32 = 0 To child_count() - uint32_1
            If child(i).type_name.Equals(type_name) Then
                r.emplace_back(child(i))
            End If
        Next
        Return r
    End Function

    Public Function leaf() As Boolean
        Return subnodes.empty()
    End Function

    Public Function word(ByVal id As UInt32) As typed_word
        assert(id < word_count())
        Return ref(word_start + id)
    End Function

    Public Function word() As typed_word
        assert(word_count() = 1)
        Return word(0)
    End Function

    Public Function word_count() As UInt32
        Return word_end - word_start
    End Function

    Public Function char_start() As UInt32
        Return word(0).start
    End Function

    Public Function char_end() As UInt32
        Return word(word_count() - uint32_1).end
    End Function

    Public Function only_descendant(ByRef o As typed_node) As Boolean
        Dim n As typed_node = Me
        While True
            If n.leaf() Then
                o = n
                Return True
            End If
            If n.child_count() > 1 Then
                Return False
            End If
            n = n.child()
        End While
        Return assert(False)
    End Function

    Public Function only_descendant() As typed_node
        Dim r As typed_node = Nothing
        assert(only_descendant(r))
        Return r
    End Function

    Public Function immediate_descentdant_of(ByVal ParamArray names() As String) As Boolean
        assert(Not names.null_or_empty())
        Dim n As typed_node = Me
        For i As Int32 = 0 To names.Length() - 1
            n = n.parent
            If n Is Nothing Then
                Return False
            End If
            If Not n.type_name.Equals(names(i)) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function descentdant_of(ByVal ParamArray names() As String) As Boolean
        assert(Not names.null_or_empty())
        Dim n As typed_node = Me
        Dim i As Int32 = 0
        While True
            If n.root() Then
                Return False
            End If
            n = n.parent
            assert(Not n Is Nothing)
            If n.type_name.Equals(names(i)) Then
                i += 1
                If i = names.Length() Then
                    Return True
                End If
            End If
        End While
        Return assert(False)
    End Function

    Public Function ancestor_of(ByVal name As String, ByRef o As typed_node) As Boolean
        assert(Not name.null_or_empty())
        Dim n As typed_node = Me
        While True
            assert(Not n Is Nothing)
            If n.type_name.Equals(name) Then
                o = n
                Return True
            End If
            If n.root() Then
                Return False
            End If
            n = n.parent
        End While
        Return assert(False)
    End Function

    Public Function ancestor_of(ByVal name As String) As typed_node
        Dim o As typed_node = Nothing
        assert(ancestor_of(name, o))
        Return o
    End Function
End Class
