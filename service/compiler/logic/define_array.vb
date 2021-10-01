﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    ' Define an array with @name, @type and @size
    Public NotInheritable Class define_array
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
            Return assert(False)
        End Function
    End Class
End Namespace
