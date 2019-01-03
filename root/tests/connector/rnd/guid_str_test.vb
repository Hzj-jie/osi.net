
Imports osi.root.connector
Imports osi.root.utt

Friend Class guid_str_case
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim s As String = Nothing
        s = guid_str()
        If assertion.equal(strlen(s), CUInt(32)) Then
            For i As UInt32 = 0 To strlen(s) - 1
                assertion.is_true(s(i).hex())
            Next
        End If
        Return True
    End Function
End Class

Public Class guid_str_test
    Inherits repeat_case_wrapper

    Public Sub New()
        MyBase.New(New guid_str_case(), 1024 * 1024 * 8 * If(isdebugmode(), 1, 4))
    End Sub
End Class

Public Class guid_str_specified_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(multithreading(repeat(New guid_str_case(),
                                         CLng(1024) * 1024 * 1024 * 8 * If(isdebugmode(), 1, 4)),
                                  max(2, Environment.ProcessorCount() >> 1)))
    End Sub
End Class
