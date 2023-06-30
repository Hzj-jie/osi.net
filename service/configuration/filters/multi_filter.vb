
Imports osi.root.connector
Imports osi.service.configuration.constants.multi_filter

Public Class multi_filter
    Implements ifilter

    Private ReadOnly filters() As ifilter

    Protected Sub New(ByVal ctor As Func(Of String, ifilter), ByVal s As String)
        assert(Not s.null_or_empty())
        Dim ss() As String = Nothing
        ss = s.Split(separators, StringSplitOptions.RemoveEmptyEntries)
        ReDim filters(array_size(ss) - 1)
        For i As Int32 = 0 To array_size(ss) - 1
            filters(i) = ctor(ss(i))
            assert(Not filters(i) Is Nothing)
        Next
    End Sub

    Public Function match(ByVal i As String) As Boolean Implements ifilter.match
        For j As Int32 = 0 To array_size(filters) - 1
            If filters(j).match(i) Then
                Return True
            End If
        Next
        Return False
    End Function
End Class
