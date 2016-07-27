﻿
Friend Class string_patterns_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(i) New string_pattern_filter(i), s)
    End Sub
End Class

Friend Class string_care_case_patterns_filter
    Inherits multi_filter

    Public Sub New(ByVal s As String)
        MyBase.New(Function(i) New string_care_case_pattern_filter(i), s)
    End Sub
End Class
