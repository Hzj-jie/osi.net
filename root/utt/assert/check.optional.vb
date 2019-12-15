
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.template

Partial Public Class check(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Public Shared Function has_value(Of T)(ByVal i As [optional](Of T)) As Boolean
        Return is_not_null(i) AndAlso is_true(i)
    End Function

    Public Shared Function has_value(Of T)(ByVal i As [optional](Of T), ByVal v As T) As Boolean
        Return has_value(Of T)(i) AndAlso equal(+i, v)
    End Function

    Public Shared Function is_empty(Of T)(ByVal i As [optional](Of T)) As Boolean
        Return is_false(i)
    End Function
End Class
