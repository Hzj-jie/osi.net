
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class _class
        Inherits code_gens(Of typed_node_writer).reparser(Of b2style.parser)
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Protected Overrides Function dump(ByVal n As typed_node, ByRef s As String) As Boolean
            Dim o As New StringBuilder()
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            Dim class_name As String = n.child(1).input()
            Dim cd As New class_def(class_name)
            bstyle.struct.parse_struct_body(n).foreach(AddressOf cd.with_var)
            Dim classes As scope.class_proxy = scope.current().classes()
            If n.child(2).type_name.Equals("class-inheritance") AndAlso
               Not l.of_all_children(n.child(2).child(1)).
                     dump().
                     stream().
                     with_index().
                     map(Function(ByVal t As tuple(Of UInt32, String)) As Boolean
                             Dim bcd As class_def = Nothing
                             If Not classes.resolve(t.second(), bcd) Then
                                 Return False
                             End If
                             cd.inherit_from(bcd).
                                with_var(bstyle.struct.create_id(t.second()))
                             Return True
                         End Function).
                     aggregate(bool_stream.aggregators.all_true) Then
                Return False
            End If
            If Not scope.current().classes().define(class_name, cd) Then
                Return False
            End If
            ' Append struct-body back into the structure.
            o.Append("struct ").
              Append(class_name).
              Append("{")
            cd.vars().
               foreach(Sub(ByVal var As builders.parameter)
                           o.Append(var.type).
                             Append(" ").
                             Append(var.name).
                             Append(";")
                       End Sub)
            o.Append("};")
            ' Append functions after the structure.
            Dim has_constructor As Boolean = False
            Dim has_destructor As Boolean = False
            n.children_of("class-function").
              stream().
              map(Function(ByVal node As typed_node) As typed_node
                      assert(Not node Is Nothing)
                      node = node.child()
                      If node.type_name.Equals("virtual-function") OrElse
                         node.type_name.Equals("override-function") Then
                          Return node.child(1)
                      End If
                      Return node
                  End Function).
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
