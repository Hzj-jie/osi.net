
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

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 1)
            Dim type As String = Nothing
            If Not scope.current().variables().resolve(n.child().word().str(), type) Then
                Return False
            End If
            Dim ps As vector(Of builders.parameter) = Nothing
            If scope.current().structs().resolve(type, n.child().word().str(), ps) Then
                Dim vs As vector(Of String) = code_gen_of(Of value)().with_temp_target(type, n, o)
                assert(Not vs Is Nothing)
                assert(vs.size() = ps.size())
                Dim i As UInt32 = 0
                While i < vs.size()
                    builders.of_copy(vs(i), ps(i).name).to(o)
                    i += uint32_1
                End While
            Else
                builders.of_copy(code_gen_of(Of value)().with_internal_typed_temp_target(type, n, o),
                                 n.child().word().str()).
                         to(o)
            End If
            Return True
        End Function
    End Class
End Class
