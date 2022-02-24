
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Partial Public NotInheritable Class syntaxer
    ' TODO: Merge with nlexer
    Public NotInheritable Class matching_group
        Inherits matching
        Implements IComparable(Of matching_group)

        Private Shared prefer_best_match As argument(Of Boolean)

        Private ReadOnly ms() As matching

        Public Sub New(ByVal c As syntax_collection, ByVal ParamArray ms() As matching)
            MyBase.New(c)
            assert(Not isemptyarray(ms))
            Me.ms = ms
            ' TODO: find a way to sort the matchings to avoid manual sorting in syntaxer rule file.
        End Sub

        Public Sub New(ByVal c As syntax_collection, ByVal ParamArray ms() As UInt32)
            Me.New(c, matching_creator.create_matchings(c, ms))
        End Sub

        Private NotInheritable Class best_match_result
            Public ReadOnly id As UInt32
            Public ReadOnly result As result

            Public Sub New(ByVal id As UInt32, ByVal result As result)
                Me.id = id
                Me.result = result
            End Sub
        End Class

        Private Function first_match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As best_match_result
            Dim r As result = result.failure(p)
            For i As Int32 = 0 To array_size_i(ms) - 1
                assert(Not ms(i) Is Nothing)
                Dim c As result = ms(i).match(v, p)
                r = r Or c
                If r.succeeded() Then
                    Return New best_match_result(CUInt(i), r)
                End If
            Next
            Return New best_match_result(0, r)
        End Function

        Private Function best_match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As best_match_result
            Dim r As result = result.failure(p)
            Dim best_id As Int32 = -1
            For i As Int32 = 0 To ms.array_size_i() - 1
                assert(Not ms(i) Is Nothing)
                Dim c As result = ms(i).match(v, p)
                If c.succeeded() AndAlso (r.failed() OrElse c.suc.pos > r.suc.pos) Then
                    best_id = i
                End If
                r = r Or c
            Next
            If r.succeeded() Then
                assert(best_id >= 0 AndAlso best_id < ms.array_size_i())
                Return New best_match_result(CUInt(best_id), r)
            End If
            Return New best_match_result(0, r)
        End Function

        Public Overrides Function match(ByVal v As vector(Of typed_word), ByVal p As UInt32) As result
            If v Is Nothing OrElse v.size() <= p Then
                Return result.failure(p)
            End If
            Dim r As best_match_result = If(prefer_best_match Or False, best_match(v, p), first_match(v, p))
            If r.result.failed() Then
                log_unmatched(v, r.result.fal.pos, Me)
            Else
                log_matching(v, p, r.result.suc.pos, strcat(ms(CInt(r.id)), "@", r.id))
            End If
            Return r.result
        End Function

        Public Overrides Function CompareTo(ByVal other As matching) As Int32
            Return CompareTo(cast(Of matching_group).from(other, False))
        End Function

        Public Overloads Function CompareTo(ByVal other As matching_group) As Int32 _
                                           Implements IComparable(Of matching_group).CompareTo
            Dim c As Int32 = object_compare(Me, other)
            If c <> object_compare_undetermined Then
                Return c
            End If
            assert(Not other Is Nothing)
            Return memcmp(Me.ms, other.ms)
        End Function

        Public Overrides Function ToString() As String
            Dim r As New StringBuilder("matching_group [")
            For i As Int32 = 0 To array_size_i(ms) - 1
                assert(Not ms(i) Is Nothing)
                r.Append(ms(i)).Append(",")
            Next
            Return Convert.ToString(r.Append("]"))
        End Function
    End Class
End Class
