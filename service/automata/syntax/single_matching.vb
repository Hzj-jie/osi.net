
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Partial Public Class syntaxer
    Public Class single_matching
        Inherits matching
        Implements IComparable(Of single_matching)

        Private ReadOnly m As UInt32

        Public Sub New(ByVal m As UInt32)
            Me.m = m
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            If v Is Nothing OrElse v.size() <= p Then
                Return False
            Else
                assert(Not v(p) Is Nothing)
                If v(p).type = m Then
                    add_subnode(v, parent, v(p).type, p, p + uint32_1)
                    p += 1
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        Public NotOverridable Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of single_matching)(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As single_matching) As Int32 _
                                           Implements IComparable(Of single_matching).CompareTo
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
