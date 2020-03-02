
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_dec
    Private ReadOnly d As big_udec
    Private s As Boolean

    Public Sub New()
        d = New big_udec()
        set_positive()
    End Sub

    Public Function signal() As Boolean
        Return s
    End Function

    Public Function not_negative() As Boolean
        Return signal()
    End Function

    Public Function not_positive() As Boolean
        Return Not signal() OrElse is_zero()
    End Function

    Public Function positive() As Boolean
        Return signal() AndAlso Not is_zero()
    End Function

    Public Function negative() As Boolean
        Return Not signal() AndAlso Not is_zero()
    End Function

    Public Sub set_positive()
        set_signal(True)
    End Sub

    Public Sub set_negative()
        set_signal(False)
    End Sub

    Public Sub reverse_signal()
        set_signal(Not positive())
    End Sub

    Public Sub set_zero()
        d.set_zero()
        set_positive()
    End Sub

    Public Function is_zero() As Boolean
        'just make sure there is no logic error in the class,
        'but no matter s is true or false, d.is_zero() can determine whether is_zero()
        Return d.is_zero() AndAlso assert(signal())
    End Function

    Public Sub set_one()
        d.set_one()
        set_positive()
    End Sub

    Public Function is_one() As Boolean
        Return positive() AndAlso d.is_one()
    End Function

    Public Sub set_negative_one()
        d.set_one()
        set_negative()
    End Sub

    Public Function is_negative_one() As Boolean
        Return negative() AndAlso d.is_one()
    End Function

    Public Function is_zero_or_one() As Boolean
        Return is_zero() OrElse is_one()
    End Function

    Public Function is_zero_or_negative_one() As Boolean
        Return is_zero() OrElse is_negative_one()
    End Function

    Public Function is_one_or_negative_one() As Boolean
        Return is_one() OrElse is_negative_one()
    End Function

    Public Function is_zero_or_one_or_negative_one() As Boolean
        Return is_zero() OrElse is_one() OrElse is_negative_one()
    End Function

    Public Function [true]() As Boolean
        Return Not [false]()
    End Function

    Public Function [false]() As Boolean
        Return is_zero()
    End Function
End Class
