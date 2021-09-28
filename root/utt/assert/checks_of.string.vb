
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.template

Partial Public Class checks(Of IS_TRUE_FUNC As __void(Of Boolean, Object()))
    Public Shared Function [of](ByVal i As String) As string_subject
        Return New string_subject(i)
    End Function

    Public NotInheritable Class string_subject
        Inherits T_subject(Of String)

        Public Sub New(ByVal i As String)
            MyBase.New(i)
        End Sub

        Public Function contains(ByVal ParamArray exps() As String) As Boolean
            Return check(Of IS_TRUE_FUNC).str_contains(i, exps)
        End Function
    End Class
End Class
