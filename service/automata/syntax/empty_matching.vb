
Imports osi.root.connector
Imports osi.root.formation

Partial Public Class syntaxer
    Public Class empty_matching
        Inherits matching
        Implements IComparable(Of empty_matching)

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            Return Not v Is Nothing AndAlso v.size() > p
        End Function

        Public NotOverridable Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of empty_matching)(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As empty_matching) As Int32 _
                                           Implements IComparable(Of empty_matching).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c = object_compare_undetermined Then
                ' All empty-matching should be the same
                Return 0
            Else
                Return c
            End If
        End Function
    End Class
End Class
