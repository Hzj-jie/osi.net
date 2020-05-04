
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
                Dim t As typed(Of String).trainer = Nothing
                t = New typed(Of String).trainer()
                For i As Int32 = 0 To 100
                    Dim a As String = Nothing
                    a = guid_str()
                    If rnd_bool() Then
                        Dim b As String = Nothing
                        b = guid_str()
                        t.accumulate(a, b, thread_random.of_double.larger_than_0_and_less_or_equal_than_1())
                    Else
                        t.accumulate(a, thread_random.of_double.larger_than_0_and_less_or_equal_than_1())
                    End If
                Next
                Dim m As typed(Of String).model = Nothing
                m = t.dump()
                assertion.is_true(m.dump(ms))
                ms.Position() = 0
                Dim m2 As typed(Of String).model = Nothing
                assertion.is_true(typed(Of String).model.load(ms, m2))
                assertion.equal(m, m2)
            End Using
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
