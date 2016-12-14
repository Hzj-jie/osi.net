
#If 0 Then
Public NotInheritable Class tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)
    Implements ICloneable, IComparable, IComparable(Of tuple(Of T1, T2, T3, T4, T5, T6, T7, T8))

    Public _1 As T1
    Public _2 As T2
    Public _3 As T3
    Public _4 As T4
    Public _5 As T5
    Public _6 As T6
    Public _7 As T7
    Public _8 As T8

    Private Sub New(ByVal _1 As T1,
                    ByVal _2 As T2,
                    ByVal _3 As T3,
                    ByVal _4 As T4,
                    ByVal _5 As T5,
                    ByVal _6 As T6,
                    ByVal _7 As T7,
                    ByVal _8 As T8)

    End Sub

    Public Sub New()
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Throw New NotImplementedException()
    End Function

    Public Function CompareTo(ByVal other As tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)) As Int32 _
                             Implements IComparable(Of tuple(Of T1, T2, T3, T4, T5, T6, T7, T8)).CompareTo
        Throw New NotImplementedException()
    End Function

    Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Throw New NotImplementedException()
    End Function
End Class
#End If
