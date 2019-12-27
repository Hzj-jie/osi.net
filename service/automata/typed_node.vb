
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

' TODO: Implement typed_node.builder to ensure the immutability of typed_node.
Public NotInheritable Class typed_node
    Public Const ROOT_TYPE As UInt32 = uint32_0
    Public ReadOnly type As UInt32
    Public ReadOnly start As UInt32
    Public ReadOnly [end] As UInt32 'exclusive
    Public ReadOnly subnodes As vector(Of typed_node)
    Public ReadOnly ref As vector(Of typed_word)

    Public Sub New(ByVal ref As vector(Of typed_word),
                   ByVal type As UInt32,
                   ByVal start As UInt32,
                   ByVal [end] As UInt32,
                   ByVal ParamArray subnodes() As typed_node)
        assert(Not ref Is Nothing)
        assert(start <= [end])  ' start == end means empty-matching
        assert([end] <= ref.size())
        Me.ref = ref
        Me.type = type
        Me.start = start
        Me.end = [end]
        Me.subnodes = New vector(Of typed_node)()
        Me.subnodes.emplace_back(subnodes)

#If NOT_IMPLEMENTED Then
        If leaf() Then
            assert(word_count() = 1)
        End If
#End If
    End Sub

    ' create a root node
    Public Sub New(ByVal ref As vector(Of typed_word),
                   ByVal ParamArray subnodes() As typed_node)
        Me.New(ref, ROOT_TYPE, uint32_0, assert_not_nothing_return(ref).size(), subnodes)
    End Sub

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

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class
