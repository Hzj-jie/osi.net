
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml.onebound

Namespace onebound
    <test>
    Public NotInheritable Class model_test
        <test>
        <repeat(100)>
        Private Shared Sub load_and_dump()
            Using ms As MemoryStream = New MemoryStream()
                Dim m As typed(Of String).model = Nothing
                m = New typed(Of String).model()
                For i As Int32 = 0 To 100
                    Dim a As String = Nothing
                    Dim b As String = Nothing
                    a = guid_str()
                    b = guid_str()
                    If m.possibility(a, b) = 0 Then
                        m.set(a, b, thread_random.of_double.larger_than_0_and_less_or_equal_than_1())
                    End If
                Next
                assertion.is_true(m.dump(ms))
                ms.Position() = 0
                Dim m2 As typed(Of String).model = Nothing
                m2 = New typed(Of String).model()
                assertion.is_true(m2.load(ms))
                assertion.equal(m, m2)
            End Using
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
