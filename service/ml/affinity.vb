
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public NotInheritable Class affinity
    Private Const default_segment_count As UInt32 = 1000

    Public Shared Function [of](Of T As distribution) _
                               (ByVal l As T, ByVal r As T, ByVal segment_count As UInt32) As Double
        assert(Not l Is Nothing)
        assert(Not r Is Nothing)
        Dim lp As one_of(Of tuple(Of Double, Double), vector(Of Double)) = l.parameter_space()
        Dim rp As one_of(Of tuple(Of Double, Double), vector(Of Double)) = r.parameter_space()
        assert(equal(lp, rp))
        Dim d As Double = 0
        If lp.is_first() Then
            Dim range As tuple(Of Double, Double) = l.significant_range()
            range.union_with(r.significant_range())
            assert(segment_count > 0)
            Dim incremental As Double = (range.second() - range.first()) / segment_count
            assert(incremental > 0)
            Dim i As Double = range.first()
            While i < range.second()
                Dim c As Double = l.range_possibility(i, i + incremental) - r.range_possibility(i, i + incremental)
                d += (c * c)
                i += incremental
            End While
        Else
            Dim i As UInt32 = 0
            While i < lp.second().size()
                Dim c As Double = l.possibility(lp.second()(i)) - r.possibility(rp.second()(i))
                d += (c * c)
                i += uint32_1
            End While
        End If
        Return d
    End Function

    Public Shared Function [of](Of T As distribution)(ByVal l As T, ByVal r As T) As Double
        Return [of](l, r, default_segment_count)
    End Function

    Private Sub New()
    End Sub
End Class
