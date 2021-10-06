
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

        Private ReadOnly anchors As anchors
        Private ReadOnly types As types
        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly size As String

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ByVal type As String,
                       ByVal size As String)
            assert(Not anchors Is Nothing)
            assert(Not types Is Nothing)
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            assert(Not size.null_or_whitespace())
            Me.anchors = anchors
            Me.types = types
            Me.name = name
            Me.type = type
            Me.size = size
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim type As String = Nothing
            If Not macros.decode(anchors, scope, types, Me.type, type) Then
                Return False
            End If
            Dim size As variable = Nothing
            If Not variable.of_stack(scope, types, Me.size, size) Then
                Return False
            End If
            If Not define.export(anchors, types, name, heaps.ptr_type, scope, o) Then
                Return False
            End If
            Dim v As variable = Nothing
            assert(variable.of_stack(scope, types, name, v))
            If Not scope.define_heap(name, type) Then
                Return False
            End If
            o.emplace_back(instruction_builder.str(command.alloc, v, size))
            Return True
        End Function
    End Class
End Namespace
