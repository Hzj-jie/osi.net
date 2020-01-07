
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class condition
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of condition)()
        End Sub

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() >= 5)
            Using value_target As write_scoped(Of String).ref =
                logic_gen_of(Of value).with_value_target(n.child(2), types.bool, o)
                If Not logic_gen_of(Of value).build(n.child(2), o, "condition") Then
                    Return False
                End If
                Dim satisfied_paragraph As Func(Of Boolean) = Nothing
                satisfied_paragraph = Function() As Boolean
                                          If Not b.[of](n.child(4)).build(o) Then
                                              o.err("@condition paragraph ", n.child(4))
                                              Return False
                                          End If
                                          Return True
                                      End Function
                If n.child_count() = 5 Then
                    Return builders.of_if(+value_target, satisfied_paragraph).to(o)
                End If
                Dim unsatisfied_paragraph As Func(Of Boolean) = Nothing
                unsatisfied_paragraph = Function() As Boolean
                                            If Not b.[of](n.child(5)).build(o) Then
                                                o.err("@condition else-condition ", n.child(5))
                                                Return False
                                            End If
                                            Return True
                                        End Function
                Return builders.of_if(+value_target, satisfied_paragraph, unsatisfied_paragraph).to(o)
            End Using
        End Function
    End Class
End Class
