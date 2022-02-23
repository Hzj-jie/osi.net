
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.service.automata

Public Class ifndef_wrapped(Of WRITER As New)
    Implements code_gen(Of WRITER)

    Private ReadOnly l As code_gens(Of WRITER)
    Private ReadOnly is_defined As Func(Of String, Boolean)

    Protected Sub New(ByVal b As code_gens(Of WRITER), ByVal is_defined As Func(Of String, Boolean))
        assert(b IsNot Nothing)
        assert(is_defined IsNot Nothing)
        Me.l = b
        Me.is_defined = is_defined
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As WRITER) As Boolean Implements code_gen(Of WRITER).build
        assert(n IsNot Nothing)
        assert(n.child_count() >= 3)
        If is_defined(n.child(1).word().str()) Then
            Return True
        End If
        For i As UInt32 = 2 To n.child_count() - uint32_2
            If Not l.of(n.child(i)).build(o) Then
                Return False
            End If
        Next
        Return True
    End Function
End Class

Public Class define(Of WRITER As New)
    Implements code_gen(Of WRITER)

    Private ReadOnly define As Action(Of String)

    Protected Sub New(ByVal define As Action(Of String))
        assert(define IsNot Nothing)
        Me.define = define
    End Sub

    Public Function build(ByVal n As typed_node, ByVal o As WRITER) As Boolean Implements code_gen(Of WRITER).build
        assert(n IsNot Nothing)
        assert(n.child_count() = 2)
        define(n.child(1).word().str())
        Return True
    End Function
End Class