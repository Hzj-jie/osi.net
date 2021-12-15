
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class template
        Inherits code_gen_wrapper(Of typed_node_writer)
        Implements code_gen(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            b.register(Of template)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 5)
            Dim types As New unordered_map(Of String, String)()
            For i As UInt32 = 0 To n.child(2).child_count() - uint32_1
                Dim tn As typed_node
                If n.child(2).child(i).type_name.Equals("type-param-with-comma") Then
                    tn = n.child(2).child(i).child(0)
                Else
                    assert(n.child(2).child(i).type_name.Equals("type-param"))
                    tn = n.child(2).child(i)
                End If
                If Not types.emplace(tn.child().word().str(), strcat("{", i, "}")).second() Then
                    raise_error(error_type.user,
                                "Template parameter ",
                                tn.child().word().str(),
                                " has been defined already.")
                    Return False
                End If
            Next
            Return True
        End Function
    End Class
End Class
