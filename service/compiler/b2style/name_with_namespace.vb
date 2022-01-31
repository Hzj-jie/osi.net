
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class b2style
    Public Structure name_with_namespace
        Implements IComparable,
                   IComparable(Of name_with_namespace),
                   ICloneable,
                   ICloneable(Of name_with_namespace),
                   IEquatable(Of name_with_namespace)

        Private ReadOnly t As tuple(Of String, String)

        <copy_constructor>
        Private Sub New(ByVal t As tuple(Of String, String))
            Me.t = t
        End Sub

        Public Sub New(ByVal name As String)
            assert(Not name.null_or_whitespace())
            t = tuple.of(scope.current().current_namespace().name(), name)
        End Sub

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            Return CompareTo(obj) = 0
        End Function

        Public Function EqualsT(ByVal other As name_with_namespace) As Boolean _
                               Implements IEquatable(Of name_with_namespace).Equals
            Return CompareTo(other) = 0
        End Function

        Public Overrides Function GetHashCode() As Int32
            Return t.GetHashCode()
        End Function

        Public Overrides Function ToString() As String
            Return t.ToString()
        End Function

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of name_with_namespace)().from(obj, False))
        End Function

        Public Function CompareToT(ByVal other As name_with_namespace) As Int32 _
                                  Implements IComparable(Of name_with_namespace).CompareTo
            Return t.CompareTo(other.t)
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return CloneT()
        End Function

        Private Function CloneT() As name_with_namespace Implements ICloneable(Of name_with_namespace).Clone
            Return New name_with_namespace(t.CloneT())
        End Function
    End Structure
End Class
