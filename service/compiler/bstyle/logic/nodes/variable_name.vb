
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
    Public NotInheritable Class variable_name
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of variable_name)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal struct_handle As Func(Of String, vector(Of builders.parameter), Boolean),
                              ByVal single_data_slot_handle As Func(Of String, String, Boolean),
                              ByVal o As writer) As Boolean
            assert(Not n Is Nothing)
            assert(n.leaf())
            assert(Not struct_handle Is Nothing)
            assert(Not single_data_slot_handle Is Nothing)
            assert(Not o Is Nothing)
            Dim type As String = Nothing
            If Not scope.current().variables().resolve(n.word().str(), type) Then
                Return False
            End If
            Dim ps As vector(Of builders.parameter) = Nothing
            If scope.current().structs().resolve(type, n.word().str(), ps) Then
                Return struct_handle(type, ps)
            End If
            Return single_data_slot_handle(type, n.word().str())
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(n.child_count() = 1)
            Return build(n.child(),
                         Function(ByVal type As String, ByVal ps As vector(Of builders.parameter)) As Boolean
                             l.typed_code_gen(Of value)().with_target(type, ps)
                             Return True
                         End Function,
                         Function(ByVal type As String, ByVal source As String) As Boolean
                             l.typed_code_gen(Of value)().with_single_data_slot_target(type, source)
                             Return True
                         End Function,
                         o)
        End Function
    End Class
End Class
