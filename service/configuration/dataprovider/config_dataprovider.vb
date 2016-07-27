
Imports osi.service.dataprovider
Imports osi.root.connector

Public Class config_dataprovider
    Inherits file_dataprovider(Of config)

    Private Sub New(ByVal file As String, ByVal l As idataloader(Of config))
        MyBase.New(file, l)
    End Sub

    Public Shared Function generate(ByVal file As String,
                                    ByVal l As idataloader(Of config),
                                    ByVal loader_name As String) As dataprovider(Of config)
        assert_fullpath(file)
        Return cast(Of dataprovider(Of config))(
                    collection.generate(strcat("config-dataprovider://", file, "?loader_name=", loader_name),
                                        Function() New config_dataprovider(file, l)))
    End Function
End Class
