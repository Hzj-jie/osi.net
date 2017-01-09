
Imports System.Text
Imports osi.root.constants
Imports osi.root.connector

Namespace primitive
    Public Interface instruction
        Inherits exportable

        Sub execute(ByVal imi As imitation)
    End Interface

    Public NotInheritable Class instruction_builder
        Public Shared Function str(ByVal command As command, ParamArray ByVal args() As Object) As String
            Dim s As StringBuilder = Nothing
            s = New StringBuilder()
            s.Append(command_str(command))
            For i As Int32 = 0 To array_size(args) - 1
                s.Append(character.blank).Append(args(i))
            Next
            Return Convert.ToString(s)
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
