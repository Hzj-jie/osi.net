
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utils
Imports osi.root.template

Friend Class string_pattern_filter(Of case_sensitive As _boolean)
    Inherits string_based_filter(Of case_sensitive)

    Protected Sub New(ByVal s As String)
        MyBase.New(s)
    End Sub

    Protected NotOverridable Overrides Function match(ByVal input As String,
                                                      ByVal base As String,
                                                      ByVal case_sensitive As Boolean) As Boolean
        Return match_pattern(input, base, case_sensitive)
    End Function
End Class

Friend Class string_pattern_filter
    Inherits string_pattern_filter(Of _false)

    Public Sub New(ByVal pattern As String)
        MyBase.New(pattern)
    End Sub
End Class

Friend Class string_case_sensitive_pattern_filter
    Inherits string_pattern_filter(Of _true)

    Public Sub New(ByVal pattern As String)
        MyBase.New(pattern)
    End Sub
End Class