﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Define a variable with @name.
    Public NotInheritable Class define
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly types As types
        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly with_comment As Boolean

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ByVal type As String)
            Me.New(anchors, types, name, type, False)
        End Sub

        Private Sub New(ByVal anchors As anchors,
                        ByVal types As types,
                        ByVal name As String,
                        ByVal type As String,
                        ByVal with_comment As Boolean)
            assert(Not anchors Is Nothing)
            assert(Not types Is Nothing)
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            Me.anchors = anchors
            Me.types = types
            Me.name = name
            Me.type = type
            Me.with_comment = with_comment
        End Sub

        Public Shared Function export(ByVal anchors As anchors,
                                      ByVal types As types,
                                      ByVal name As String,
                                      ByVal type As String,
                                      ByVal scope As scope,
                                      ByVal o As vector(Of String)) As Boolean
            Dim d As define = Nothing
            d = New define(anchors, types, name, type, True)
            Return d.export(scope, o)
        End Function

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim type As String = Nothing
            If Not macros.decode(anchors, scope, types, Me.type, type) Then
                Return False
            End If
            If scope.define(name, type) Then
                Dim s As String = Nothing
                s = command_str(command.push)
                If with_comment Then
                    s = strcat(s, character.tab, comment_builder.str("+++ define ", name, type))
                End If
                o.emplace_back(s)
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace
