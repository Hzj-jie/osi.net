﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.service.automata

Partial Public NotInheritable Class b2style
    Public NotInheritable Class operations
        Private Const self_prefix As String = "self-"
        Private ReadOnly l As rewriters

        Public Sub New(ByVal l As rewriters)
            assert(Not l Is Nothing)
            Me.l = l
        End Sub

        Private Function function_name(ByVal type_name As String) As String
            assert(Not type_name.null_or_whitespace())
            Return l.code_gen_of(Of kw_namespace)().bstyle_format(strcat("::b2style::", type_name.Replace("-"c, "_"c)))
        End Function

        Public Function function_name(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            Return function_name(n.type_name)
        End Function

        Public Function self_function_name(ByVal n As typed_node) As String
            assert(Not n Is Nothing)
            Dim type_name As String = Nothing
            type_name = n.type_name
            assert(type_name.StartsWith(self_prefix))
            Return function_name(strmid(type_name, strlen(self_prefix)))
        End Function

        Private Sub New()
        End Sub
    End Class
End Class