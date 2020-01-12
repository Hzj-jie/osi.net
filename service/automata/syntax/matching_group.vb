
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
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

        Public NotInheritable Class best_match_result
            Public ReadOnly id As UInt32
            Public ReadOnly pos As UInt32

            Public Sub New(ByVal id As UInt32, ByVal pos As UInt32)
                Me.id = id
                Me.pos = pos
            End Sub
        End Class

        Public Function best_match(ByVal v As vector(Of typed_word),
                                   ByVal p As UInt32) As [optional](Of best_match_result)
            Dim max As [optional](Of UInt32) = Nothing
            max = [optional].empty(Of UInt32)()
            Dim max_id As [optional](Of UInt32) = Nothing
            max_id = [optional].empty(Of UInt32)()
            For i As Int32 = 0 To array_size_i(ms) - 1
                assert(Not ms(i) Is Nothing)
                Dim r As [optional](Of UInt32) = Nothing
                r = ms(i).find_match(v, p)
                If (Not r.empty()) AndAlso ((Not max) OrElse (+max) < (+r)) Then
                    max = r
                    max_id = [optional].of(CUInt(i))
                End If
            Next
            If Not max Then
                assert(Not max_id)
                Return [optional].empty(Of best_match_result)()
            End If
            assert(max)
            assert(max_id)
            Return [optional].of(New best_match_result(+max_id, +max))
        End Function

        Private Function best_match_id(ByVal v As vector(Of typed_word), ByVal p As UInt32) As [optional](Of UInt32)
            Return best_match(v, p).map(Function(ByVal i As best_match_result) As UInt32
                                            assert(Not i Is Nothing)
                                            Return i.id
                                        End Function)
        End Function

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByRef p As UInt32,
                                        ByVal parent As typed_node) As Boolean
            If v Is Nothing OrElse v.size() <= p Then
                Return False
            End If
            Dim i As [optional](Of UInt32) = Nothing
            i = best_match_id(v, p)
            If Not i Then
                log_unmatched(v, p, Me)
                Return False
            End If
            Dim op As UInt32 = 0
            op = p
            assert(ms(CInt(+i)).match(v, p, parent))
            log_matching(v, op, p, strcat(ms(CInt(+i)), "@", +i))
            Return True
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
