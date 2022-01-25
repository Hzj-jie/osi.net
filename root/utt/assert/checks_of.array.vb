
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Partial Public Class checks(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Public Shared Function [of](Of T)(ByVal i() As T) As array_subject(Of T)
        Return New array_subject(Of T)(i)
    End Function

    Public NotInheritable Class array_subject(Of T)
        Inherits T_subject(Of T())

        Public Sub New(ByVal i() As T)
            MyBase.New(i)
        End Sub

        Public Shadows Function equal(ByVal ParamArray j() As T) As Boolean
            Return check(Of IS_TRUE_FUNC).array_equal(i, j)
        End Function

        Public Shadows Function not_equal(ByVal ParamArray j() As T) As Boolean
            Return check(Of IS_TRUE_FUNC).array_not_equal(i, j)
        End Function
    End Class
End Class