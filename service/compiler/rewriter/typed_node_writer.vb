﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.template
Imports osi.service.automata

Namespace rewriters
    Public Class typed_node_writer(Of DEBUG_DUMP As __void(Of String))
        Inherits object_list_writer(Of DEBUG_DUMP)

        Public Overloads Function append(ByVal t As typed_node) As Boolean
            assert(Not t Is Nothing)
            Return append(lazier.of(AddressOf t.input_without_spacing))
        End Function
    End Class

    Public NotInheritable Class typed_node_writer
        Inherits typed_node_writer(Of debug_dump_t)

        Private Shared ReadOnly debug_dump As Boolean = env_bool(env_keys("rewrite", "debug", "dump"))

        Public NotInheritable Class debug_dump_t
            Inherits __void(Of String)

            Public Overrides Sub at(ByRef r As String)
                If debug_dump Then
                    raise_error(error_type.user,
                                "Debug dump of typed_node_writer ",
                                r.Replace(";", ";" + newline.incode()).Replace("}", "}" + newline.incode()))
                End If
            End Sub
        End Class
    End Class
End Namespace

