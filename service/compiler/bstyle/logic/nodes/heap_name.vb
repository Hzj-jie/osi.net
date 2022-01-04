
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
    Public NotInheritable Class heap_name
        Inherits code_gen_wrapper(Of writer)
        Implements code_gen(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of writer))
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
            Using read_target As read_scoped(Of scope.value_target_t.target).ref(Of String) =
                    value.read_target_single_data_slot()
                ' TODO: May want to restrict the type of indexstr.
                If Not read_target.retrieve(indexstr) Then
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
                             Return raw_variable_name.build(
                                        n.child(0),
                                        Function(ByVal type As String,
                                                 ByVal ps As vector(Of single_data_slot_variable)) As Boolean
                                            value.with_target(
                                                type,
                                                ps.stream().
                                                   map(Function(ByVal d As single_data_slot_variable) As _
                                                           single_data_slot_variable
                                                           assert(Not d Is Nothing)
                                                           Return d.with_index(indexstr)
                                                       End Function).
                                                   collect(Of vector(Of single_data_slot_variable))())
                                            Return True
                                        End Function,
                                        Function(ByVal type As String, ByVal source As String) As Boolean
                                            value.with_single_data_slot_target(
                                                type, variable.name_of(source, indexstr))
                                            Return True
                                        End Function,
                                        o)
                         End Function)
        End Function
    End Class
End Class
