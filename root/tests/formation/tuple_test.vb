
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class tuple_test
    Inherits [case]

    Private Shared Function create_case() As Boolean
        For i As Int32 = 0 To 100
            Dim _1 As Int32 = 0
            Dim _2 As Int64 = 0
            Dim _3 As Boolean = False
            Dim _4 As String = Nothing
            Dim _5 As Int16 = 0
            Dim _6 As UInt32 = 0
            Dim _7 As UInt64 = 0
            Dim _8 As UInt16 = 0
            _1 = rnd_int()
            _2 = rnd_int64()
            _3 = rnd_bool()
            _4 = rnd_chars(rnd_int(10, 200))
            _5 = rnd_int16()
            _6 = rnd_uint()
            _7 = rnd_uint64()
            _8 = rnd_uint16()
            Dim t As tuple(Of Int32, Int64, Boolean, String, Int16, UInt32, UInt64, UInt16) = Nothing
            t = make_tuple(_1, _2, _3, _4, _5, _6, _7, _8)
            assertion.equal(t._1(), _1)
            assertion.equal(t._2(), _2)
            assertion.equal(t._3(), _3)
            assertion.equal(t._4(), _4)
            assertion.equal(t._5(), _5)
            assertion.equal(t._6(), _6)
            assertion.equal(t._7(), _7)
            assertion.equal(t._8(), _8)
        Next
        Return True
    End Function

    Private Shared Function random_compare_case() As Boolean
        For i As Int32 = 0 To 100
            Dim _1 As Int32 = 0
            Dim _2 As Int64 = 0
            Dim _3 As Boolean = False
            Dim _4 As String = Nothing
            Dim _5 As Int16 = 0
            Dim _6 As UInt32 = 0
            Dim _7 As UInt64 = 0
            Dim _8 As UInt16 = 0
            _1 = rnd_int()
            _2 = rnd_int64()
            _3 = rnd_bool()
            _4 = rnd_chars(rnd_int(10, 200))
            _5 = rnd_int16()
            _6 = rnd_uint()
            _7 = rnd_uint64()
            _8 = rnd_uint16()
            Dim t = make_tuple(_1, _2, _3, _4, _5, _6, _7, _8)
            assertion.equal(t, t)
            assertion.is_true(t = t)
            assertion.is_false(t <> t)
            Dim t2 = make_tuple(_1, _2, _3, _4, _5, _6, _7, _8)
            assertion.equal(t, t2)
            assertion.is_true(t = t2)
            assertion.is_false(t <> t2)
        Next
        Return True
    End Function

    Private Shared Function predefined_compare_case() As Boolean
        Dim t = make_tuple(1, 1, 1)
        Dim t2 = make_tuple(2, 1, 1)
        assertion.equal(compare(t, t2), compare(1, 2))
        t2 = make_tuple(1, 3, 1)
        assertion.equal(compare(t, t2), compare(1, 3))
        t2 = make_tuple(1, 1, 4)
        assertion.equal(compare(t, t2), compare(1, 4))
        t2 = make_tuple(0, 1, 4)
        assertion.equal(compare(t, t2), compare(1, 0))
        t2 = make_tuple(0, 2, 4)
        assertion.equal(compare(t, t2), compare(1, 0))
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return create_case() AndAlso
               random_compare_case() AndAlso
               predefined_compare_case()
    End Function
End Class
