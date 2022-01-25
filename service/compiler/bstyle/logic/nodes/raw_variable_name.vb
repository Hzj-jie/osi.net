
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class raw_variable_name
        Implements code_gen(Of writer)

        Private ReadOnly l As code_gens(Of writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Shared Function build(ByVal n As typed_node,
                                     ByVal struct_handle As Func(Of String, vector(Of single_data_slot_variable), Boolean),
                                     ByVal single_data_slot_handle As Func(Of String, String, Boolean),
                                     ByVal o As writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not struct_handle Is Nothing)
            assert(Not single_data_slot_handle Is Nothing)
            assert(Not o Is Nothing)

            Dim type As String = Nothing
            If Not scope.current().variables().resolve(n.input_without_spacing(), type) Then
                Return False
            End If
            Dim ps As struct_def = Nothing
            If scope.current().structs().resolve(type, n.input_without_spacing(), ps) Then
                Return struct_handle(type, ps.expanded)
            End If
            Return single_data_slot_handle(type, n.input_without_spacing())
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 1)
            Return build(n.child(),
                         Function(ByVal type As String, ByVal ps As vector(Of single_data_slot_variable)) As Boolean
                             value.with_target(type, ps)
                             Return True
                         End Function,
                         Function(ByVal type As String, ByVal source As String) As Boolean
                             value.with_single_data_slot_target(type, source)
                             Return True
                         End Function,
                         o)
        End Function
    End Class
End Class
