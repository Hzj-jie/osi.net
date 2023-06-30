
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class typed_node
    Private NotInheritable Class stop_navigating_sub_nodes_exception
        Inherits Exception

        Public Shared ReadOnly instance As New stop_navigating_sub_nodes_exception()

        Private Sub New()
        End Sub
    End Class

    Private Shared Sub stop_navigating_sub_nodes()
        Throw stop_navigating_sub_nodes_exception.instance
    End Sub

    Public Sub dfs(ByVal node_handle As Action(Of typed_node, Action),
                   ByVal leaf_handle As Action(Of typed_node))
        assert(Not node_handle Is Nothing)
        assert(Not leaf_handle Is Nothing)

        If leaf() Then
            leaf_handle(Me)
            Return
        End If

        Try
            node_handle(Me, AddressOf stop_navigating_sub_nodes)
        Catch ex As stop_navigating_sub_nodes_exception
            Return
        End Try

        assert(child_count() > 0)
        For i As UInt32 = 0 To child_count() - uint32_1
            child(i).dfs(node_handle, leaf_handle)
        Next
    End Sub
End Class
