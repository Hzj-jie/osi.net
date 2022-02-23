
Imports osi.service.cache
Imports osi.root.utils
Imports osi.root.connector

Friend Class cached_filter
    Implements ifilter

    Private ReadOnly f As ifilter
    Private ReadOnly c As icache(Of String, Boolean)

    Public Sub New(ByVal cache_size As Int64, ByVal inner As ifilter)
        assert(inner IsNot Nothing)
        c = New hash_cache(Of String, Boolean)(cache_size)
        f = inner
    End Sub

    Public Sub New(ByVal inner As ifilter)
        Me.New(1024, inner)
    End Sub

    Public Function match(ByVal i As String) As Boolean Implements ifilter.match
        Dim r As Boolean = False
        If Not c.get(i, r) Then
            r = f.match(i)
            c.set(i, r)
        End If
        Return r
    End Function
End Class
