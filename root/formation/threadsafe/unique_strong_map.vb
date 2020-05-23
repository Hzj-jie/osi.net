
Option Explicit On
Option Infer Off
Option Strict On

Public Class unique_strong_map(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Inherits unique_map(Of KEY_T, VALUE_T, VALUE_T)

    Public Sub New()
        MyBase.New()
    End Sub

    Protected NotOverridable Overrides Function store_value(ByVal i As VALUE_T,
                                                            ByRef o As VALUE_T) As Boolean
        o = i
        Return True
    End Function

    Protected NotOverridable Overrides Function value_store(ByVal i As VALUE_T) As VALUE_T
        Return i
    End Function
End Class
