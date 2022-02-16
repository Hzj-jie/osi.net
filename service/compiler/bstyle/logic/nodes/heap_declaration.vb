
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class heap_declaration
        Implements code_gen(Of logic_writer)

        Private ReadOnly l As code_gens(Of logic_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of logic_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            assert(n.child(1).child_count() = 4)
            Return build(n.child(0), n.child(1).child(0), n.child(1).child(2), o)
        End Function

        Public Function build(ByVal type As typed_node,
                              ByVal name As typed_node,
                              ByVal length As typed_node,
                              ByVal o As logic_writer) As Boolean
            assert(Not type Is Nothing)
            assert(Not name Is Nothing)
            assert(Not length Is Nothing)
            Dim t As String = type.input_without_ignored()
            Dim n As String = name.input_without_ignored()
            Return l.typed(Of struct).define_in_heap(t, n, length, o) OrElse
                   l.typed(Of heap_name).build(
                       length,
                       o,
                       Function(ByVal len_name As String) As Boolean
                           Return declare_single_data_slot(t, n, len_name, o)
                       End Function)
        End Function

        Public Shared Function declare_single_data_slot(ByVal type As String,
                                                        ByVal name As String,
                                                        ByVal length As String,
                                                        ByVal o As logic_writer) As Boolean
            assert(Not scope.current().structs().defined(type))
            assert(Not o Is Nothing)
            Return scope.current().variables().define_heap(type, name) AndAlso
                   builders.of_define_heap(name,
                                           scope.current().type_alias()(type),
                                           length).
                            to(o)
        End Function
    End Class
End Class
