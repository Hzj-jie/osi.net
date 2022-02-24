
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public Class stream(Of T)
    Private Structure compare_node
        Implements IComparable(Of compare_node)

        Public ReadOnly v As T
        Private ReadOnly cmp As Func(Of T, T, Int32)

        Public Sub New(ByVal v As T, ByVal cmp As Func(Of T, T, Int32))
            assert(Not cmp Is Nothing)
            Me.v = v
            Me.cmp = cmp
        End Sub

        Public Function CompareTo(ByVal other As compare_node) As Int32 _
                Implements IComparable(Of compare_node).CompareTo
            Return cmp(v, other.v)
        End Function
    End Structure
End Class
