
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.envs
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Public Module _instruction_gen
    Public ReadOnly debug_dump As Boolean = env_bool(env_keys("logic", "debug", "dump"))

    ' TODO: Reconsider the way to share interrupts between simulator and scope.
    <Extension()> Public Function import(ByVal e As exportable, ByVal es() As logic.instruction_gen) As Boolean
        Return import(direct_cast(Of simulator)(e), es)
    End Function

    ' @VisibleForTesting
    <Extension()> Public Function import(ByVal e As simulator, ByVal es() As logic.instruction_gen) As Boolean
        ' Do not run the end_scope operations if currently it's in the root scope, the interpreter/primitive will be
        ' freed anyway.
        Dim o As New vector(Of String)()
        Using New logic.scope(e.interrupts()).without_end_scope()
            For i As Int32 = 0 To es.Length() - 1
                If es(i) Is Nothing Then
                    Return False
                End If
                If Not es(i).build(o) Then
                    Return False
                End If
            Next
        End Using
        Dim r As String = o.str(character.newline)
        If debug_dump Then
            raise_error(error_type.user, "Debug dump of primitive ", character.newline, r)
        End If
        Return assert(e.import(r))
    End Function
End Module

Partial Public NotInheritable Class logic
    Public Interface instruction_gen
        Function build(ByVal o As vector(Of String)) As Boolean
    End Interface

    Private Sub New()
    End Sub
End Class
