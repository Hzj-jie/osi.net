
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    ' TODO: Merge with nlexer
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
            Public ReadOnly result As result

            Public Sub New(ByVal id As UInt32, ByVal result As result)
                assert(Not result Is Nothing)
                Me.id = id
                Me.result = result
            End Sub
        End Class

        Public Function best_match(ByVal v As vector(Of typed_word),
                                   ByVal p As UInt32) As [optional](Of best_match_result)
            Dim max As [optional](Of result) = Nothing
            max = [optional].empty(Of result)()
            Dim max_id As Int32 = 0
            max_id = npos
            For i As Int32 = 0 To array_size_i(ms) - 1
                assert(Not ms(i) Is Nothing)
                Dim r As [optional](Of result) = Nothing
                r = ms(i).match(v, p)
                If (Not r.empty()) AndAlso ((Not max) OrElse (+max).pos < (+r).pos) Then
                    max = r
                    max_id = i
                End If
            Next
            If Not max Then
                assert(max_id = npos)
                Return [optional].empty(Of best_match_result)()
            End If
            assert(max)
            assert(max_id >= 0 AndAlso max_id < array_size_i(ms))
            Return [optional].of(New best_match_result(CUInt(max_id), +max))
        End Function

        Public Overrides Function match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As [optional](Of result)
            If v Is Nothing OrElse v.size() <= p Then
                Return [optional].empty(Of result)
            End If
            Dim r As [optional](Of best_match_result) = Nothing
            r = best_match(v, p)
            If Not r Then
                log_unmatched(v, p, Me)
                Return [optional].empty(Of result)()
            End If
            log_matching(v, p, (+r).result.pos, strcat(ms(CInt((+r).id)), "@", (+r).id))
            Return [optional].of((+r).result)
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
