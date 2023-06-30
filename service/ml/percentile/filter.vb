
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class percentile
    Partial Public NotInheritable Class ascent
        Public Shared Function filter(Of T)(ByVal v As vector(Of tuple(Of T, Double)),
                                            ByVal percentile As Double) As Func(Of tuple(Of T, Double), Boolean)
            Return ml.percentile.filter(v,
                                        percentile,
                                        Function(ByVal i As Double, ByVal j As Double) As Int32
                                            Return i.CompareTo(j)
                                        End Function)
        End Function

        Public Shared Function filter(Of T)(ByVal v As vector(Of tuple(Of T, UInt32)),
                                            ByVal percentile As Double) As Func(Of tuple(Of T, UInt32), Boolean)
            Return ml.percentile.filter(v,
                                        percentile,
                                        Function(ByVal i As UInt32, ByVal j As UInt32) As Int32
                                            Return i.CompareTo(j)
                                        End Function)
        End Function

        Private Sub New()
        End Sub
    End Class

    Partial Public NotInheritable Class descent
        Public Shared Function filter(Of T)(ByVal v As vector(Of tuple(Of T, Double)),
                                            ByVal percentile As Double) As Func(Of tuple(Of T, Double), Boolean)
            Return ml.percentile.filter(v,
                                        percentile,
                                        Function(ByVal i As Double, ByVal j As Double) As Int32
                                            Return j.CompareTo(i)
                                        End Function)
        End Function

        Public Shared Function filter(Of T)(ByVal v As vector(Of tuple(Of T, UInt32)),
                                            ByVal percentile As Double) As Func(Of tuple(Of T, UInt32), Boolean)
            Return ml.percentile.filter(v,
                                        percentile,
                                        Function(ByVal i As UInt32, ByVal j As UInt32) As Int32
                                            Return j.CompareTo(i)
                                        End Function)
        End Function

        Private Sub New()
        End Sub
    End Class

    Private Shared Function filter(Of T, P)(ByVal v As vector(Of tuple(Of T, P)),
                                            ByVal percentile As Double,
                                            ByVal cmp As Func(Of P, P, Int32)) _
                               As Func(Of tuple(Of T, P), Boolean)
        assert(Not cmp Is Nothing)
        If percentile = 0 Then
            Return Function(ByVal i As tuple(Of T, P)) As Boolean
                       Return False
                   End Function
        End If
        If v.size() <= 1 OrElse percentile = 1 Then
            Return Function(ByVal i As tuple(Of T, P)) As Boolean
                       Return True
                   End Function
        End If
        Dim max As P = v.stream().
            map(tuple(Of T, P).second_selector).
            sort(cmp).
            aggregate(stream(Of P).aggregators.percentile(percentile))
        Return Function(ByVal i As tuple(Of T, P)) As Boolean
                   Return cmp(i.second(), max) <= 0
               End Function
    End Function

    Private Sub New()
    End Sub
End Class
