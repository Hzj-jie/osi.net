
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Public NotInheritable Class writer
    Private ReadOnly s As StringBuilder
    Private ReadOnly e As vector(Of String)

    Public Sub New()
        s = New StringBuilder()
        e = New vector(Of String)()
    End Sub

    Public Function append(ByVal ParamArray s() As String) As writer
        For i As Int32 = 0 To array_Size_i(s) - 1
            Me.s.Append(s(i)).Append(character.blank)
        Next
        Return Me
    End Function

    Public Function err(ByVal ParamArray s() As Object) As writer
        e.emplace_back(strcat(s))
        Return Me
    End Function
End Class
