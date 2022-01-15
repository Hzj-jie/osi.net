
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace primitive
    Public Module _command
        Private ReadOnly str() As String = build_str()

        Private Function build_str() As String()
            Dim str(command.COUNT - 1) As String
            For i As Int32 = 0 To command.COUNT - 1
                ' Note, there is no Convert.ToString([Enum]) overload, so Convert.ToString(i) falls back to
                ' Convert.ToString(UInt32)
                str(i) = enum_def(Of command).from(i).ToString()
            Next
            Return str
        End Function

        Public Function command_str(ByVal i As command) As String
            Return str(enum_def(Of command).to(Of Int32)(i))
        End Function
    End Module
End Namespace
