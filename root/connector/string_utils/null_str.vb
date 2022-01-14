
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _null_str
    <Extension()> Public Function or_empty_str(ByVal s As String) As String
        If s Is Nothing Then
            Return ""
        End If
        Return s
    End Function
End Module
