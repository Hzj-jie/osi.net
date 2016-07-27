
Imports osi.root.connector
Imports osi.root.utt
Imports osi.service.interpreter.fullstack.executor

Namespace fullstack.executor
    Public Class location_test
        Inherits [case]

        Private ReadOnly size As Int32

        Public Sub New()
            size = rnd_int(10, 100)
        End Sub

        Public Overrides Function run() As Boolean
            Dim r As domain = Nothing
            r = New domain(New variables())
            For i As Int32 = 0 To size - 1
                r.define(rnd_variable())
            Next
            For i As Int32 = 0 To size - 1
                Dim l As location = Nothing
                l = New location(0, i)
                assert_reference_equal(l(r), r.variable(0, i))
            Next

            Dim d As domain = Nothing
            d = r
            For i As Int32 = 0 To size - 1
                d = New domain(d)
                For j As Int32 = 0 To size - 1
                    d.define(rnd_variable())
                Next
                For j As Int32 = 0 To size - 1
                    Dim l As location = Nothing
                    l = New location(0, j)
                    assert_reference_equal(l(d), d.variable(0, j))
                Next
                For j As Int32 = 0 To size - 1
                    Dim l As location = Nothing
                    l = New location(i + 1, j)
                    assert_reference_equal(l(d), r.variable(0, j))
                Next
                For j As Int32 = 0 To i - 1
                    Dim l As location = Nothing
                    l = New location(j, 0)
                    assert_reference_equal(l(d), d.variable(j, 0))
                Next
            Next
            Return True
        End Function
    End Class
End Namespace
