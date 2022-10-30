
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class raw_variable_name
        Implements code_gen(Of logic_writer)

        Public Shared Function build(ByVal n As typed_node,
                                     ByVal struct_handle As Func(Of String, stream(Of builders.parameter), Boolean),
                                     ByVal single_data_slot_handle As Func(Of String, String, Boolean),
                                     ByVal o As logic_writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not struct_handle Is Nothing)
            assert(Not single_data_slot_handle Is Nothing)
            assert(Not o Is Nothing)

            Dim type As String = Nothing
            If Not scope.current().variables().resolve(n.input_without_ignored(), type) Then
                Return False
            End If
            Dim ps As scope.struct_def = Nothing
            If scope.current().structs().resolve(type, n.input_without_ignored(), ps) Then
                Return struct_handle(type, ps.primitives())
            End If
            Return single_data_slot_handle(type, n.input_without_ignored())
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 1)
            Return build(n.child(),
                         Function(ByVal type As String, ByVal ps As stream(Of builders.parameter)) As Boolean
                             scope.current().value_target().with_value(type, ps)
                             Return True
                         End Function,
                         Function(ByVal type As String, ByVal source As String) As Boolean
                             scope.current().value_target().with_value(type, source)
                             Return True
                         End Function,
                         o)
        End Function
    End Class
End Class
