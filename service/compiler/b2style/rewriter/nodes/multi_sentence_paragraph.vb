﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class multi_sentence_paragraph
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(b IsNot Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(n IsNot Nothing)
            assert(o IsNot Nothing)
            assert(n.child_count() >= 2)
            assert(n.child(0).type_name.Equals("start-paragraph"))
            assert(n.last_child().type_name.Equals("end-paragraph"))
            If Not o.append("{") Then
                Return False
            End If
            ' Ensure the scope.end_scope executes before "}".
            Using scope.current().start_scope()
                Dim i As UInt32 = 1
                While i < n.child_count() - uint32_1
                    If Not l.of(n.child(i)).build(o) Then
                        Return False
                    End If
                    i += uint32_1
                End While
            End Using
            Return o.append("}")
        End Function
    End Class
End Class
