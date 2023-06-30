
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Partial Public Class checks(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Public Shared Function [of](Of T)(ByVal i As T) As T_subject(Of T)
        Return New T_subject(Of T)(i)
    End Function

    Public Class T_subject(Of T)
        Protected ReadOnly i As T

        Public Sub New(ByVal i As T)
            Me.i = i
        End Sub

        Public Function between(ByVal min As T, ByVal max As T) As Boolean
            Return check(Of IS_TRUE_FUNC).more_and_less(i, min, max)
        End Function

        Public Function between_include(ByVal min As T, ByVal max As T) As Boolean
            Return check(Of IS_TRUE_FUNC).more_or_equal_and_less_or_equal(i, min, max)
        End Function

        Public Function equal(ByVal other As T) As Boolean
            Return check(Of IS_TRUE_FUNC).equal(i, other)
        End Function

        Public Function not_equal(ByVal other As T) As Boolean
            Return check(Of IS_TRUE_FUNC).not_equal(i, other)
        End Function

        Public Function is_null() As Boolean
            Return check(Of IS_TRUE_FUNC).is_null(i)
        End Function

        Public Function is_not_null() As Boolean
            Return check(Of IS_TRUE_FUNC).is_not_null(i)
        End Function
    End Class
End Class