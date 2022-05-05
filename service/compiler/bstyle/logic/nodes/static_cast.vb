﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic

Partial Public NotInheritable Class bstyle
    Private NotInheritable Class static_cast
        Implements code_gen(Of logic_writer)

        Public Shared ReadOnly instance As New static_cast()

        Private Sub New()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 6)
            Dim name As String = n.child(2).input_without_ignored()
            Dim type As String = scope.current().type_alias()(n.child(4).input_without_ignored())
            If scope.current().structs().types().defined(type) AndAlso
               scope.current().structs().variables().defined(name) Then
                ' Convert from struct ptr to struct ptr
            End If
            If scope.current().structs().types().defined(type) Then
                ' Convert from type_ptr to struct ptr
            End If
            If scope.current().structs().variables().defined(name) Then
                ' Convert from struct ptr to type_ptr
            End If
            ' Convert from type_ptr to type_ptr
            Return scope.current().variables().redefine(type, name)
        End Function
    End Class
End Class
