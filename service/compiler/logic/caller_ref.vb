﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class _caller_ref
        Implements exportable

        Private ReadOnly name As String
        Private ReadOnly result As [optional](Of String)
        Private ReadOnly parameters() As String

        Public Sub New(ByVal name As String,
                       ByVal result As String,
                       ByVal parameters As vector(Of String))
            Me.New(name, result, +parameters)
        End Sub

        Public Sub New(ByVal name As String,
                       ByVal result As String,
                       ParamArray ByVal parameters() As String)
            assert(Not name.null_or_whitespace())
            assert(result Is Nothing OrElse Not result.null_or_whitespace())
            Me.name = name
            Me.result = [optional].of_nullable(result)
            Me.parameters = parameters
            For Each parameter As String In Me.parameters
                assert(Not parameter.null_or_whitespace())
            Next
        End Sub

        Public Sub New(ByVal name As String, ByVal parameters As vector(Of String))
            Me.New(name, +parameters)
        End Sub

        Public Sub New(ByVal name As String, ParamArray ByVal parameters() As String)
            Me.New(name, Nothing, parameters)
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(False)
        End Function
    End Class
End Namespace
