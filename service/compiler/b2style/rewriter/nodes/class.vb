
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
    Public NotInheritable Class _class
        Inherits code_gens(Of typed_node_writer).reparser(Of b2style.parser)
        Implements code_gen(Of typed_node_writer)

        Public Shared ReadOnly instance As New _class()

        Private Sub New()
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
            Dim class_name As String = n.child(1).input()
            If Not scope.current().classes().define(class_name) Then
                Return False
            End If
            o.Append("struct ")
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
            Dim has_constructor As Boolean = False
            Dim has_destructor As Boolean = False
            streams.range(3, n.child_count() - uint32_2).
                    map(class_body).
                    filter(AddressOf is_function).
                    foreach(Sub(ByVal node As typed_node)
                                assert(Not node Is Nothing)
                                assert(node.child_count() = 5 OrElse node.child_count() = 6)
                                If node.child(1).input().Equals("construct") Then
                                    has_constructor = True
                                ElseIf node.child(1).input().Equals("destruct") Then
                                    has_destructor = True
                                End If
                                ' No namespace is necessary, the first parameter contains namespace.
                                o.Append(node.child(0).input()).
                                  Append(" ").
                                  Append(_namespace.with_global_namespace(node.child(1).input())).
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
            If Not has_constructor Then
                o.Append("void ").
                  Append(_namespace.with_global_namespace("construct")).
                  Append("(").
                  Append(class_name).
                  Append("& this){}")
            End If
            If Not has_destructor Then
                o.Append("void ").
                  Append(_namespace.with_global_namespace("destruct")).
                  Append("(").
                  Append(class_name).
                  Append("& this){}")
            End If
            s = o.ToString()
            Return True
        End Function
    End Class
End Class
