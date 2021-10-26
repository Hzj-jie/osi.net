
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
                              ByVal struct_copy As Action(Of vector(Of String), vector(Of builders.parameter)),
                              ByVal internal_typed_copy As Action(Of String, String),
                              ByVal o As writer) As Boolean
            assert(Not n Is Nothing)
            assert(n.leaf())
            assert(Not struct_copy Is Nothing)
            assert(Not internal_typed_copy Is Nothing)
            assert(Not o Is Nothing)
            Dim type As String = Nothing
            If Not scope.current().variables().resolve(n.word().str(), type) Then
                Return False
            End If
            Dim ps As vector(Of builders.parameter) = Nothing
            If scope.current().structs().resolve(type, n.word().str(), ps) Then
                struct_copy(l.typed_code_gen(Of value)().with_temp_target(type, n, o), ps)
            Else
                internal_typed_copy(l.typed_code_gen(Of value)().with_internal_typed_temp_target(type, n, o),
                                    n.word().str())
            End If
            Return True
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(n.child_count() = 1)
            Return build(n.child(),
                         Sub(ByVal vs As vector(Of String), ByVal ps As vector(Of builders.parameter))
                             assert(Not vs Is Nothing)
                             assert(Not ps Is Nothing)
                             assert(vs.size() = ps.size())
                             Dim i As UInt32 = 0
                             While i < vs.size()
                                 builders.of_copy(vs(i), ps(i).name).to(o)
                                 i += uint32_1
                             End While
                         End Sub,
                         Sub(ByVal target As String, ByVal source As String)
                             builders.of_copy(target, source).to(o)
                         End Sub,
                         o)
        End Function
    End Class
End Class
