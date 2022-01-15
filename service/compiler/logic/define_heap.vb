﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Define an array with @name, @type and @size
    Public NotInheritable Class _define_heap
        Implements exportable

        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly size As String

        Public Sub New(ByVal name As String,
                       ByVal type As String,
                       ByVal size As String)
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            assert(Not size.null_or_whitespace())
            Me.name = name
            Me.type = type
            Me.size = size
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            Dim size As variable = Nothing
            If Not variable.of(Me.size, o, size) Then
                Return False
            End If
            If Not scope.current().variables().define_heap(name, type) Then
                Return False
            End If
            If debug_dump Then
                o.emplace_back(comment_builder.str("+++ define ", name, type))
            End If
            o.emplace_back(command_str(command.push))
            o.emplace_back(instruction_builder.str(command.alloc, "rel0", size))
            Return True
        End Function
    End Class
End Namespace
