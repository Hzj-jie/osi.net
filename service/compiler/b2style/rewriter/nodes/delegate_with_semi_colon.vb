﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata
Imports typed_node_writer = osi.service.compiler.rewriters.typed_node_writer

Partial Public NotInheritable Class b2style
    Private NotInheritable Class delegate_with_semi_colon
        Implements scope.template_t.name_node, scope.template_t.name

        Private Function name_node_of(ByVal n As typed_node, ByRef o As typed_node) As Boolean _
                                     Implements scope.template_t.name_node.of
            assert(Not n Is Nothing)
            o = n.child(0).child(2)
            Return True
        End Function

        Private Function name_of(ByVal n As typed_node, ByRef o As String) As Boolean _
                                Implements scope.template_t.name.of
            o = template.name_of(scope.template_proxy(Of typed_node_writer, code_builder_proxy, code_gens_proxy).
                                       name_node_of(n),
                                 scope.template_proxy(Of typed_node_writer, code_builder_proxy, code_gens_proxy).
                                       type_param_count(n))
            Return True
        End Function
    End Class
End Class
