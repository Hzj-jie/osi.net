
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports System.IO
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Public Module _resource
    <Extension()> Public Function sync_export(ByVal b() As Byte,
                                              ByVal file_name As String,
                                              Optional ByVal overwrite As Boolean = False) As Boolean
        Return async_sync(b.export(file_name, overwrite))
    End Function

    <Extension()> Public Function sync_export_exec(ByVal b() As Byte,
                                                   ByVal file_name As String,
                                                   Optional ByVal overwrite As Boolean = False) As Boolean
        Return async_sync(b.export_exec(file_name, overwrite))
    End Function

    <Extension()> Public Function export(ByVal b() As Byte,
                                         ByVal file_name As String,
                                         Optional ByVal overwrite As Boolean = False) As event_comb
        Dim w As FileStream = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If isemptyarray(b) Then
                                      Return False
                                  End If
                                  Try
                                      Directory.GetParent(file_name).Create()
                                  Catch
                                  End Try
                                  Try
                                      w = New FileStream(file_name,
                                                         If(overwrite, FileMode.Create, FileMode.CreateNew),
                                                         FileAccess.Write,
                                                         FileShare.Read,
                                                         4096,
                                                         True)
                                  Catch ex As Exception
                                      raise_error(error_type.warning,
                                                  "failed to export resource to local file ",
                                                  file_name,
                                                  ", ex ",
                                                  ex.Message())
                                      Return False
                                  End Try
                                  ec = w.send(b, 0, array_size(b), close_when_finish:=True)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function export_exec(ByVal b() As Byte,
                                             ByVal file_name As String,
                                             Optional ByVal overwrite As Boolean = False) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = export(b, file_name, overwrite)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         (Not envs.mono OrElse chmod_exe(file_name)) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module
