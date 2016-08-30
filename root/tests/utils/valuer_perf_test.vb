
Imports osi.root.utils
Imports osi.root.utt

Public Class valuer_perf_test
    Inherits performance_comparison_case_wrapper

    Public Sub New()
        MyBase.New(R(New valuer_perf_case()), R(New direct_access_perf_case()))
    End Sub

    Protected Overrides Function min_rate_table() As Double(,)
        Return {{0, 4000},
                {0.001, 0}}
    End Function

    Private Shared Function R(ByVal i As [case]) As [case]
        Return repeat(i, 1000000)
    End Function

    Private Class C
        Public b As Boolean
        Public i As Int32
        Public s As String
    End Class

    Private Class valuer_perf_case
        Inherits [case]

        Private ReadOnly i As C
        Private ReadOnly vb As valuer(Of Boolean)
        Private ReadOnly vi As valuer(Of Int32)
        Private ReadOnly vs As valuer(Of String)

        Public Sub New()
            i = New C()
            vb = New valuer(Of Boolean)(i, "b")
            vi = New valuer(Of Int32)(i, "i")
            vs = New valuer(Of String)(i, "s")
        End Sub

        Public Overrides Function run() As Boolean
            vb.get()
            vb.set(False)
            vi.get()
            vi.set(100)
            vs.get()
            vs.set("abc")
            Return True
        End Function
    End Class

    Private Class direct_access_perf_case
        Inherits [case]

        Private ReadOnly i As C

        Public Sub New()
            i = New C()
        End Sub

        Public Overrides Function run() As Boolean
            Dim bb As Boolean = False
            bb = i.b
            i.b = False
            Dim ii As Int32 = 0
            ii = i.i
            i.i = 100
            Dim ss As String = Nothing
            ss = i.s
            i.s = "abc"
            Return True
        End Function
    End Class
End Class
