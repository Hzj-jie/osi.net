﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.automata
Imports osi.service.compiler.rewriters

Partial Public NotInheritable Class b2style
    Private NotInheritable Class function_call_with_template
        Implements code_gen(Of typed_node_writer)

        Public Function build(ByVal n As typed_node, ByVal o As typed_node_writer) As Boolean _
                Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            Dim extended_type As String = Nothing
            Return scope.current().template().resolve(
                       Function(ByRef name As String) As Boolean
                           Dim t As tuple(Of String, String) = Nothing
                           If function_call.split_struct_function(n.child(0).child(0).input_without_ignored(), t) Then
                               name = t.second()
                               Return True
                           End If
                           Return False
                       End Function,
                       n.child(0),
                       extended_type) AndAlso
                   code_gens().typed(Of function_call).build(_namespace.with_global_namespace(extended_type), n, o)
        End Function
    End Class
End Class
