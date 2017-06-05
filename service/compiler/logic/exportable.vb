
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Module _exportable
        <Extension()> Public Function import(ByVal e As interpreter.primitive.exportable,
                                             ByVal scope As scope,
                                             ByVal es() As exportable) As Boolean
            assert(Not e Is Nothing)
            If scope Is Nothing Then
                Return False
            Else
                Dim o As vector(Of String) = Nothing
                o = New vector(Of String)()
                For i As Int32 = 0 To array_size_i(es) - 1
                    If es(i) Is Nothing Then
                        Return False
                    End If
                    If Not es(i).export(scope, o) Then
                        Return False
                    End If
                Next
                Return debug_assert(e.import(o.ToString(character.newline)))
            End If
        End Function

        <Extension()> Public Function import(ByVal e As interpreter.primitive.exportable,
                                             ByVal es() As exportable) As Boolean
            Return import(e, New scope(), es)
        End Function
    End Module

    Public Interface exportable
        Function export(ByVal scope As scope, ByVal o As vector(Of String)) As Boolean
    End Interface
End Namespace
