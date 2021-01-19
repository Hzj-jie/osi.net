
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation
Imports osi.root.template

Partial Public Class checks(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Public Shared Function [of](Of T)(ByVal i As [optional](Of T)) As optional_subject(Of T)
        Return New optional_subject(Of T)(i)
    End Function

    Public Class optional_subject(Of T)
        Inherits T_subject(Of [optional](Of T))

        Public Sub New(ByVal i As [optional](Of T))
            MyBase.New(i)
        End Sub

        Public Function has_value() As Boolean
            Return check(Of IS_TRUE_FUNC).has_value(i)
        End Function

        Public Function has_value(ByVal v As T) As Boolean
            Return check(Of IS_TRUE_FUNC).has_value(i, v)
        End Function

        Public Function is_empty() As Boolean
            Return check(Of IS_TRUE_FUNC).is_empty(i)
        End Function
    End Class
End Class
