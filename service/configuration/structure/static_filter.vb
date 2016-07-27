
Imports osi.root.formation

Friend Class static_filter
    Inherits filter_set

    Private ReadOnly m As map(Of String, String)

    Private Sub New(ByVal fs As filter_selector, ByVal raw_filters As vector(Of pair(Of String, String)))
        MyBase.New(fs, raw_filters)
    End Sub

    Public Overloads Shared Function match(ByVal fs As filter_selector,
                                           ByVal raw_filters As vector(Of pair(Of String, String)),
                                           ByVal variants As vector(Of pair(Of String, String))) As Boolean
        Return (New static_filter(fs, raw_filters)).match(variants)
    End Function
End Class
