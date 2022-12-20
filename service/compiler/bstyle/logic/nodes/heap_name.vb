﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.automata
Imports builders = osi.service.compiler.logic.builders
Imports variable = osi.service.compiler.logic.variable

Partial Public NotInheritable Class bstyle
    Public Class heap_name(Of BUILDER As func_t(Of String, logic_writer, Boolean),
                              CODE_GENS As func_t(Of code_gens(Of logic_writer)),
                              T As scope(Of logic_writer, BUILDER, CODE_GENS, T))
        Implements code_gen(Of logic_writer)

        Public Shared Function build(ByVal index As typed_node,
                                     ByVal o As logic_writer,
                                     ByVal f As Func(Of String, Boolean)) As Boolean
            assert(Not index Is Nothing)
            assert(Not o Is Nothing)
            assert(Not f Is Nothing)
            If Not code_gen_of(index).build(o) Then
                Return False
            End If
            Dim indexstr As String = Nothing
            Using read_target As read_scoped(Of scope(Of logic_writer, BUILDER, CODE_GENS, T).value_target_t.target).
                                 ref(Of String) =
                    scope(Of logic_writer, BUILDER, CODE_GENS, T).current().value_target().primitive_type()
                ' TODO: May want to restrict the type of indexstr.
                If Not read_target.retrieve(indexstr) Then
                    raise_error(error_type.user, "Index or length of a heap declaration cannot be a struct.")
                    Return False
                End If
            End Using
            Return f(indexstr)
        End Function

        Private Function build(ByVal n As typed_node,
                               ByVal o As logic_writer) As Boolean Implements code_gen(Of logic_writer).build
            assert(Not n Is Nothing)
            assert(Not o Is Nothing)
            assert(n.child_count() = 4)
            Return build(n.child(2),
                         o,
                         Function(ByVal indexstr As String) As Boolean
                             Return raw_variable_name.build(
                                        n.child(0),
                                        Function(ByVal type As String,
                                                 ByVal ps As stream(Of builders.parameter)) As Boolean
                                            scope(Of logic_writer, BUILDER, CODE_GENS, T).
                                                current().
                                                value_target().
                                                with_value(
                                                    type,
                                                    ps.map(Function(ByVal d As builders.parameter) As builders.parameter
                                                               assert(Not d Is Nothing)
                                                               Return d.map_name(
                                                                       Function(ByVal name As String) As String
                                                                           Return variable.name_of(name, indexstr)
                                                                       End Function)
                                                           End Function))
                                            Return True
                                        End Function,
                                        Function(ByVal type As String, ByVal source As String) As Boolean
                                            scope(Of logic_writer, BUILDER, CODE_GENS, T).
                                                current().
                                                value_target().
                                                with_value(
                                                    type, variable.name_of(source, indexstr))
                                            Return True
                                        End Function,
                                        o)
                         End Function)
        End Function
    End Class

    Private NotInheritable Class heap_name
        Inherits heap_name(Of code_builder_proxy, code_gens_proxy, scope)
    End Class
End Class
