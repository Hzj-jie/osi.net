
Option Explicit On
Option Infer Off
Option Strict On

Public Class comparison_case_wrapper
    Inherits chained_case_wrapper

    Public Sub New(ByVal continue_when_failure As Boolean,
                   ByVal ParamArray cs() As [case])
        MyBase.New(continue_when_failure, cs)
        AddHandler finished, AddressOf after_finish
    End Sub

    Public Sub New(ByVal ParamArray cs() As [case])
        MyBase.New(cs)
        AddHandler finished, AddressOf after_finish
    End Sub

    Protected Overridable Sub compare()
    End Sub

    Private Sub after_finish(ByRef r As Boolean)
        compare()
    End Sub
End Class
