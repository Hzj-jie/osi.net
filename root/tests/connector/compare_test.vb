
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

Public Class compare_perf
    Inherits performance_case_wrapper

    Private Class compare_perf_case
        Inherits compare_test

        Public Sub New()
            MyBase.New(1000000, max(Environment.ProcessorCount() >> 1, 1))
        End Sub
    End Class

    Public Sub New()
        MyBase.New(New compare_perf_case())
    End Sub
End Class

Public Class compare_test
    Inherits case_wrapper

    Public Sub New()
        Me.New(20000, 1)
    End Sub

    Private Shared Function chained_case(ByVal repeat_count As Int32) As [case]
        Return chained(repeat(New compare_case(), repeat_count),
                       New not_comparable_case(),
                       New comparer_defined_case())
    End Function

    Protected Sub New(ByVal repeat_count As Int32, ByVal thread_count As Int32)
        MyBase.New(If(thread_count > 1,
                      multithreading(chained_case(repeat_count),
                                     thread_count),
                      chained_case(repeat_count)))
    End Sub

    'to avoid the impact from suppress_compare_error
    Public Overrides Function reserved_processors() As Int16
        Return CShort(Environment.ProcessorCount())
    End Function

    Public Overrides Function run() As Boolean
        Using scoped.atomic_bool(suppress.compare_error)
            Return MyBase.run()
        End Using
    End Function

    Private Class compare_case
        Inherits [case]

        Private Class not_comparable
            Public ReadOnly v As Int32

            Public Sub New(ByVal v As Int32)
                Me.v = v
            End Sub
        End Class

        Private Class comparable_class
            Implements IComparable(Of comparable_class), IComparable(Of not_comparable), IComparable

            Public ReadOnly v As Int32

            Public Sub New(ByVal v As Int32)
                Me.v = v
            End Sub

            Public Function CompareTo(ByVal other As comparable_class) As Int32 _
                Implements IComparable(Of comparable_class).CompareTo
                Return v.CompareTo(other.v)
            End Function

            Public Function CompareTo(ByVal other As not_comparable) As Int32 _
                Implements IComparable(Of not_comparable).CompareTo
                Return v.CompareTo(other.v)
            End Function

            Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
                If TypeOf obj Is comparable_class Then
                    Return CompareTo(cast(Of comparable_class)(obj))
                ElseIf TypeOf obj Is not_comparable Then
                    Return CompareTo(cast(Of not_comparable)(obj))
                Else
                    assertion.is_true(False, "should not call CompareTo in comparable_class with unknown object")
                    Throw New ArgumentException("not comparable")
                    Return rnd_int(min_int32, max_int32)
                End If
            End Function
        End Class

        Private Structure comparable_struct
            Implements IComparable(Of comparable_struct), IComparable(Of not_comparable)

            Public ReadOnly v As Int32

            Public Sub New(ByVal v As Int32)
                Me.v = v
            End Sub

            Public Function CompareTo(ByVal other As comparable_struct) As Int32 _
                Implements IComparable(Of comparable_struct).CompareTo
                Return v.CompareTo(other.v)
            End Function

            Public Function CompareTo(ByVal other As not_comparable) As Int32 _
                Implements IComparable(Of not_comparable).CompareTo
                Return v.CompareTo(other.v)
            End Function
        End Structure

        Private Class comparable
            Implements IComparable

            Public ReadOnly v As Int32

            Public Sub New(ByVal v As Int32)
                Me.v = v
            End Sub

            Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
                Return v.CompareTo(cast(Of comparable)(obj).v)
            End Function
        End Class

        Private Shared Function compare_case(Of T, T2)(ByVal this As T,
                                                       ByVal that As T2,
                                                       ByVal exp As Int32,
                                                       ByVal comparable As Boolean) As Boolean
            assertion.equal(osi.root.connector.comparable(this, that), comparable)
            If comparable Then
                assertion.equal(compare(this, that), exp)
            Else
                assertion.is_false(compare(this, that, 0))
            End If
            Return True
        End Function

        Private Shared Function compare_case(Of T, T2)(ByVal this As T,
                                                       ByVal that As T2,
                                                       ByVal exp As Int32) As Boolean
            Return compare_case(this, that, exp, True)
        End Function

        Private Shared Function not_comparable_case(Of T, T2)(ByVal this As T,
                                                              ByVal that As T2) As Boolean
            Return compare_case(this, that, 0, False)
        End Function

        Private Shared Function string_compare() As Boolean
            Dim s1 As String = Nothing
            Dim s2 As String = Nothing
            s1 = rnd_ascii_display_chars(rnd_int(10, 30))
            s2 = rnd_ascii_display_chars(rnd_int(4, 80))
            Return compare_case(s1, s2, strcmp(s1, s2)) AndAlso
                   compare_case(s1, s1, 0) AndAlso
                   compare_case(s2, s2, 0) AndAlso
                   not_comparable_case(s1, 0) AndAlso
                   not_comparable_case(0, s1)
        End Function

        Private Shared Function int_compare() As Boolean
            Dim i1 As Int32 = 0
            Dim i2 As Int32 = 0
            i1 = rnd_int(min_int32, max_int32)
            i2 = rnd_int(min_int32, max_int32)
            Return compare_case(i1, i2, i1.CompareTo(i2)) AndAlso
                   compare_case(i1, i1, 0) AndAlso
                   compare_case(i2, i2, 0) AndAlso
                   compare_case(i1, Convert.ToInt64(i1), 0) AndAlso
                   compare_case(Convert.ToInt64(i1), i1, 0) AndAlso
                   compare_case(i1, Convert.ToInt64(i2), i1.CompareTo(i2)) AndAlso
                   compare_case(Convert.ToInt64(i1), i2, i1.CompareTo(i2))
        End Function

        Private Shared Function nullable_compare() As Boolean
            Const zero As Int32 = 0
            Dim i1 As Int32 = 0
            Dim i2 As Int32 = 0
            i1 = rnd_int(min_int32, max_int32)
            i2 = rnd_int(min_int32, max_int32)
            Dim n As Int32? = Nothing
            Return compare_case(New Int32?(i1), i2, i1.CompareTo(i2)) AndAlso
                   compare_case(i2, New Int32?(i1), i2.CompareTo(i1)) AndAlso
                   compare_case(New Int32?(i1), i1, 0) AndAlso
                   compare_case(i1, New Int32?(i1), 0) AndAlso
                   compare_case(New Int32?(i2), i2, 0) AndAlso
                   compare_case(i2, New Int32?(i2), 0) AndAlso
                   compare_case(New Int32?(zero), i1, zero.CompareTo(i1)) AndAlso
                   compare_case(i1, New Int32?(zero), i1.CompareTo(zero)) AndAlso
                   compare_case(n, i1, npos) AndAlso
                   compare_case(i1, n, 1) AndAlso
                   not_comparable_case(New Int32?(i1), Convert.ToInt64(i2)) AndAlso
                   not_comparable_case(Convert.ToInt64(i2), New Int32?(i1))
        End Function

        Private Shared Function comparable_compare() As Boolean
            Dim c1 As comparable = Nothing
            Dim c2 As comparable = Nothing
            c1 = New comparable(rnd_int(min_int32, max_int32))
            c2 = New comparable(rnd_int(min_int32, max_int32))
            Return compare_case(c1, c2, c1.CompareTo(c2)) AndAlso
                   compare_case(c1, c2, c1.v.CompareTo(c2.v)) AndAlso
                   compare_case(c1, DirectCast(Nothing, comparable), 1) AndAlso
                   compare_case(c1, c1, 0) AndAlso
                   compare_case(DirectCast(Nothing, comparable), c2, npos) AndAlso
                   compare_case(c2, c2, 0)
        End Function

        Private Shared Function comparable_class_compare() As Boolean
            Dim cc As comparable_class = Nothing
            Dim nc As not_comparable = Nothing
            Dim cc2 As comparable_class = Nothing
            cc = New comparable_class(rnd_int(min_int32, max_int32))
            nc = New not_comparable(rnd_int(min_int32, max_int32))
            cc2 = New comparable_class(rnd_int(min_int32, max_int32))
            Return compare_case(cc, nc, cc.v.CompareTo(nc.v)) AndAlso
                   compare_case(cc, nc, cc.CompareTo(nc)) AndAlso
                   compare_case(nc, cc, comparer.reverse(cc.v.CompareTo(nc.v))) AndAlso
                   compare_case(nc, cc, comparer.reverse(cc.CompareTo(nc))) AndAlso
                   compare_case(cc, cc2, cc.v.CompareTo(cc2.v)) AndAlso
                   compare_case(cc, cc2, cc.CompareTo(cc2)) AndAlso
                   compare_case(cc, cc, 0) AndAlso
                   compare_case(cc, DirectCast(Nothing, comparable_class), 1) AndAlso
                   compare_case(DirectCast(Nothing, comparable_class), cc, npos)
        End Function

        Private Shared Function comparable_struct_compare() As Boolean
            Dim cc As comparable_struct = Nothing
            Dim nc As not_comparable = Nothing
            Dim cc2 As comparable_struct = Nothing
            cc = New comparable_struct(rnd_int(min_int32, max_int32))
            nc = New not_comparable(rnd_int(min_int32, max_int32))
            cc2 = New comparable_struct(rnd_int(min_int32, max_int32))
            'a known issue in .net framework to cause NullReferenceException when DirectCast nothing to a structure
            Dim n As comparable_struct = Nothing

            Return compare_case(cc, nc, cc.v.CompareTo(nc.v)) AndAlso
                   compare_case(cc, nc, cc.CompareTo(nc)) AndAlso
                   compare_case(nc, cc, comparer.reverse(cc.v.CompareTo(nc.v))) AndAlso
                   compare_case(nc, cc, comparer.reverse(cc.CompareTo(nc))) AndAlso
                   compare_case(cc, cc2, cc.v.CompareTo(cc2.v)) AndAlso
                   compare_case(cc, cc2, cc.CompareTo(cc2)) AndAlso
                   compare_case(cc, cc, 0) AndAlso
                   compare_case(cc, n, cc.v.CompareTo(n.v)) AndAlso
                   compare_case(cc, n, comparer.reverse(n.v.CompareTo(cc.v))) AndAlso
                   compare_case(n, cc, comparer.reverse(cc.v.CompareTo(n.v))) AndAlso
                   compare_case(n, cc, n.v.CompareTo(cc.v))
        End Function

        Protected Shared Function not_comparable_compare() As Boolean
            Dim nc1 As not_comparable = Nothing
            Dim nc2 As not_comparable = Nothing
            nc1 = New not_comparable(rnd_int(min_int32, max_int32))
            nc2 = New not_comparable(rnd_int(min_int32, max_int32))
            Return not_comparable_case(nc1, nc2) AndAlso
                   not_comparable_case(nc2, nc1) AndAlso
                   compare_case(nc1, nc1, 0) AndAlso
                   compare_case(nc1, DirectCast(Nothing, not_comparable), 1) AndAlso
                   compare_case(DirectCast(Nothing, not_comparable), nc1, npos)
        End Function

        Private Shared Function compare_as_object() As Boolean
            Dim cc As comparable_class = Nothing
            Dim nc As not_comparable = Nothing
            Dim cc2 As comparable_class = Nothing
            cc = New comparable_class(rnd_int(min_int32, max_int32))
            nc = New not_comparable(rnd_int(min_int32, max_int32))
            cc2 = New comparable_class(rnd_int(min_int32, max_int32))
            Return compare_case(Of Object, Object)(cc, nc, cc.v.CompareTo(nc.v)) AndAlso
                   compare_case(Of Object, Object)(cc, nc, cc.CompareTo(nc)) AndAlso
                   compare_case(Of Object, Object)(nc, cc, comparer.reverse(cc.v.CompareTo(nc.v))) AndAlso
                   compare_case(Of Object, Object)(nc, cc, comparer.reverse(cc.CompareTo(nc))) AndAlso
                   compare_case(Of Object, Object)(cc, cc2, cc.v.CompareTo(cc2.v)) AndAlso
                   compare_case(Of Object, Object)(cc, cc2, cc.CompareTo(cc2)) AndAlso
                   compare_case(Of Object, Object)(cc, cc, 0) AndAlso
                   compare_case(Of Object, Object)(cc, Nothing, 1) AndAlso
                   compare_case(Of Object, Object)(Nothing, cc, npos) AndAlso
 _
                   compare_case(Of comparable_class, Object)(cc, nc, cc.v.CompareTo(nc.v)) AndAlso
                   compare_case(Of comparable_class, Object)(cc, nc, cc.CompareTo(nc)) AndAlso
                   compare_case(Of not_comparable, Object)(nc, cc, comparer.reverse(cc.v.CompareTo(nc.v))) AndAlso
                   compare_case(Of not_comparable, Object)(nc, cc, comparer.reverse(cc.CompareTo(nc))) AndAlso
                   compare_case(Of comparable_class, Object)(cc, cc2, cc.v.CompareTo(cc2.v)) AndAlso
                   compare_case(Of comparable_class, Object)(cc, cc2, cc.CompareTo(cc2)) AndAlso
                   compare_case(Of comparable_class, Object)(cc, cc, 0) AndAlso
                   compare_case(Of comparable_class, Object)(cc, Nothing, 1) AndAlso
                   compare_case(Of comparable_class, Object)(Nothing, cc, npos) AndAlso
                   compare_case(Of not_comparable, Object)(Nothing, cc, npos) AndAlso
 _
                   compare_case(Of Object, not_comparable)(cc, nc, cc.v.CompareTo(nc.v)) AndAlso
                   compare_case(Of Object, not_comparable)(cc, nc, cc.CompareTo(nc)) AndAlso
                   compare_case(Of Object, comparable_class)(nc, cc, comparer.reverse(cc.v.CompareTo(nc.v))) AndAlso
                   compare_case(Of Object, comparable_class)(nc, cc, comparer.reverse(cc.CompareTo(nc))) AndAlso
                   compare_case(Of Object, comparable_class)(cc, cc2, cc.v.CompareTo(cc2.v)) AndAlso
                   compare_case(Of Object, comparable_class)(cc, cc2, cc.CompareTo(cc2)) AndAlso
                   compare_case(Of Object, comparable_class)(cc, cc, 0) AndAlso
                   compare_case(Of Object, comparable_class)(cc, Nothing, 1) AndAlso
                   compare_case(Of Object, not_comparable)(cc, Nothing, 1) AndAlso
                   compare_case(Of Object, comparable_class)(Nothing, cc, npos)
        End Function

        Private Shared Function compare_as_object2() As Boolean
            Dim i As Int32 = 0
            Dim j As Int32 = 0
            i = rnd_int(min_int32, max_int32)
            j = rnd_int(min_int32, max_int32)
            Return compare_case(Of Object, Object)(i, Convert.ToInt64(j), i.CompareTo(j)) AndAlso
                   compare_case(Of Object, Object)(i, j, i.CompareTo(j)) AndAlso
                   compare_case(Of Object, Object)(Convert.ToInt64(i), j, i.CompareTo(j)) AndAlso
 _
                   not_comparable_case(Of Int32, Object)(i, Convert.ToInt64(j)) AndAlso  ' TODO: Make these two workable
                   compare_case(Of Int32, Object)(i, j, i.CompareTo(j)) AndAlso
                   not_comparable_case(Of Int64, Object)(Convert.ToInt64(i), j) AndAlso
 _
                   not_comparable_case(Of Object, Int64)(i, Convert.ToInt64(j)) AndAlso
                   compare_case(Of Object, Int32)(i, j, i.CompareTo(j)) AndAlso
                   not_comparable_case(Of Object, Int32)(Convert.ToInt64(i), j)
        End Function

        Public Overrides Function run() As Boolean
            Return string_compare() AndAlso
                   int_compare() AndAlso
                   comparable_compare() AndAlso
                   comparable_class_compare() AndAlso
                   comparable_struct_compare() AndAlso
                   nullable_compare() AndAlso
                   compare_as_object() AndAlso
                   compare_as_object2()
        End Function
    End Class

    Private Class not_comparable_case
        Inherits compare_case

        Public Overrides Function run() As Boolean
            Return not_comparable_compare()
        End Function
    End Class

    Private Class comparer_defined_case
        Inherits [case]

        Private Class C1
        End Class

        Private Class C2
        End Class

        Private Shared ReadOnly result As Int32

        Shared Sub New()
            result = rnd_int()
            comparer.register(Function(i As C1, j As C2) As Int32
                                  assertion.is_not_null(i)
                                  assertion.is_not_null(j)
                                  Return result
                              End Function)
        End Sub

        Public Overrides Function run() As Boolean
            Dim o As Int32 = 0
            assertion.is_true(compare(Of C1, C2)(Nothing, New C2(), o))
            assertion.equal(o, -1)
            assertion.is_true(compare(New C1(), New C2(), o))
            assertion.equal(o, result)
            assertion.is_true(compare(Of C1, C2)(New C1(), Nothing, o))
            assertion.equal(o, 1)
            Return True
        End Function
    End Class
End Class
