
Imports osi.root.connector
Imports osi.root.formation

Friend Class filter_set
    Private ReadOnly filters As map(Of String, vector(Of ifilter))

    Public Sub New(ByVal fs As filter_selector, ByVal raw_filters As vector(Of pair(Of String, String)))
        assert(fs IsNot Nothing)
        If raw_filters Is Nothing OrElse raw_filters.size() = 0 Then
            filters = Nothing
        Else
            filters = New map(Of String, vector(Of ifilter))()
            For i As Int32 = 0 To raw_filters.size() - 1
                Dim f As ifilter = Nothing
                f = fs.create(raw_filters(i).first, raw_filters(i).second)
                assert(f IsNot Nothing)
                filters(raw_filters(i).first).push_back(f)
            Next
        End If
    End Sub

    Private Shared Function match(ByVal fs As vector(Of ifilter), ByVal s As String) As Boolean
        assert(fs IsNot Nothing AndAlso fs.size() > 0)
        For i As Int32 = 0 To fs.size() - 1
            assert(fs(i) IsNot Nothing)
            If Not fs(i).match(s) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function match(ByVal variants As vector(Of pair(Of String, String))) As Boolean
        If filters Is Nothing OrElse variants Is Nothing OrElse variants.empty() Then
            Return True
        Else
            For i As Int32 = 0 To variants.size() - 1
                If Not String.IsNullOrEmpty(variants(i).first) Then
                    Dim j As map(Of String, vector(Of ifilter)).iterator = Nothing
                    j = filters.find(variants(i).first)
                    If j <> filters.end() AndAlso
                       assert((+j).second IsNot Nothing) AndAlso
                       Not match((+j).second, variants(i).second) Then
                        Return False
                    End If
                End If
            Next
            Return True
        End If
    End Function
End Class
