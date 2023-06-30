
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class heap_declaration
        Implements code_gen(Of logic_writer)

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            assert(n.child(1).child_count() = 4)
            Dim type As typed_node = n.child(0)
            Dim name As typed_node = n.child(1).child(0)
            Dim length As typed_node = n.child(1).child(2)
            assert(Not type Is Nothing)
            assert(Not name Is Nothing)
            assert(Not length Is Nothing)
            Dim type_str As String = type.input_without_ignored()
            Dim name_str As String = name.input_without_ignored()
            Return struct.define_in_heap(type_str, name_str, length, o) OrElse
                   heap_name.build(length,
                                   o,
                                   Function(ByVal len_name As String) As Boolean
                                       Return declare_primitive_type(type_str, name_str, len_name, o)
                                   End Function)
        End Function

        Public Shared Function declare_primitive_type(ByVal type As String,
                                                            ByVal name As String,
                                                            ByVal length As String,
                                                            ByVal o As logic_writer) As Boolean
            assert(Not scope.current().structs().types().defined(type))
            assert(Not o Is Nothing)
            Return scope.current().variables().define(type, name) AndAlso
                   builders.of_define_heap(name,
                                           scope.normalized_type.logic_type_of(type),
                                           length).
                            to(o)
        End Function
    End Class
End Class
