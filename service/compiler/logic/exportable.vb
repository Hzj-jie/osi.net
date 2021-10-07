
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Module _exportable
        Private ReadOnly debug_dump As Boolean

        Sub New()
            debug_dump = env_bool(env_keys("logic", "debug", "dump"))
        End Sub

        <Extension()> Public Function import(ByVal e As interpreter.primitive.exportable,
                                             ByVal scope As scope,
                                             ByVal es() As exportable) As Boolean
            assert(Not e Is Nothing)
            If scope Is Nothing Then
                Return False
            End If
            Dim o As New vector(Of String)()
            For i As Int32 = 0 To es.Length() - 1
                If es(i) Is Nothing Then
                    Return False
                End If
                If Not es(i).export(scope, o) Then
                    Return False
                End If
            Next
            Dim r As String = o.str(character.newline)
            If debug_dump Then
                raise_error(error_type.user, "Debug dump of primitive ", character.newline, r)
            End If
            Return assert(e.import(r))
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
