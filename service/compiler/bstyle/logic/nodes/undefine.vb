﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class undefine
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New undefine()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4)
            Dim name As String = n.child(2).input_without_ignored()
            Return struct.undefine(name, o) OrElse
                   builders.of_undefine(name).to(o)
        End Function
    End Class
End Class
