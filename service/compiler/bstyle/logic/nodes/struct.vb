
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class struct
        Implements logic_gen

        Private Shared ReadOnly instance As New struct()

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(instance)
        End Sub

        Private Sub New()
        End Sub

        Public Function export(ByVal n As typed_node, ByVal o As writer) As Boolean
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            If n.child_count() > 2 Then
                ' TODO: Support value-definition
                Return False
            End If
            assert(n.child_count() = 2)
            Dim v As vector(Of builders.parameter) = Nothing
            If Not scope.current().structs().resolve(n.child(0).word().str(), n.child(1).word().str(), v) Then
                Return False
            End If
            assert(Not v Is Nothing)
            v.stream().foreach(Sub(ByVal m As builders.parameter)
                                   assert(Not m Is Nothing)
                                   builders.of_define(m.name, m.type).to(o)
                               End Sub)
            Return True
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As writer) As Boolean Implements code_gen(Of writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            Return scope.current().
                         structs().
                         define(n.child(1).word().str(),
                                streams.range_closed(CUInt(3), n.child_count() - CUInt(3)).
                                        map(Function(ByVal index As Int32) As builders.parameter
                                                ' TODO: Support value_definition.
                                                Dim c As typed_node = n.child(CUInt(index))
                                                assert(Not c Is Nothing)
                                                assert(c.child_count() = 2)
                                                assert(c.child(0).child_count() = 2)
                                                Return New builders.parameter(c.child(0).child(0).word().str(),
                                                                              c.child(0).child(1).word().str())
                                            End Function).
                                        collect(Of vector(Of builders.parameter))())
        End Function
    End Class
End Class
