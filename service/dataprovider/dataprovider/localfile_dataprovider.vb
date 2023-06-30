
Imports osi.root.connector

Public Class localfile_dataprovider(Of T)
    Inherits dataprovider(Of T)

    Protected Sub New(ByVal file As String, ByVal l As idataloader(Of T))
        MyBase.New(localfile_datawatcher.generate(file),
                   empty_datafetcher.instance,
                   New redirect_dataloader(Of T)(l, file))
    End Sub
End Class

Public Class localfile_content_dataprovider
    Inherits localfile_dataprovider(Of String)

    Private Sub New(ByVal file As String)
        MyBase.New(file, New filestream_dataloader(Of String)(content_dataloader.instance))
    End Sub

    Public Shared Function generate(ByVal file As String) As idataprovider
        assert_fullpath(file)
        Return collection.generate(strcat("localfile-content-dataprovider://", file),
                                   Function() New localfile_content_dataprovider(file))
    End Function
End Class
