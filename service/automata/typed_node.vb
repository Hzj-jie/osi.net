
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

' TODO: Implement typed_node.builder to ensure the immutability of typed_node.
Partial Public NotInheritable Class typed_node
    Public Const ROOT_TYPE As UInt32 = uint32_0
    Public Const ROOT_TYPE_NAME As String = "ROOT"
    Public ReadOnly type As UInt32
    Public ReadOnly type_name As String
    Public ReadOnly start As UInt32
    Public ReadOnly [end] As UInt32 'exclusive
    Public ReadOnly subnodes As New vector(Of typed_node)()
    Private ReadOnly ref As vector(Of typed_word)
    Private parent As typed_node

    Public Sub New(ByVal ref As vector(Of typed_word),
                   ByVal type As UInt32,
                   ByVal type_name As String,
                   ByVal start As UInt32,
                   ByVal [end] As UInt32)
        assert(Not ref Is Nothing)
        assert(start <= [end])  ' start == end means empty-matching
        assert([end] <= ref.size())
        assert(Not type_name.null_or_whitespace())
        Me.ref = ref
        Me.type = type
        Me.type_name = type_name
        Me.start = start
        Me.end = [end]

#If NOT_IMPLEMENTED Then
        If leaf() Then
            assert(word_count() = 1)
        End If
#End If
    End Sub

    Public Shared Function of_root(ByVal ref As vector(Of typed_word)) As typed_node
        assert(Not ref Is Nothing)
        Return New typed_node(ref, ROOT_TYPE, ROOT_TYPE_NAME, uint32_0, ref.size())
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
        assert(subnodes.available_index(id))
        Return subnodes(id)
    End Function

    Public Function child() As typed_node
        assert(child_count() = 1)
        Return child(0)
    End Function

    Public Function last_child() As typed_node
        assert(child_count() > 0)
        Return child(child_count() - uint32_1)
    End Function

    Public Function child_count() As UInt32
        Return subnodes.size()
    End Function

    Public Function leaf() As Boolean
        Return subnodes.empty()
    End Function

    Public Function word(ByVal id As UInt32) As typed_word
        assert(id < word_count())
        Return ref(start + id)
    End Function

    Public Function word() As typed_word
        assert(word_count() = 1)
        Return word(0)
    End Function

    Public Function word_count() As UInt32
        Return [end] - start
    End Function

    Public Function word_start() As UInt32
        Return word(0).start
    End Function

    Public Function word_end() As UInt32
        Return word(word_count() - uint32_1).end
    End Function
End Class
