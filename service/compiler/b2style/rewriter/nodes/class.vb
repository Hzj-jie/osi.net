
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Public NotInheritable Class [class]
        Inherits code_gens(Of typed_node_writer).reparser(Of b2style.parser)
        Implements code_gen(Of typed_node_writer)

        Private Shared ReadOnly instance As New [class]()

        Private Sub New()
        End Sub

        Public Shared ReadOnly code_gen As Action(Of code_gens(Of typed_node_writer)) =
            Sub(ByVal b As code_gens(Of typed_node_writer))
                assert(Not b Is Nothing)
                b.register(instance)
            End Sub

        Private Shared Sub ensure_subnode_type(ByVal n As typed_node)
            assert(Not n Is Nothing)
            assert(n.type_name.Equals("struct-body") OrElse
                   n.type_name.Equals("function"),
                   n.type_name)
        End Sub

        Private Shared Function is_struct_body(ByVal n As typed_node) As Boolean
            ensure_subnode_type(n)
            Return n.type_name.Equals("struct-body")
        End Function

        Private Shared Function is_function(ByVal n As typed_node) As Boolean
            ensure_subnode_type(n)
            Return n.type_name.Equals("function")
        End Function

        Protected Overrides Function dump(ByVal n As typed_node, ByRef s As String) As Boolean
            Dim o As New StringBuilder()
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            o.Append("struct ")
            Dim class_name As String = n.child(1).input()
            o.Append(class_name).Append("{")
            ' Append struct-body back into the structure.
            Dim class_body As Func(Of Int32, typed_node) =
                    Function(ByVal i As Int32) As typed_node
                        Dim c As typed_node = n.child(CUInt(i))
                        assert(c.type_name.Equals("class-body"))
                        Return c.child()
                    End Function
            streams.range(3, n.child_count() - uint32_2).
                    map(class_body).
                    filter(AddressOf is_struct_body).
                    foreach(Sub(ByVal node As typed_node)
                                o.Append(node.input())
                            End Sub)
            o.Append("};")
            ' Append functions after the structure.
            streams.range(3, n.child_count() - uint32_2).
                    map(class_body).
                    filter(AddressOf is_function).
                    foreach(Sub(ByVal node As typed_node)
                                assert(Not node Is Nothing)
                                assert(node.child_count() = 5 OrElse node.child_count() = 6)
                                ' No namespace is necessary, the first parameter contains namespace.
                                o.Append(node.child(0).input()).
                                  Append(" ").
                                  Append(namespace_.with_global_namespace(node.child(1).input())).
                                  Append("(").
                                  Append(class_name).
                                  Append("& this")
                                ' With parameter list.
                                If node.child_count() = 6 Then
                                    o.Append(", ").
                                      Append(node.child(3).input())
                                End If
                                o.Append(")").
                                  Append(node.last_child().input()).
                                  AppendLine() ' beautiful output.
                            End Sub)
            s = o.ToString()
            Return True
        End Function
    End Class
End Class
