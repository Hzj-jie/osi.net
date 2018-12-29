
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

' TODO: Test
Public NotInheritable Class type_bytes_serializer
    Private Shared ReadOnly ss As type_resolver(Of bytes_serializer(Of Object))

    Shared Sub New()
        ss = type_resolver(Of bytes_serializer(Of Object)).default
    End Sub

    Public Shared Function serializer(ByVal type As Type, ByRef o As bytes_serializer(Of Object)) As Boolean
        Return ss.from_type(type, o)
    End Function

    Private Sub New()
    End Sub
End Class