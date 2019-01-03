
Imports osi.root.connector

Partial Public Class big_int
    Public Function add(ByVal that As big_uint) As big_int
        Return add(share(that))
    End Function

    Public Function add(ByVal that As big_int) As big_int
        If Not that Is Nothing AndAlso Not that.is_zero() Then
            If is_zero() Then
                replace_by(that)
            ElseIf positive() = that.positive() Then
                d.add(that.d)
            Else
                If d.less(that.d) Then
                    Dim t As big_uint = Nothing
                    t = New big_uint(that.d)
                    t.assert_sub(d)
                    assert(big_uint.swap(d, t))
                    set_signal(that.signal())
                Else
                    d.assert_sub(that.d)
                    confirm_signal()
                End If
            End If
        End If
        Return Me
    End Function

    Public Function [sub](ByVal that As big_uint) As big_int
        Return [sub](share(that))
    End Function

    Public Function [sub](ByVal that As big_int) As big_int
        Dim t As big_int = Nothing
        t = share(that)
        If t Is Nothing Then
            Return Me
        Else
            t.reverse_signal()
            Return add(t)
        End If
    End Function

    Public Function multiply(ByVal that As big_uint) As big_int
        Return multiply(share(that))
    End Function

    Public Function multiply(ByVal that As big_int) As big_int
        If Not that Is Nothing Then
            d.multiply(that.d)
            set_signal(positive() = that.positive())
        End If
        Return Me
    End Function

    Public Function divide(ByVal that As big_uint,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        Return divide(share(that), divide_by_zero, remainder)
    End Function

    Public Function divide(ByVal that As big_int,
                           ByRef divide_by_zero As Boolean,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        divide(that, remainder, divide_by_zero)
        Return Me
    End Function

    Public Function divide(ByVal that As big_uint,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        Return divide(share(that), remainder)
    End Function

    Public Function divide(ByVal that As big_int,
                           Optional ByRef remainder As big_int = Nothing) As big_int
        Dim r As Boolean = False
        divide(that, remainder, r)
        If r Then
            Throw divide_by_zero()
        End If
        Return Me
    End Function

    Public Function assert_divide(ByVal that As big_uint,
                                  Optional ByRef remainder As big_int = Nothing) As big_int
        Return assert_divide(share(that), remainder)
    End Function

    Public Function assert_divide(ByVal that As big_int,
                                  Optional ByRef remainder As big_int = Nothing) As big_int
        Dim r As Boolean = False
        divide(that, remainder, r)
        assert(Not r)
        Return Me
    End Function

    Public Function power_2() As big_int
        d.power_2()
        set_positive()
        Return Me
    End Function

    Public Function power(ByVal that As big_uint) As big_int
        Return power(share(that))
    End Function

    Public Function power(ByVal that As big_int) As big_int
        If Not that Is Nothing Then
            If Not is_zero() Then
                If that.not_negative() Then
                    d.power(that.d)
                ElseIf Not is_one_or_negative_one() Then
                    set_zero()
                End If
                If that.d.even() Then
                    set_positive()
                End If
            End If
        End If
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint,
                            ByRef divide_by_zero As Boolean,
                            ByRef imaginary_number As Boolean,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        Return extract(share(that), divide_by_zero, imaginary_number, remainder)
    End Function

    Public Function extract(ByVal that As big_int,
                            ByRef divide_by_zero As Boolean,
                            ByRef imaginary_number As Boolean,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        extract(that, remainder, divide_by_zero, imaginary_number)
        Return Me
    End Function

    Public Function extract(ByVal that As big_uint,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        Return extract(share(that), remainder)
    End Function

    Public Function extract(ByVal that As big_int,
                            Optional ByRef remainder As big_int = Nothing) As big_int
        Dim d As Boolean = False
        Dim i As Boolean = False
        extract(that, remainder, d, i)
        If d Then
            Throw divide_by_zero()
        End If
        If i Then
            Throw imaginary_number()
        End If
        Return Me
    End Function

    Public Function assert_extract(ByVal that As big_uint,
                                   Optional ByRef remainder As big_int = Nothing) As big_int
        Return assert_extract(share(that), remainder)
    End Function

    Public Function assert_extract(ByVal that As big_int,
                                   Optional ByRef remainder As big_int = Nothing) As big_int
        Dim d As Boolean = False
        Dim i As Boolean = False
        extract(that, remainder, d, i)
        assert(Not d)
        assert(Not i)
        Return Me
    End Function
End Class
