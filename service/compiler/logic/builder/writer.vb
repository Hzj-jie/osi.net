
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class writer
        Private ReadOnly s As StringBuilder
        Private ReadOnly e As vector(Of String)

        Public Sub New()
            s = New StringBuilder()
            e = New vector(Of String)()
        End Sub

        Public Sub append(ByVal ParamArray s() As Object)
            For i As Int32 = 0 To array_size_i(s) - 1
                Me.s.Append(s(i)).Append(character.blank)
            Next
        End Sub

        Public Sub err(ByVal ParamArray s() As Object)
            e.emplace_back(strcat(s))
        End Sub
    End Class
End Namespace
