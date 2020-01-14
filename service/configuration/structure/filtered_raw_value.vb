
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Friend Class filtered_raw_value(Of T)
    Private ReadOnly m As map(Of String, vector(Of pair(Of T, vector(Of pair(Of String, String)))))

    Public Sub New()
        m = New map(Of String, vector(Of pair(Of T, vector(Of pair(Of String, String)))))()
    End Sub

    Public Function empty() As Boolean
        Return m.empty()
    End Function

    Public Sub insert(ByVal name As String,
                      ByVal value As T,
                      ByVal filters As vector(Of pair(Of String, String)))
        m(name).emplace_back(pair.emplace_of(value, filters))
    End Sub

    Public Sub foreach(ByVal d As void(Of String, T, vector(Of pair(Of String, String))))
        assert(Not d Is Nothing)
        osi.root.utils.foreach(AddressOf m.foreach,
                               Sub(ByRef k As String,
                                   ByRef v As vector(Of pair(Of T, vector(Of pair(Of String, String)))))
                                   If Not v Is Nothing AndAlso Not v.empty() Then
                                       For i As Int32 = 0 To v.size() - 1
                                           d(k, v(i).first, v(i).second)
                                       Next
                                   End If
                               End Sub)
    End Sub
End Class
