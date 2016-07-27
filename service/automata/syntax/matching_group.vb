
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class syntaxer
    Public Class matching_group
        Inherits matching
        Implements IComparable(Of matching_group)

        Private ReadOnly ms() As matching

        Public Sub New(ByVal ParamArray ms() As matching)
            assert(Not isemptyarray(ms))
            Me.ms = ms
        End Sub

        Public Sub New(ByVal ParamArray ms() As UInt32)
            Me.New(matching_creator.create_matchings(ms))
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            If v Is Nothing OrElse v.size() <= p Then
                Return False
            Else
                Dim op As UInt32 = 0
                op = p
                For i As UInt32 = 0 To array_size(ms) - uint32_1
                    assert(Not ms(i) Is Nothing)
                    p = op
                    If ms(i).match(v, p, parent) Then
                        Return True
                    End If
                Next
                Return False
            End If
        End Function

        Public NotOverridable Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of matching_group)(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As matching_group) As Int32 _
                                           Implements IComparable(Of matching_group).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c = object_compare_undetermined Then
                assert(Not other Is Nothing)
                Return memcmp(Me.ms, other.ms)
            Else
                Return c
            End If
        End Function
    End Class
End Class
