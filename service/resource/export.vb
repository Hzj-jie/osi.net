
Imports System.Runtime.CompilerServices
Imports osi.root.procedure

Public Module _export
    <Extension()> Public Function sync_export(ByVal b() As Byte,
                                              ByVal file_name As String,
                                              Optional ByVal overwrite As Boolean = False) As Boolean
        Return _resource.sync_export(b, file_name, overwrite)
    End Function

    <Extension()> Public Function sync_export_exec(ByVal b() As Byte,
                                                  ByVal file_name As String,
                                                  Optional ByVal overwrite As Boolean = False) As Boolean
        Return _resource.sync_export_exec(b, file_name, overwrite)
    End Function

    <Extension()> Public Function export(ByVal b() As Byte,
                                         ByVal file_name As String,
                                         Optional ByVal overwrite As Boolean = False) As event_comb
        Return _resource.export(b, file_name, overwrite)
    End Function

    <Extension()> Public Function export_exec(ByVal b() As Byte,
                                             ByVal file_name As String,
                                             Optional ByVal overwrite As Boolean = False) As event_comb
        Return _resource.export_exec(b, file_name, overwrite)
    End Function
End Module
