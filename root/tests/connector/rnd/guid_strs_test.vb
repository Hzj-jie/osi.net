
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class guid_strs_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New guid_strs_case(), 128), Environment.ProcessorCount())
    End Sub

    Private Class guid_strs_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim c As UInt32 = 0
            c = rnd_uint(uint32_1, max_uint8)
            Dim s() As String = Nothing
            s = guid_strs(c)
            If assertion.equal(array_size(s), c) Then
                For i As UInt32 = uint32_0 To c - uint32_1
                    assertion.is_not_null(s(i))
                    If i > uint32_0 Then
                        For j As UInt32 = uint32_0 To i - uint32_1
                            assertion.not_equal(s(i), s(j))
                        Next
                    End If
                Next
            End If
            Return True
        End Function
    End Class
End Class
