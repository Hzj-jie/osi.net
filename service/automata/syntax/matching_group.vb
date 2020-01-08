
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    Public NotInheritable Class matching_group
        Inherits matching
        Implements IComparable(Of matching_group)

        Private ReadOnly ms() As matching

        Public Sub New(ByVal c As syntax_collection, ByVal ParamArray ms() As matching)
            MyBase.New(c)
            assert(Not isemptyarray(ms))
            Me.ms = ms
        End Sub

        Public Sub New(ByVal c As syntax_collection, ByVal ParamArray ms() As UInt32)
            Me.New(c, matching_creator.create_matchings(c, ms))
        End Sub

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            If v Is Nothing OrElse v.size() <= p Then
                Return False
            End If
            Dim op As UInt32 = 0
            op = p
            For i As Int32 = 0 To array_size_i(ms) - 1
                assert(Not ms(i) Is Nothing)
                p = op
                If ms(i).match(v, p, parent) Then
                    Return True
                End If
            Next
            If syntaxer.debug_log Then
                raise_error(error_type.user, "Failed to match token ", v(op), " when matching group ", Me)
            End If
            Return False
        End Function

        Public Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of matching_group).from(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As matching_group) As Int32 _
                                           Implements IComparable(Of matching_group).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c <> object_compare_undetermined Then
                Return c
            End If
            assert(Not other Is Nothing)
            Return memcmp(Me.ms, other.ms)
        End Function

        Public Overrides Function ToString() As String
            Dim r As StringBuilder = Nothing
            r = New StringBuilder("matching_group [")
            For i As Int32 = 0 To array_size_i(ms) - 1
                assert(Not ms(i) Is Nothing)
                r.Append(ms(i)).Append(",")
            Next
            Return Convert.ToString(r.Append("]"))
        End Function
    End Class
End Class
