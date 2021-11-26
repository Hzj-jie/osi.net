
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class struct
        Inherits code_gen_wrapper(Of typed_node_writer)
        Implements code_gen(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal i As code_gens(Of typed_node_writer))
            MyBase.New(i)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            b.register(Of struct)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not n.leaf())
            Dim i As UInt32 = 0
            While i < n.child_count()
                If Not build_child(n.child(i), o) Then
                    Return False
                End If
                i += uint32_1
            End While
            Return True
        End Function

        Public Function build_value_declaration(ByVal child As typed_node, ByVal o As typed_node_writer) As Boolean
            assert(Not child Is Nothing)
            assert(child.type_name.Equals("value-declaration-with-semi-colon"))
            Return build_child(child, o)
        End Function

        Private Function build_child(ByVal child As typed_node, ByVal o As typed_node_writer) As Boolean
            assert(Not child Is Nothing)
            assert(Not o Is Nothing)
            If Not child.type_name.Equals("value-declaration-with-semi-colon") Then
                Return l.of(child).build(o)
            End If

            ' TODO: Support value-definition
            assert(child.child_count() = 2)
            assert(child.child(0).child_count() = 2)
            If Not l.of(child.child(0).child(0)).build(o) Then
                Return False
            End If
            ' Ignore namespace prefix for variables within the structure.
            o.append(namespace_.bstyle_format_in_global_namespace(child.child(0).child(1).word().str()))
            If Not l.of(child.child(1)).build(o) Then
                Return False
            End If
            Return True
        End Function
    End Class
End Class
