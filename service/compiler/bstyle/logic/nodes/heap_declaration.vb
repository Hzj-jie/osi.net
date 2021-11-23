
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class heap_declaration
        Inherits code_gen_wrapper(Of writer)
        Implements code_gen(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            b.register(Of heap_declaration)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            assert(n.child(1).child_count() = 4)
            Return build(n.child(0), n.child(1).child(0), n.child(1).child(2), o)
        End Function

        Public Function build(ByVal length As typed_node,
                              ByVal o As writer,
                              ByVal f As Func(Of String, Boolean)) As Boolean
            Return l.typed_code_gen(Of heap_name).build(length, o, f)
        End Function

        Public Function build(ByVal type As typed_node,
                              ByVal name As typed_node,
                              ByVal length As typed_node,
                              ByVal o As writer) As Boolean
            assert(Not type Is Nothing)
            assert(Not name Is Nothing)
            assert(Not length Is Nothing)
            Dim t As String = type.word().str()
            Dim n As String = name.word().str()
            If l.typed_code_gen(Of struct).define_in_heap(t, n, length, o) Then
                Return True
            End If
            Return build(length,
                         o,
                         Function(ByVal len_name As String) As Boolean
                             Return declare_single_data_slot(t, n, len_name, o)
                         End Function)
        End Function

        Public Shared Function declare_single_data_slot(ByVal type As String,
                                                        ByVal name As String,
                                                        ByVal length As String,
                                                        ByVal o As writer) As Boolean
            assert(Not scope.current().structs().defined(type))
            assert(Not o Is Nothing)
            Return scope.current().variables().define(type, name) AndAlso
                   builders.of_define_heap(name,
                                           scope.current().type_alias()(type),
                                           length).
                            to(o)
        End Function
    End Class
End Class
