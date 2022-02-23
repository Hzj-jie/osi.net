﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.constructor
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class _string
        Implements code_gen(Of logic_writer)

        Private ReadOnly l As code_gens(Of logic_writer)

        <inject_constructor>
        Public Sub New(ByVal b As code_gens(Of logic_writer))
            assert(b IsNot Nothing)
            Me.l = b
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(n IsNot Nothing)
            assert(o IsNot Nothing)
            assert(n.leaf())
            Return builders.of_copy_const(value.with_single_data_slot_temp_target(code_types.string, n, o),
                                          New data_block(n.word().str().Trim(character.quote).c_unescape())).to(o)
        End Function
    End Class
End Class
