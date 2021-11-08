﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
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
            If Not l.of(n.child(2)).build(o) Then
                Return False
            End If
            Using read_target As read_scoped(Of value.target).ref(Of value.single_data_slot_target) =
                    l.typed_code_gen(Of value)().read_target_single_data_slot()
                Dim condition As String = Nothing
                ' TODO: May want to restrict the type of condition.
                If Not value.single_data_slot_target.ignore_type(read_target, condition) Then
                    raise_error(error_type.user, "Condition of if cannot be a struct.")
                    Return False
                End If
                Dim satisfied_paragraph As Func(Of Boolean) = Function() As Boolean
                                                                  Return l.[of](n.child(4)).build(o)
                                                              End Function
                If n.child_count() = 5 Then
                    Return builders.of_if(condition, satisfied_paragraph).to(o)
                End If
                Dim unsatisfied_paragraph As Func(Of Boolean) = Function() As Boolean
                                                                    Return l.[of](n.child(5)).build(o)
                                                                End Function
                Return builders.of_if(condition, satisfied_paragraph, unsatisfied_paragraph).to(o)
            End Using
        End Function
    End Class
End Class
