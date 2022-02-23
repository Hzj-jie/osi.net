
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports SysArray = System.Array

Partial Public NotInheritable Class percentile
    Public NotInheritable Class ranking_samples(Of T)
        Private ReadOnly samples() As T
        Private cmp As Func(Of T, T, Int32)

        Public Sub New(ByVal v As vector(Of T), ByVal sample_count As UInt32, ByVal cmp As Func(Of T, T, Int32))
            assert(v IsNot Nothing)
            assert(cmp IsNot Nothing)
            assert(sample_count > 0)
            samples = +(v.stream().collect_by(stream(Of T).collectors.samples(sample_count)))
            SysArray.Sort(samples, icomparer_delegator.of(cmp))
            Me.cmp = cmp
        End Sub

        Default Public ReadOnly Property at(ByVal i As T) As Double
            Get
                Dim index As Int32 = SysArray.BinarySearch(samples, i, icomparer_delegator.of(cmp))
                If index >= 0 Then
                    Return (index + 1) / samples.array_size()
                End If
                Return (Not index) / samples.array_size()
            End Get
        End Property
    End Class

    Partial Public NotInheritable Class ascent
        Public Shared Function ranking_samples(Of T)(ByVal v As vector(Of T),
                                                     ByVal sample_count As UInt32) As ranking_samples(Of T)
            Return New ranking_samples(Of T)(v,
                                             sample_count,
                                             Function(ByVal a As T, ByVal b As T) As Int32
                                                 Return compare(a, b)
                                             End Function)
        End Function

        Public Shared Function ranking_samples(Of T)(ByVal v As vector(Of T)) As ranking_samples(Of T)
            Return ranking_samples(v, 1000)
        End Function

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
        Public Shared Function ranking_samples(Of T)(ByVal v As vector(Of T),
                                                     ByVal sample_count As UInt32) As ranking_samples(Of T)
            Return New ranking_samples(Of T)(v,
                                             sample_count,
                                             Function(ByVal a As T, ByVal b As T) As Int32
                                                 Return compare(b, a)
                                             End Function)
        End Function

        Public Shared Function ranking_samples(Of T)(ByVal v As vector(Of T)) As ranking_samples(Of T)
            Return ranking_samples(v, 1000)
        End Function

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
        Return (New ranking_samples(Of T)(v, sample_count, cmp))(i)
    End Function

    Private Shared Function ranking(Of T)(ByVal v As vector(Of T),
                                          ByVal i As T,
                                          ByVal cmp As Func(Of T, T, Int32)) As Double
        Return ranking(v, i, cmp, 1000)
    End Function
End Class
