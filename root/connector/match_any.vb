
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class match_any
    Implements IEquatable(Of match_any), IComparable, IComparable(Of match_any)

    Public Shared ReadOnly instance As match_any = New match_any()

    Private Sub New()
    End Sub

    Public Function CompareTo(ByVal other As match_any) As Int32 Implements IComparable(Of match_any).CompareTo
        assert(object_compare(other, instance) = 0)
        Return 0
    End Function

    Public Function CompareTo(obj As Object) As Int32 Implements IComparable.CompareTo
        If Equals(obj) Then
            Return 0
        End If
        Return -1
    End Function

    Public Overloads Function Equals(ByVal other As match_any) As Boolean Implements IEquatable(Of match_any).Equals
        assert(object_compare(other, instance) = 0)
        Return True
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return object_compare(obj, instance) = 0
    End Function
End Class
