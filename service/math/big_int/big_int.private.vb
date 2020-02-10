
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class big_int
    'should be only to use as temporary value, since the big_uint has not been copied
    'do not call this ctor directly, use shared ctor function
    Private Sub New(ByVal d As big_uint, ByVal s As Boolean)
        Me.New()
        Me.d = d
        Me.s = s
    End Sub

    Private Shared Function share(ByVal i As big_uint) As big_int
        If i Is Nothing Then
            Return Nothing
        Else
            Return New big_int(i, True)
        End If
    End Function

    Private Shared Function share(ByVal i As big_int) As big_int
        If i Is Nothing Then
            Return Nothing
        Else
            Return New big_int(i.d, i.s)
        End If
    End Function

    'the behavior of the following two functions
    'is to make sure when is_zero(), always set the signal as positive
    Private Sub set_signal(ByVal v As Boolean)
        If d.is_zero() Then
            s = True
        Else
            s = v
        End If
        'make sure the logic is correct to set the signal as true when is_zero
        is_zero()
    End Sub

    Private Sub confirm_signal()
        set_signal(signal())
    End Sub

    Private Sub divide(ByVal that As big_int, ByRef remainder As big_int, ByRef divide_by_zero As Boolean)
        If that Is Nothing Then
            Return
        End If
        Dim n As Boolean = False
        n = negative()
        Dim r As big_uint = Nothing
        d.divide(that.d, divide_by_zero, r)
        remainder = share(r)
        confirm_signal()
        If n Then
            remainder.set_negative()
        End If
        If n = that.negative() Then
            set_positive()
        Else
            set_negative()
        End If
    End Sub

    Private Sub extract(ByVal that As big_int,
                        ByRef remainder As big_int,
                        ByRef divide_by_zero As Boolean,
                        ByRef imaginary_number As Boolean)
        If that Is Nothing Then
            Return
        End If
        imaginary_number = (negative() AndAlso that.even())
        remainder = New big_int()
        If Not is_zero_or_one_or_negative_one() Then
            If that.negative() Then
                remainder.replace_by(Me)
                set_zero()
            Else
                Dim r As big_uint = Nothing
                d.extract(that.d, divide_by_zero, r)
                remainder.replace_by(r)
                remainder.set_signal(signal())
            End If
            confirm_signal()
        End If
    End Sub
End Class
