﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor

Partial Public NotInheritable Class cstyle
    Public NotInheritable Class [while]
        Inherits logic_gen_wrapper
        Implements logic_gen

        <inject_constructor>
        Public Sub New(ByVal b As logic_gens)
            MyBase.New(b)
        End Sub

        Public Shared Sub register(ByVal b As logic_gens)
            assert(Not b Is Nothing)
            b.register(Of [while])()
        End Sub

        Private Function while_value(ByVal n As typed_node, ByVal o As writer) As Boolean
            Return logic_gen_of(Of value).build(n.child(2), o, "while")
        End Function

        Public Function build(ByVal n As typed_node, ByVal o As writer) As Boolean Implements logic_gen.build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 5)
            Using value_target As write_scoped(Of String).ref =
                logic_gen_of(Of value).with_value_target(n.child(2), types.bool, o)
                If Not while_value(n, o) Then
                    Return False
                End If
                Return builders.of_while_then(+value_target,
                                              Function() As Boolean
                                                  If Not b.[of](n.child(4)).build(o) Then
                                                      o.err("@while paragraph ", n.child(4))
                                                      Return False
                                                  End If
                                                  If Not while_value(n, o) Then
                                                      Return False
                                                  End If
                                                  Return True
                                              End Function).to(o)
            End Using
        End Function
    End Class
End Class
