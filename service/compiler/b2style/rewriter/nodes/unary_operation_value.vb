﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class unary_operation_value
        Implements code_gen(Of typed_node_writer)

        Private ReadOnly operator_index As UInt32
        Private ReadOnly suffix As String

        Public Sub New(ByVal operator_index As UInt32, ByVal suffix As String)
            assert(Not suffix.null_or_whitespace())
            Me.operator_index = operator_index
            Me.suffix = suffix
        End Sub

        Private Function build(ByVal n As typed_node,
                               ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(n.child_count() = 2)
            Dim function_name As String =
                    _namespace.bstyle_format.operator_function_name(n.child(operator_index).type_name) + suffix
            scope.current().call_hierarchy().to_bstyle_function(function_name)
            o.append(function_name)
            o.append("(")
            If Not code_gen_of(n.child(uint32_1 - operator_index)).build(o) Then
                Return False
            End If
            o.append(")")
            Return True
        End Function
    End Class
End Class
