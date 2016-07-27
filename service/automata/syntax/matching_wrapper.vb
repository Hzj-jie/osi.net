
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class syntaxer
    Public Class matching_wrapper
        Inherits matching
        Implements IComparable(Of matching_wrapper)

        Private ReadOnly m As matching

        Public Sub New(ByVal m As matching)
            assert(Not m Is Nothing)
            Me.m = m
        End Sub

        Public Sub New(ByVal m As UInt32)
            Me.New(matching_creator.create(m))
        End Sub

        Public Sub New(ByVal ms() As UInt32)
            Me.New(matching_creator.create(ms))
        End Sub

        Public Sub New(ByVal m1 As UInt32, ByVal m2 As UInt32, ByVal ParamArray ms() As UInt32)
            Me.New(matching_creator.create(m1, m2, ms))
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            Return m.match(v, p, parent)
        End Function

        Public Shared Operator +(ByVal this As matching_wrapper) As matching
            Return If(this Is Nothing, Nothing, this.m)
        End Operator

        Public NotOverridable Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of any_matching_group)(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As matching_wrapper) As Int32 _
                                           Implements IComparable(Of matching_wrapper).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c = object_compare_undetermined Then
                assert(Not other Is Nothing)
                Return compare(Me.m, other.m)
            Else
                Return c
            End If
        End Function
    End Class
End Class
