
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.formation

Friend Class raw_section
    Inherits filtered_raw_value(Of String)

    Private ReadOnly n As String
    Private ReadOnly rf As vector(Of pair(Of String, String))

    Public Sub New(ByVal name As String, ByVal rf As vector(Of pair(Of String, String)))
        assert(Not name.null_or_empty())
        Me.n = name
        Me.rf = rf
    End Sub

    Public Function name() As String
        Return n
    End Function

    Public Function raw_filters() As vector(Of pair(Of String, String))
        Return rf
    End Function
End Class

Public Class section
    Inherits filtered_value(Of String)

    Friend Sub New(ByVal fs As filter_selector, ByVal m As raw_section)
        MyBase.New(fs, m)
    End Sub
End Class
