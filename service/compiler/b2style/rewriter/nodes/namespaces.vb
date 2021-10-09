﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata
Imports osi.service.compiler.logic
Imports osi.service.compiler.rewriters
Imports osi.service.constructor

Partial Public NotInheritable Class b2style
    Public NotInheritable Class namespace_
        Inherits rewriter_wrapper
        Implements rewriter

        Private Const namespace_separator As String = "::"
        Private Const namespace_replacer As String = "__"
        Private ReadOnly ws As write_scoped(Of String)

        <inject_constructor>
        Public Sub New(ByVal i As rewriters)
            MyBase.New(i)
            Me.ws = New write_scoped(Of String)()
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(Of namespace_)()
        End Sub

        Private Function format(ByVal i As String) As String
            assert(Not i.null_or_whitespace())
            If i.StartsWith(namespace_separator) Then
                Return i
            End If
            Return strcat(ws.current_or(empty_string), namespace_separator, i)
        End Function

        Public Function bstyle_format(ByVal i As String) As String
            Return format(i).Replace(namespace_separator, namespace_replacer).Substring(namespace_replacer.Length())
        End Function

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() >= 4)
            Using ws.push(format(n.child(1).word().str()))
                For i As UInt32 = 3 To n.child_count() - uint32_2
                    If Not l.of(n.child(i)).build(o) Then
                        Return False
                    End If
                Next
            End Using
            Return True
        End Function
    End Class

    Public NotInheritable Class namespace_content
        Inherits rewriter_wrapper
        Implements rewriter

        <inject_constructor>
        Public Sub New(ByVal i As rewriters)
            MyBase.New(i)
        End Sub

        Public Shared Sub register(ByVal b As rewriters)
            assert(Not b Is Nothing)
            b.register(Of namespace_content)()
        End Sub

        Public Function build(ByVal n As typed_node,
                              ByVal o As typed_node_writer) As Boolean Implements code_gen(Of typed_node_writer).build
            assert(Not n Is Nothing)
            assert(n.child_count() = 1)
            Return l.of(n.child()).build(o)
        End Function
    End Class
End Class