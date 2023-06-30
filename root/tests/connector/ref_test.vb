
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.lock

Public NotInheritable Class ref_test
    Inherits gc_memory_measured_case_wrapper

    Public Sub New()
        MyBase.New(New ref_case())
    End Sub

    Protected Overrides Function max_lower_bound() As Int64
        Return ref_case.large_memory_object_size_lowerbound * ref_case.large_memory_object_count_lowerbound
    End Function

    Protected Overrides Function min_upper_bound() As Int64
        Return max(first_sample(), CLng(max_lower_bound() * 1.5 * 1.5))
    End Function

    Protected Overrides Function last_sample_upper_bound() As Int64
        Return min_upper_bound()
    End Function

    Private NotInheritable Class ref_case
        Inherits [case]

        Public Const large_memory_object_count_lowerbound As Int64 = 10000
        Public Const large_memory_object_size_lowerbound As Int64 = 200

        Private Class test_class
            Private Shared finalized As Int64 = 0

            Public Shared Sub clear_finalized()
                atomic.eva(finalized, 0)
            End Sub

            Public Shared Function finalized_count() As Int64
                Return atomic.read(finalized)
            End Function

            Protected Overrides Sub Finalize()
                Interlocked.Increment(finalized)
                MyBase.Finalize()
            End Sub
        End Class

        Private Shared Function hosting_and_release() As Boolean
            test_class.clear_finalized()

            Dim count As Int32 = 0
            count = rnd_int(100, 1000)
            Dim ps() As ref(Of test_class) = Nothing
            ReDim ps(count - 1)
            For i As Int32 = 0 To count - 1
                ps(i) = New ref(Of test_class)(New test_class())
            Next
            assertion.equal(test_class.finalized_count(), 0)
            garbage_collector.repeat_collect()
            assertion.equal(test_class.finalized_count(), 0)

            Dim released As Int32 = 0
            released = rnd_int(0, count)
            For i As Int32 = 0 To released - 1
                ps(i) = Nothing
            Next
            garbage_collector.repeat_collect()
            assertion.equal(test_class.finalized_count(), released)

            For i As Int32 = 0 To count - 1
                ps(i) = Nothing
            Next
            garbage_collector.repeat_collect()
            assertion.equal(test_class.finalized_count(), count)

            Return True
        End Function

        Private Shared Function large_memory() As Boolean
            Using garbage_collector.force_aggressive_collecting()
                'a single character uses two bytes
                Const string_size_lowerbound As Int64 = (large_memory_object_size_lowerbound >> 1)
                Dim count As Int32 = 0
                count = rnd_int(large_memory_object_count_lowerbound, large_memory_object_count_lowerbound * 3)
                Dim ps() As ref(Of String) = Nothing
                ReDim ps(count - 1)
                For i As Int32 = 0 To count - 1
                    ps(i) = New ref(Of String)()
                    ps(i).set(New String(character.h, rnd_int(string_size_lowerbound, string_size_lowerbound * 3)))
                Next
                garbage_collector.repeat_collect()
                ps = Nothing
                garbage_collector.repeat_collect()
                Return True
            End Using
        End Function

        Private NotInheritable Class test_class2
            Implements IComparable(Of test_class2)

            Private ReadOnly s As String = Nothing

            Public Sub New(ByVal s As String)
                Me.s = s
            End Sub

            Public Shared Operator +(ByVal this As test_class2) As String
                Return If(this Is Nothing, Nothing, this.s)
            End Operator

            Public Function CompareTo(ByVal that As test_class2) As Int32 _
                                     Implements IComparable(Of test_class2).CompareTo
                Return compare(s, +that)
            End Function
        End Class

        Private Shared Function compares(ByVal i As ref(Of test_class2),
                                         ByVal s As String,
                                         ByVal exp As Int32) As Boolean
            assert(Not i Is Nothing)
            assertion.equal(i.CompareTo(New test_class2(s)), exp)
            assertion.equal(i.CompareTo(New ref(Of test_class2)(New test_class2(s))), exp)
            assertion.equal(i.CompareTo(DirectCast(New ref(Of test_class2)(New test_class2(s)), Object)), exp)
            Return True
        End Function

        Private Shared Function compares() As Boolean
            Const s1 As String = "ABC"
            Const s2 As String = "abc"
            Dim p As ref(Of test_class2) = Nothing
            p = New ref(Of test_class2)(New test_class2(s1))
            Return compares(p, s1, strcmp(s1, s1)) AndAlso
                   compares(p, s2, strcmp(s1, s2))
        End Function

        Private Shared Function operators() As Boolean
            Dim p1 As ref(Of test_class) = Nothing
            Dim p2 As ref(Of test_class) = Nothing
            Dim t1 As test_class = Nothing
            Dim t2 As test_class = Nothing
            t1 = New test_class()
            t2 = New test_class()
            p1 = New ref(Of test_class)(t1)
            assertion.is_true(p1 = p1)
            assertion.is_false(p1 <> p1)
            assertion.is_true(p1 = t1)
            assertion.is_false(p1 <> t1)
            assertion.is_false(p1 = t2)
            assertion.is_true(p1 <> t2)
            p2 = New ref(Of test_class)(t2)
            assertion.is_false(p1 = p2)
            assertion.is_true(p1 <> p2)
            assertion.is_true(p2 = DirectCast(t2, Object))
            assertion.is_false(p2 <> DirectCast(t2, Object))
            assertion.is_false(p2 = DirectCast(p1, Object))
            assertion.is_true(p2 <> DirectCast(p1, Object))
            Return True
        End Function

        Private Shared Function [overrides]() As Boolean
            Dim p1 As ref(Of test_class) = Nothing
            p1 = New ref(Of test_class)(New test_class())
            assertion.is_true(p1.Equals(p1))
            assertion.is_true(p1.Equals(+p1))
            assertion.is_true(p1.Equals(New ref(Of test_class)(p1)))
            assertion.is_true(p1.Equals(+(New ref(Of test_class)(p1))))
            assertion.is_false(p1.Equals(New test_class()))
            assertion.is_false(p1.Equals(New ref(Of test_class)(New test_class())))

            Const s As String = "ABCDEF"
            Dim p2 As ref(Of String) = Nothing
            p2 = New ref(Of String)(s)
            assertion.equal(p2.GetHashCode(), s.GetHashCode())
            assertion.equal(p2.ToString(), s.ToString())

            Return True
        End Function

        Private Shared Function clone() As Boolean
            Dim p As ref(Of test_class2) = Nothing
            p = New ref(Of test_class2)(New test_class2("abc"))
            Dim q As ref(Of test_class2) = Nothing
            copy(q, p)
            assertion.equal(object_compare(p.get(), q.get()), 0)
            assertion.equal(object_compare(p, q), object_compare_undetermined)
            Return True
        End Function

        Private Shared Function empty() As Boolean
            Dim p As ref(Of UInt32) = Nothing
            p = New ref(Of UInt32)()
            assertion.is_false(p.empty())
            p.set(100)
            assertion.is_false(p.empty())
            p.set(Nothing)
            assertion.is_false(p.empty())

            Dim p2 As ref(Of String) = Nothing
            p2 = New ref(Of String)()
            assertion.is_true(p2.empty())
            p2.set("abc")
            assertion.is_false(p2.empty())
            p2.set("")
            assertion.is_false(p2.empty())
            p2.set(Nothing)
            assertion.is_true(p2.empty())

            Return True
        End Function

        Private Shared Function operator_less_or_great() As Boolean
            Dim p As ref(Of String) = Nothing
            p = New ref(Of String)()
            Dim str As String = Nothing
            str = guid_str()
            assertion.is_true(p < str)
            assertion.equal(+p, str)

            Dim p2 As ref(Of Int32) = Nothing
            p2 = New ref(Of Int32)()
            Dim i As Int32 = 0
            i = rnd_int()
            assertion.is_true(p2 < i)
            assertion.equal(+p2, i)

            assertion.is_false(DirectCast(Nothing, ref(Of Int32)) < 100)
            Return True
        End Function

        Public Overrides Function reserved_processors() As Int16
            Return CShort(Environment.ProcessorCount())
        End Function

        Public Overrides Function run() As Boolean
            Return hosting_and_release() AndAlso
                   large_memory() AndAlso
                   compares() AndAlso
                   operators() AndAlso
                   [overrides]() AndAlso
                   clone() AndAlso
                   operator_less_or_great()
        End Function
    End Class
End Class
