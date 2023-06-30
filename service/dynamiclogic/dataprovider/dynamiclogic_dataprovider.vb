
Option Explicit On
Option Infer Off
Option Strict On

Imports System
Imports osi.root.connector
Imports osi.service.dataprovider
Imports osi.service.compiler.dotnet

Public NotInheritable Class dynamiclogic_dataprovider
    Inherits file_dataprovider(Of Func(Of Object(), Object))

    Private Sub New(ByVal file As String, ByVal l As dynamiclogic_dataloader)
        MyBase.New(file, l)
    End Sub

    Public Shared Function generate(ByVal file As String,
                                    ByVal language As source_executor.language,
                                    ByVal type_name As String,
                                    ByVal function_name As String) As dataprovider(Of Func(Of Object(), Object))
        assert_fullpath(file)
        Return direct_cast(Of dataprovider(Of Func(Of Object(), Object))) _
            (collection.generate(name(Of dynamiclogic_dataprovider) _
                                     (file,
                                      parameter("language", language),
                                      parameter("type_name", type_name),
                                      parameter("function_name", function_name)),
                                 Function() New dynamiclogic_dataprovider(
                                     file,
                                     New dynamiclogic_dataloader(language, type_name, function_name))))
    End Function
End Class
