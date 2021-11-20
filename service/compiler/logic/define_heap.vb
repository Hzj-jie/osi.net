﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Define an array with @name, @type and @size
    Public NotInheritable Class define_heap
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly size As String

        Public Sub New(ByVal types As types,
                       ByVal name As String,
                       ByVal type As String,
                       ByVal size As String)
            assert(Not types Is Nothing)
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            assert(Not size.null_or_whitespace())
            Me.types = types
            Me.name = name
            Me.type = type
            Me.size = size
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            Dim size As variable = Nothing
            If Not variable.of_stack(types, Me.size, size) Then
                Return False
            End If
            If Not define.export(types, name, heaps.ptr_type, o) Then
                Return False
            End If
            Dim v As variable = Nothing
            assert(variable.of_stack(types, name, v))
            If Not scope.current().define_heap(name, type) Then
                Return False
            End If
            o.emplace_back(instruction_builder.str(command.alloc, v, size))
            Return True
        End Function
    End Class
End Namespace
