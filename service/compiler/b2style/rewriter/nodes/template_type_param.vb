﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class template_type_param
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly l As code_gens(Of typed_node_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of typed_node_writer))
            assert(Not b Is Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not o Is Nothing)
            ' By default raw-type-name has namespace prefixed, so child().child() cannot be directly forwarded to
            ' raw-Type-name.
            If n.child().child(0).child().type_name.Equals("template-type-name") Then
                Dim extended_type As String = Nothing
                If Not l.of(n.child().child()).dump(extended_type) Then
                    Return False
                End If
                If Not o.append(extended_type) Then
                    Return False
                End If
                If n.child().child_count() = 1 Then
                    Return True
                End If
                assert(n.child().child_count() = 2)
                assert(n.child().child(1).type_name.Equals("reference"))
                Return o.append(n.child().child(1).children_word_str())
            End If
            Return o.append(n.children_word_str())
        End Function
    End Class
End Class

