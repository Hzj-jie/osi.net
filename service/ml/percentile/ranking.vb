
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports SysArray = System.Array

Partial Public NotInheritable Class percentile
    Partial Public NotInheritable Class ascent
        Public Shared Function ranking(Of T)(ByVal v As vector(Of T),
                                             ByVal i As T,
                                             ByVal sample_count As UInt32) As Double
            Return percentile.ranking(v,
                                      i,
                                      Function(ByVal a As T, ByVal b As T) As Int32
                                          Return compare(a, b)
                                      End Function,
                                      sample_count)
        End Function

        Public Shared Function ranking(Of T)(ByVal v As vector(Of T), ByVal i As T) As Double
            Return percentile.ranking(v,
                                      i,
                                      Function(ByVal a As T, ByVal b As T) As Int32
                                          Return compare(a, b)
                                      End Function)
        End Function
    End Class

    Partial Public NotInheritable Class descent
        Public Shared Function ranking(Of T)(ByVal v As vector(Of T),
                                             ByVal i As T,
                                             ByVal sample_count As UInt32) As Double
            Return percentile.ranking(v,
                                      i,
                                      Function(ByVal a As T, ByVal b As T) As Int32
                                          Return compare(b, a)
                                      End Function,
                                      sample_count)
        End Function

        Public Shared Function ranking(Of T)(ByVal v As vector(Of T), ByVal i As T) As Double
            Return percentile.ranking(v,
                                      i,
                                      Function(ByVal a As T, ByVal b As T) As Int32
                                          Return compare(b, a)
                                      End Function)
        End Function
    End Class

    Private Shared Function ranking(Of T)(ByVal v As vector(Of T),
                                          ByVal i As T,
                                          ByVal cmp As Func(Of T, T, Int32),
                                          ByVal sample_count As UInt32) As Double
        assert(Not v Is Nothing)
        assert(Not cmp Is Nothing)
        assert(sample_count > 0)
        Dim samples() As T = +(v.stream().collect_by(stream(Of T).collectors.samples(sample_count)))
        SysArray.Sort(samples, icomparer_delegator.of(cmp))
        Dim index As Int32 = SysArray.BinarySearch(samples, i, icomparer_delegator.of(cmp))
        If index >= 0 Then
            Return (index + 1) / samples.array_size()
        End If
        Return (Not index) / samples.array_size()
    End Function

    Private Shared Function ranking(Of T)(ByVal v As vector(Of T),
                                          ByVal i As T,
                                          ByVal cmp As Func(Of T, T, Int32)) As Double
        Return ranking(v, i, cmp, 1000)
    End Function
End Class
