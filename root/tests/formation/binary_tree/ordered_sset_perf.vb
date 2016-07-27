
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt

Public Class ordered_sset_perf
    Inherits commandline_specific_case_wrapper

    Public Sub New()
        MyBase.New(performance(New ordered_sset_case()))
    End Sub

    'copy from ordered_bset_case
    Private Class ordered_sset_case
        Inherits [case]

        Private Const min As Int32 = min_int16
        Private Const max As Int32 = max_int16 + 1

        Public Overrides Function run() As Boolean
            Dim s As sset(Of Int32) = Nothing
            s = New sset(Of Int32)()
            For i As Int32 = min To max - 1
                Dim r As pair(Of sset(Of Int32).iterator, Boolean) = Nothing
                r = s.insert(i)
                If assert_not_nothing(r) Then
                    assert_true(r.second)
                End If
            Next

            For i As Int32 = min To max - 1
                assert_not_equal(s.find(i), s.end())
            Next

            For i As Int32 = max - 1 To min Step -1
                assert_not_equal(s.find(i), s.end())
            Next

            Return True
        End Function
    End Class
End Class
