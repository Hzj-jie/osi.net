
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class object_unique_set_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New object_unique_set_case(), 10000), Environment.ProcessorCount())
    End Sub

    Private Class object_unique_set_case
        Inherits [case]

        Private ReadOnly m As object_unique_set(Of Object)
        Private o() As Object

        Public Sub New()
            m = New object_unique_set(Of Object)()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                ReDim o(rnd_uint(1000, 3000))
                For i As Int32 = 0 To array_size(o) - 1
                    o(i) = New Object()
                Next
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            Dim x As Int32 = 0
            x = rnd_int(0, 3)
            If x = 0 Then
                m.insert(o(rnd_uint(0, array_size(o))))
            ElseIf x = 1 Then
                m.erase(o(rnd_uint(0, array_size(o))))
            Else
                Dim i As Object = Nothing
                If m.get(rnd_uint(0, m.size()), i) Then
                    Dim j As Int32 = 0
                    For j = 0 To array_size(o) - 1
                        If object_compare(i, o(j)) = 0 Then
                            Exit For
                        End If
                    Next
                    assert_less(CUInt(j), array_size(o))
                End If
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            o = Nothing
            Return MyBase.finish()
        End Function
    End Class
End Class
