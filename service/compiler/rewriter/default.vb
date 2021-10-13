﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata

Partial Public NotInheritable Class rewriters
    Inherits code_gens(Of typed_node_writer)

    Public Class [default]
        Inherits rewriter_wrapper
        Implements rewriter

        Public Sub New(ByVal l As rewriters)
            MyBase.New(l)
        End Sub

        Protected Overridable Function build(ByVal child As typed_node,
                                             ByVal index As UInt32,
                                             ByVal o As typed_node_writer) As Boolean
            assert(Not child Is Nothing)
            assert(Not o Is Nothing)
            Return l.of(child).build(o)
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(Not n.leaf())
            Dim i As UInt32 = 0
            While i < n.child_count()
                If Not build(n.child(i), i, o) Then
                    Return False
                End If
                i += uint32_1
            End While
            Return True
        End Function
    End Class
End Class
