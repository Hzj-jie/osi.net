
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class heap_declaration
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of heap_declaration)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 2)
            assert(n.child(1).child_count() = 4)
            Return build(n.child(0), n.child(1).child(0), n.child(1).child(2), o)
        End Function

        Public Function build(ByVal length As typed_node,
                              ByVal o As writer,
                              ByVal f As Func(Of String, Boolean)) As Boolean
            assert(Not length Is Nothing)
            assert(Not o Is Nothing)
            assert(Not f Is Nothing)
            If Not l.of(length).build(o) Then
                Return False
            End If
            Using read_target As read_scoped(Of vector(Of String)).ref(Of String) =
                    code_gen_of(Of value)().read_target_internal_typed()
                Dim lenstr As String = Nothing
                If Not read_target.retrieve(lenstr) Then
                    raise_error(error_type.user, "Length of a heap declaration cannot be a struct.")
                    Return False
                End If
                Return f(lenstr)
            End Using
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
            If code_gen_of(Of struct).define_in_heap(t, n, length, o) Then
                Return True
            End If
            Return build(length,
                         o,
                         Function(ByVal len_name As String) As Boolean
                             Return declare_internal_typed(t, n, len_name, o)
                         End Function)
        End Function

        Public Shared Function declare_internal_typed(ByVal type As String,
                                                      ByVal name As String,
                                                      ByVal length As String,
                                                      ByVal o As writer) As Boolean
            assert(Not scope.current().structs().defined(type))
            assert(Not o Is Nothing)
            If Not scope.current().variables().define(type, name) Then
                Return False
            End If
            builders.of_define_heap(name,
                                    scope.current().type_alias()(type),
                                    length).
                     to(o)
            Return True
        End Function
    End Class
End Class
