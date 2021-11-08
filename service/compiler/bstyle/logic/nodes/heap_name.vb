
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
            Dim indexstr As String = Nothing
            Using read_target As read_scoped(Of value.target).ref(Of value.single_data_slot_target) =
                    l.typed_code_gen(Of value)().read_target_single_data_slot()
                ' TODO: May want to restrict the type of indexstr.
                If Not value.single_data_slot_target.ignore_type(read_target, indexstr) Then
                    raise_error(error_type.user, "Index or length of a heap declaration cannot be a struct.")
                    Return False
                End If
            End Using
            Return f(indexstr)
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
                                        Function(ByVal type As String,
                                                 ByVal ps As vector(Of builders.parameter)) As Boolean
                                            Dim vs As vector(Of String) =
                                                l.typed_code_gen(Of value)().with_temp_target(type, n, o)
                                            assert(Not ps Is Nothing)
                                            assert(vs.size() = ps.size())
                                            Dim i As UInt32 = 0
                                            While i < vs.size()
                                                If Not builders.of_copy_heap_out(vs(i), ps(i).name, indexstr).to(o) Then
                                                    Return False
                                                End If
                                                i += uint32_1
                                            End While
                                            Return True
                                        End Function,
                                        Function(ByVal type As String, ByVal source As String) As Boolean
                                            Return builders.of_copy_heap_out(
                                                                l.typed_code_gen(Of value)().
                                                                  with_single_data_slot_temp_target(type, n, o),
                                                                source,
                                                                indexstr).
                                                            to(o)
                                        End Function,
                                        o)
                         End Function)
        End Function
    End Class
End Class
