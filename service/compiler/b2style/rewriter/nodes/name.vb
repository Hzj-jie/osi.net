
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class name
        Inherits code_gen_wrapper(Of typed_node_writer)
        Implements code_gen(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal i As code_gens(Of typed_node_writer))
            MyBase.New(i)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            b.register(Of name)()
        End Sub

        Private Shared Function should_prefix_with_namespace(ByVal n As typed_node) As Boolean
            assert(Not n Is Nothing)
            assert(Not n.parent() Is Nothing)
            Return n.parent().type_name.Equals("function") OrElse
                   n.parent().type_name.Equals("function-call") OrElse
                   n.parent().type_name.Equals("struct") OrElse
                   n.parent().type_name.Equals("class") OrElse
                   n.parent().type_name.Equals("typedef-type") OrElse
                   ((n.parent().type_name.Equals("param") OrElse
                     n.parent().type_name.Equals("value-declaration") OrElse
                     n.parent().type_name.Equals("value-definition") OrElse
                     n.parent().type_name.Equals("heap-declaration")) AndAlso
                    n.parent().child_index(n) = 0)
        End Function

        Public Function name(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            assert(n.leaf())
            assert(n.type_name.Equals("name"))
            If should_prefix_with_namespace(n) Then
                Return l.typed_code_gen(Of namespace_)().bstyle_format(n.word().str())
            Else
            End If
            Return n.word().str()
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            o.append(name(n))
            Return True
        End Function
    End Class
End Class
