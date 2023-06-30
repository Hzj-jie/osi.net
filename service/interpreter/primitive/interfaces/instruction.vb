
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

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
            For i As Int32 = 0 To array_size_i(args) - 1
                s.Append(character.blank).Append(args(i))
            Next
            Return Convert.ToString(s)
        End Function

        Private Sub New()
        End Sub
    End Class

    Public NotInheritable Class comment_builder
        Private Shared ReadOnly comment_start As String

        Shared Sub New()
            Dim v As valuer(Of String) = Nothing
            v = New valuer(Of String)(GetType(_exportable), binding_flags.private, "comment_start")
            comment_start = +v
        End Sub

        Public Shared Function str(ParamArray ByVal args() As Object) As String
            Dim s As StringBuilder = Nothing
            s = New StringBuilder()
            s.Append(comment_start)
            For i As Int32 = 0 To array_size_i(args) - 1
                s.Append(character.blank).Append(args(i))
            Next
            Return Convert.ToString(s)
        End Function

        Private Sub New()
        End Sub
    End Class
End Namespace
