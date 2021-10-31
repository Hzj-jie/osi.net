
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
                              ByVal struct_handle As Action(Of String, vector(Of builders.parameter)),
                              ByVal single_data_slot_handle As Action(Of String, String),
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
                struct_handle(type, ps)
            Else
                single_data_slot_handle(type, n.word().str())
            End If
            Return True
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(n.child_count() = 1)
            Return build(n.child(),
                         Sub(ByVal type As String, ByVal ps As vector(Of builders.parameter))
                             l.typed_code_gen(Of value)().with_target(ps)
                         End Sub,
                         Sub(ByVal type As String, ByVal source As String)
                             l.typed_code_gen(Of value)().with_single_data_slot_target(source)
                         End Sub,
                         o)
        End Function
    End Class
End Class
