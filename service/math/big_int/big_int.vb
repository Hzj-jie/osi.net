
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class big_int
    Private ReadOnly d As big_uint
    Private s As Boolean

    Public Sub New()
        d = New big_uint()
        set_positive()
    End Sub

    Public Sub New(ByVal i As Int32)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As UInt32)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As Int64)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As UInt64)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As big_int)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i As big_uint)
        Me.New()
        replace_by(i)
    End Sub

    Public Sub New(ByVal i() As Byte)
        Me.New()
        replace_by(i)
    End Sub

    Public Shared Function move(ByVal i As big_int) As big_int
        If i Is Nothing Then
            Return Nothing
        End If
        Dim s As Boolean = False
        s = i.signal()
        Dim r As big_int = Nothing
        r = New big_int(big_uint.move(i.d), s)
        i.confirm_signal()
        Return r
    End Function

    Public Shared Function swap(ByVal this As big_int, ByVal that As big_int) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        root.connector.swap(this.s, that.s)
        Return assert(big_uint.swap(this.d, that.d))
    End Function

    Public Sub replace_by(ByVal i As Int32)
        If i >= 0 Then
            replace_by(CUInt(i))
        Else
            replace_by(CUInt(-CLng(i)))
            set_negative()
        End If
    End Sub

    Public Sub replace_by(ByVal i As UInt32)
        d.replace_by(i)
        set_positive()
    End Sub

    Public Sub replace_by(ByVal i As Int64)
        If i >= 0 Then
            replace_by(CULng(i))
        Else
            replace_by(CULng(-CDec(i)))
            set_negative()
        End If
    End Sub

    Public Sub replace_by(ByVal i As UInt64)
        d.replace_by(i)
        set_positive()
    End Sub

    Public Function replace_by(ByVal i As big_int) As Boolean
        If i Is Nothing Then
            Return False
        End If
        assert(d.replace_by(i.d))
        set_signal(i.signal())
        Return True
    End Function

    Public Function replace_by(ByVal i As big_uint) As Boolean
        If d.replace_by(i) Then
            set_positive()
            Return True
        End If
        Return False
    End Function

    Public Sub replace_by(ByVal i As Boolean)
        d.replace_by(i)
        set_signal(Not i)
    End Sub

    ' treat as positive, otherwise, we will need to revert and +1
    Public Function replace_by(ByVal a() As Byte) As Boolean
        If d.replace_by(a) Then
            set_positive()
            Return True
        End If
        Return False
    End Function

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
