
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class heap_name
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of heap_name)()
        End Sub

        Public Function build(ByVal index As typed_node,
                              ByVal o As writer,
                              ByVal f As Func(Of String, Boolean)) As Boolean
            assert(Not index Is Nothing)
            assert(Not o Is Nothing)
            assert(Not f Is Nothing)
            If Not l.of(index).build(o) Then
                Return False
            End If
            Using read_target As read_scoped(Of vector(Of String)).ref(Of String) =
                    l.typed_code_gen(Of value)().read_target_internal_typed()
                Dim indexstr As String = Nothing
                If Not read_target.retrieve(indexstr) Then
                    raise_error(error_type.user, "Index or length of a heap declaration cannot be a struct.")
                    Return False
                End If
                Return f(indexstr)
            End Using
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4)
            Return build(n.child(2),
                         o,
                         Function(ByVal indexstr As String) As Boolean
                             Return l.typed_code_gen(Of variable_name)().build(
                                        n.child(0),
                                        Sub(ByVal vs As vector(Of String),
                                            ByVal ps As vector(Of builders.parameter))
                                            assert(Not vs Is Nothing)
                                            assert(Not ps Is Nothing)
                                            assert(vs.size() = ps.size())
                                            Dim i As UInt32 = 0
                                            While i < vs.size()
                                                builders.of_copy_heap_out(vs(i), ps(i).name, indexstr).to(o)
                                                i += uint32_1
                                            End While
                                        End Sub,
                                        Sub(ByVal target As String, ByVal source As String)
                                            builders.of_copy_heap_out(target, source, indexstr).to(o)
                                        End Sub,
                                        o)
                         End Function)
        End Function
    End Class
End Class
