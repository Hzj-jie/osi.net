
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.ml.boolaffinity(Of String)

Namespace boolaffinity
    <test>
    Public NotInheritable Class model_test
        <test>
        <repeat(100)>
        Private Shared Sub load_and_dump()
            Using ms As MemoryStream = New MemoryStream()
                Dim m As model = Nothing
                m = New model()
                For i As Int32 = 0 To 100
                    Dim k As String = Nothing
                    k = guid_str()
                    If m.has(k) Then
                        Continue For
                    End If
                    If rnd_bool() Then
                        m.set(k,
                              thread_random.of_double.larger_than_0_and_less_or_equal_than_1(),
                              thread_random.of_double.larger_or_equal_than_0_and_less_than_1())
                    Else
                        m.set(k,
                              thread_random.of_double.larger_or_equal_than_0_and_less_than_1(),
                              thread_random.of_double.larger_than_0_and_less_or_equal_than_1())
                    End If
                Next
                assertion.is_true(m.dump(ms))
                ms.Position() = 0
                Dim m2 As model = Nothing
                m2 = New model()
                assertion.is_true(m2.load(ms))
                assertion.equal(m, m2)
            End Using
        End Sub

        Private Sub New()
        End Sub
    End Class
End Namespace
