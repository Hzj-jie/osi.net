
Imports osi.root.connector
Imports osi.service.dataprovider.constants.trigger_datawatcher

Public Class file_dataprovider(Of T)
    Inherits dataprovider(Of T)

    Protected Sub New(ByVal file As String, ByVal interval_ms As Int64, ByVal l As idataloader(Of T))
        MyBase.New(file_datawatcher.generate(file, interval_ms),
                   empty_datafetcher.instance,
                   New redirect_dataloader(Of T)(l, file))
    End Sub

    Protected Sub New(ByVal file As String, ByVal l As idataloader(Of T))
        Me.New(file, default_interval_ms, l)
    End Sub
End Class

Public Class file_content_dataprovider
    Inherits file_dataprovider(Of String)

    Private Sub New(ByVal file As String, ByVal interval_ms As Int64)
        MyBase.New(file,
                   interval_ms,
                   New filestream_dataloader(Of String)(content_dataloader.instance))
    End Sub

    Public Shared Function generate(ByVal file As String,
                                    Optional ByVal interval_ms As Int64 = default_interval_ms) As idataprovider
        assert_fullpath(file)
        Return collection.generate(strcat("file-content-dataprovider://", file, "?interval_ms=", interval_ms),
                                   Function() New file_content_dataprovider(file, interval_ms))
    End Function
End Class
