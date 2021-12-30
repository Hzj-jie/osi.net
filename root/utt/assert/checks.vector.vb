
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.template

Partial Public Class checks(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Public Shared Function [of](Of T)(ByVal v As vector(Of T)) As vector_subject(Of T)
        Return New vector_subject(Of T)(v)
    End Function

    Public NotInheritable Class vector_subject(Of T)
        Inherits T_subject(Of vector(Of T))

        Public Sub New(ByVal v As vector(Of T))
            MyBase.New(v)
        End Sub

        Public Shadows Function equal(ByVal j As vector(Of T), ByVal ParamArray msg() As Object) As Boolean
            Return check(Of IS_TRUE_FUNC).array_equal(+i, +j, msg)
        End Function

        Public Function not_equal(ByVal j As vector(Of T), ByVal ParamArray msg() As Object) As Boolean
            Return check(Of IS_TRUE_FUNC).array_not_equal(+i, +j, msg)
        End Function

        Public Function empty(ByVal ParamArray msg() As Object) As Boolean
            Return check(Of IS_TRUE_FUNC).is_not_null(i, msg) AndAlso
                   check(Of IS_TRUE_FUNC).equal(i.size(), uint32_0, msg, ", vector [", i, "]")
        End Function

        Public Function vector_not_empty(ByVal ParamArray msg() As Object) As Boolean
            Return check(Of IS_TRUE_FUNC).is_not_null(i, msg) AndAlso
                   check(Of IS_TRUE_FUNC).not_equal(i.size(), uint32_0, msg)
        End Function
    End Class
End Class
