﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Private MustInherit Class raw_value
        Implements code_gen(Of logic_writer)

        Private ReadOnly code_type As String

        Protected Sub New(ByVal code_type As String)
            assert(Not code_type.null_or_whitespace())
            Me.code_type = code_type
        End Sub

        Protected MustOverride Function parse(ByVal n As typed_node, ByRef o As data_block) As Boolean

        Protected Function build(ByVal i As data_block, ByVal o As logic_writer) As Boolean
            assert(Not i Is Nothing)
            assert(Not o Is Nothing)
            Return builders.of_copy_const(value.with_single_data_slot_temp_target(code_type, o), i).to(o)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.leaf())
            Dim d As data_block = Nothing
            If Not parse(n, d) Then
                raise_error(error_type.user, "Cannot parse data to ", code_type, " ", n.trace_back_str())
                Return False
            End If
            Return build(d, o)
        End Function
    End Class
End Class
