
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class [class]
        Inherits code_gen_wrapper(Of typed_node_writer)
        Implements code_gen(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            b.register(Of [class])()
        End Sub

        Private Shared Function is_value_declaration(ByVal n As typed_node) As Boolean
            assert(Not n Is Nothing)
            assert(n.type_name.Equals("value-declaration-with-semi-colon") OrElse
                   n.type_name.Equals("function"))
            Return n.type_name.Equals("value-declaration-with-semi-colon")
        End Function

        Private Shared Function is_function(ByVal n As typed_node) As Boolean
            assert(Not n Is Nothing)
            assert(n.type_name.Equals("value-declaration-with-semi-colon") OrElse
                   n.type_name.Equals("function"))
            Return n.type_name.Equals("function")
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            o.append("struct")
            Dim class_name As String = namespace_.bstyle_format(n.child(1).word().str())
            o.append(class_name)
            o.append("{")
            ' Append variables into the structure.
            If Not streams.range(3, n.child_count() - uint32_2).
                           map(Function(ByVal i As Int32) As typed_node
                                   Return n.child(CUInt(i))
                               End Function).
                           filter(AddressOf is_value_declaration).
                           map(Function(ByVal node As typed_node) As Boolean
                                   Return l.typed_code_gen(Of struct)().build_value_declaration(node, o)
                               End Function).
                           aggregate(bool_stream.aggregators.all_true, True) Then
                Return False
            End If
            o.append("};")
            ' Append functions after the structure.
            If Not streams.range(3, n.child_count() - uint32_2).
                           map(Function(ByVal i As Int32) As typed_node
                                   Return n.child(CUInt(i))
                               End Function).
                           filter(AddressOf is_function).
                           map(Function(ByVal node As typed_node) As Boolean
                                   assert(Not node Is Nothing)
                                   assert(node.child_count() = 5 OrElse node.child_count() = 6)
                                   If Not l.of(node.child(0)).build(o) Then
                                       Return False
                                   End If
                                   ' No namespace is necessary, the first parameter contains namespace.
                                   o.append(namespace_.bstyle_format_in_global_namespace(
                                                node.child(1).children_word_str()))
                                   o.append("(")
                                   o.append(class_name)
                                   o.append("&")
                                   o.append(namespace_.bstyle_format("this"))
                                   If node.child_count() = 6 Then
                                       o.append(", ")
                                       If Not l.of(node.child(3)).build(o) Then
                                           Return False
                                       End If
                                   End If
                                   o.append(")")
                                   If Not l.of(node.last_child()).build(o) Then
                                       Return False
                                   End If
                                   Return True
                               End Function).
                           aggregate(bool_stream.aggregators.all_true, True) Then
                Return False
            End If
            Return True
        End Function
    End Class
End Class
