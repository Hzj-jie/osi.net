
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class typed_node
    Public NotInheritable Class child_named_map
        Private ReadOnly m As map(Of String, vector(Of typed_node))

        Public Sub New(ByVal n As typed_node)
            assert(n IsNot Nothing)
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
            assert(v IsNot Nothing)
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

    Public Function named_children() As child_named_map
        Return New child_named_map(Me)
    End Function
End Class
