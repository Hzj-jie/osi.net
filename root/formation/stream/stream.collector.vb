
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector

Partial Public Class stream(Of T)
    Public NotInheritable Class collectors
        Public Shared Function to_str(Of ST)(ByVal sep As ST) As Action(Of StringBuilder, T)
            Return Sub(ByVal s As StringBuilder, ByVal v As T)
                       If Not s.empty() Then
                           s.Append(sep)
                       End If
                       s.Append(v)
                   End Sub
        End Function

        Public Shared Function to_str() As Action(Of StringBuilder, T)
            Return to_str(", ")
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
