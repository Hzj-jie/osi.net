
Imports System.IO
Imports osi.service.dataprovider.constants.filestream_dataloader

Public Class filestream_dataloader(Of T)
    Inherits stream_dataloader(Of T)

    Public Sub New(ByVal l As istreamdataloader(Of T))
        MyBase.New(l)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected NotOverridable Overrides Function create(ByVal localfile As String) As Stream
        Return New FileStream(localfile,
                              FileMode.Open,
                              FileAccess.Read,
                              FileShare.Read,
                              buff_size,
                              FileOptions.Asynchronous Or FileOptions.SequentialScan)
    End Function
End Class
