
Imports osi.root.constants

Namespace primitive
    Public Module _command
        Private ReadOnly str() As String

        Sub New()
            ReDim str(command.COUNT - 1)
            For i As command = uint32_0 To command.COUNT - uint32_1
                str(i) = i.ToString()
            Next
        End Sub

        Public Function command_str(ByVal i As command) As String
            Return str(i)
        End Function
    End Module
End Namespace
