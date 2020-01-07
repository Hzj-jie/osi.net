
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

' TODO: Implement typed_node.builder to ensure the immutability of typed_node.
' TODO: Add parent node
' TODO: Add type-str
Public NotInheritable Class typed_node
    Public Const ROOT_TYPE As UInt32 = uint32_0
    Public Const ROOT_TYPE_STR As String = "ROOT"
    Public ReadOnly type As UInt32
    Public ReadOnly type_name As String
    Public ReadOnly start As UInt32
    Public ReadOnly [end] As UInt32 'exclusive
    Public ReadOnly parent As typed_node
    Public ReadOnly subnodes As vector(Of typed_node)
    Public ReadOnly ref As vector(Of typed_word)

    Public Sub New(ByVal ref As vector(Of typed_word),
                   ByVal type As UInt32,
                   ByVal type_name As String,
                   ByVal start As UInt32,
                   ByVal [end] As UInt32,
                   ByVal parent As typed_node,
                   ByVal ParamArray subnodes() As typed_node)
        assert(Not ref Is Nothing)
        assert(start <= [end])  ' start == end means empty-matching
        assert([end] <= ref.size())
        assert(Not type_name.null_or_whitespace())
        Me.ref = ref
        Me.type = type
        Me.type_name = type_name
        Me.start = start
        Me.end = [end]
        Me.parent = parent
        Me.subnodes = New vector(Of typed_node)()
        Me.subnodes.emplace_back(subnodes)

#If NOT_IMPLEMENTED Then
        If leaf() Then
            assert(word_count() = 1)
        End If
#End If
    End Sub

    Public Shared Function of_root(ByVal ref As vector(Of typed_word),
                                   ByVal ParamArray subnodes() As typed_node) As typed_node
        assert(Not ref Is Nothing)
        Return New typed_node(ref, ROOT_TYPE, ROOT_TYPE_STR, uint32_0, ref.size(), Nothing, subnodes)
    End Function

    Public NotInheritable Class child_named_map
        Private ReadOnly m As map(Of String, vector(Of typed_node))

        Public Sub New(ByVal lp As lang_parser, ByVal n As typed_node)
            assert(Not lp Is Nothing)
            assert(Not n Is Nothing)
            m = New map(Of String, vector(Of typed_node))()
            Dim i As UInt32 = 0
            While i < n.child_count()
                m(n.child(i).type_name).emplace_back(n.child(i))
                i += uint32_1
            End While
        End Sub

        Public Function nodes(ByVal name As String, ByRef o As vector(Of typed_node)) As Boolean
            Dim it As map(Of String, vector(Of typed_node)).iterator = Nothing
            it = m.find(name)
            If it = m.end() Then
                Return False
            End If
            o = (+it).second
            Return True
        End Function

        Public Function node(ByVal name As String, ByRef o As typed_node) As Boolean
            Dim v As vector(Of typed_node) = Nothing
            If Not nodes(name, v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            If v.size() <> 1 Then
                Return False
            End If
            o = v(0)
            Return True
        End Function

        Public Function nodes(ByVal name As String) As vector(Of typed_node)
            Dim o As vector(Of typed_node) = Nothing
            assert(nodes(name, o))
            Return o
        End Function

        Public Function node(ByVal name As String) As typed_node
            Dim o As typed_node = Nothing
            assert(node(name, o))
            Return o
        End Function
    End Class

    Public Function named_children(ByVal lp As lang_parser) As child_named_map
        Return New child_named_map(lp, Me)
    End Function

    Public Function root() As Boolean
        Return parent Is Nothing
    End Function

    Public Function child(ByVal id As UInt32) As typed_node
        assert(subnodes.available_index(id))
        Return subnodes(id)
    End Function

    Public Function child() As typed_node
        assert(child_count() = 1)
        Return child(0)
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

    Private Function nodes_str() As String
        Dim s As StringBuilder = Nothing
        s = New StringBuilder()
        Dim i As UInt32 = 0
        While i < subnodes.size()
            If i > 0 Then
                s.Append(", ")
            End If
            s.Append(subnodes(i).str())
            i += uint32_1
        End While
        Return Convert.ToString(s)
    End Function

    Public Function str() As String
        Return strcat("[", type, "<", start, ",", [end], "> {", newline.incode(), nodes_str(), newline.incode(), "}]")
    End Function

    ' TODO: Implementation
    Public Function stack_trace() As String

    End Function

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class
