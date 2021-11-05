
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
                                   ByVal p As UInt32) As one_of(Of best_match_result, failure)
            Dim max As best_match_result = Nothing
            Dim max_failure As UInt32 = 0
            For i As Int32 = 0 To array_size_i(ms) - 1
                assert(Not ms(i) Is Nothing)
                Dim r As one_of(Of result, failure) = ms(i).match(v, p)
                If r.is_second() Then
                    If max_failure < r.second().pos Then
                        max_failure = r.second().pos
                    End If
                Else
                    If max Is Nothing OrElse max.result.pos < r.first().pos Then
                        max = New best_match_result(CUInt(i), r.first())
                    End If
                End If
            Next
            If max Is Nothing Then
                Return failure.of(Of best_match_result)(max_failure)
            End If
            Return one_of(Of best_match_result, failure).of_first(max)
        End Function

        Public Overrides Function match(ByVal v As vector(Of typed_word),
                                        ByVal p As UInt32) As one_of(Of result, failure)
            If v Is Nothing OrElse v.size() <= p Then
                Return failure.of(p)
            End If
            Dim r As one_of(Of best_match_result, failure) = best_match(v, p)
            If r.is_second() Then
                log_unmatched(v, r.second().pos, Me)
                Return r.of_second(Of result)()
            End If
            log_matching(v, p, r.first().result.pos, strcat(ms(CInt(r.first().id)), "@", r.first().id))
            Return r.map_first(Function(ByVal x As best_match_result) As result
                                   assert(Not x Is Nothing)
                                   Return x.result
                               End Function)
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
