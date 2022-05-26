﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class reinterpret_cast
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New reinterpret_cast()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 6)
            Dim name As String = n.child(2).input_without_ignored()
            Dim type As String = scope.current().type_alias()(n.child(4).input_without_ignored())
            Return struct.redefine(name, type, o) OrElse
                   (builders.of_redefine(name, type).to(o) AndAlso scope.current().variables().redefine(type, name))
        End Function
    End Class
End Class
