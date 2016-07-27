
Imports osi.root.connector
Imports osi.root.utils

Partial Public Class big_int
    Public Function left_shift(ByVal size As Int32) As big_int
        d.left_shift(size)
        Return Me
    End Function

    Public Function left_shift(ByVal size As UInt64) As big_int
        d.left_shift(size)
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_int, ByRef overflow As Boolean) As big_int
        If Not size Is Nothing Then
            Dim u As UInt64 = 0
            u = size.as_uint64(overflow)
            If Not overflow Then
                assert(object_compare(left_shift(u), Me) = 0)
            End If
        End If
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_uint, ByRef overflow As Boolean) As big_int
        If Not size Is Nothing Then
            Dim u As UInt64 = 0
            u = size.as_uint64(overflow)
            If Not overflow Then
                assert(object_compare(left_shift(u), Me) = 0)
            End If
        End If
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_int) As big_int
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function left_shift(ByVal size As big_uint) As big_int
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function assert_left_shift(ByVal size As big_int) As big_int
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function

    Public Function assert_left_shift(ByVal size As big_uint) As big_int
        Dim o As Boolean = False
        assert(object_compare(left_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function

    Public Function right_shift(ByVal size As Int32) As big_int
        d.right_shift(size)
        Return Me
    End Function

    Public Function right_shift(ByVal size As UInt64) As big_int
        d.right_shift(size)
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_int, ByRef overflow As Boolean) As big_int
        If Not size Is Nothing Then
            Dim u As UInt64 = 0
            u = size.as_uint64(overflow)
            If Not overflow Then
                assert(object_compare(right_shift(u), Me) = 0)
            End If
        End If
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_uint, ByRef overflow As Boolean) As big_int
        If Not size Is Nothing Then
            Dim u As UInt64 = 0
            u = size.as_uint64(overflow)
            If Not overflow Then
                assert(object_compare(right_shift(u), Me) = 0)
            End If
        End If
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_int) As big_int
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function right_shift(ByVal size As big_uint) As big_int
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        If o Then
            Throw overflow()
        End If
        Return Me
    End Function

    Public Function assert_right_shift(ByVal size As big_int) As big_int
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function

    Public Function assert_right_shift(ByVal size As big_uint) As big_int
        Dim o As Boolean = False
        assert(object_compare(right_shift(size, o), Me) = 0)
        assert(Not o)
        Return Me
    End Function
End Class
