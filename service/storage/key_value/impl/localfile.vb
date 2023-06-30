
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.formation

Public Module _localfile
    Private Const buff_size As Int32 = 16 * 1024

    Public Function delete_file(ByVal fp As String) As event_comb
        Dim r As Boolean = False
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     Try
                                                         IO.File.Delete(fp)
                                                         r = True
                                                     Catch ex As Exception
                                                         raise_error(error_type.warning,
                                                                     "failed to delete ",
                                                                     fp,
                                                                     ", ex ",
                                                                     ex.Message())
                                                         r = False
                                                     End Try
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return r AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function set_timestamp(ByVal fp As String,
                                  ByVal ts As Int64,
                                  ByVal result As ref(Of Boolean)) As event_comb
        Dim r As Boolean = False
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     Try
                                                         IO.File.SetLastWriteTimeUtc(fp, ts.to_date())
                                                         r = True
                                                     Catch ex As Exception
                                                         raise_error(error_type.warning,
                                                                     "failed to set last write time of ",
                                                                     fp,
                                                                     " to ",
                                                                     ts.to_date(),
                                                                     ", ex ",
                                                                     ex.Message())
                                                         r = False
                                                     End Try
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return eva(result, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function get_timestamp(ByVal fp As String, ByVal ts As ref(Of Int64)) As event_comb
        Dim r As Boolean = False
        Dim t As Date = Nothing
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     Try
                                                         t = IO.File.GetLastWriteTimeUtc(fp)
                                                         r = True
                                                     Catch ex As Exception
                                                         raise_error(error_type.warning,
                                                                     "failed to get last write time of ",
                                                                     fp,
                                                                     ", ex ",
                                                                     ex.Message())
                                                         r = False
                                                     End Try
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return r AndAlso
                                         eva(ts, t.to_timestamp()) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function list_files(ByVal path As String) As String()
        Try
            Return IO.Directory.GetFiles(path,
                                         "*",
                                         IO.SearchOption.AllDirectories)
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to get files under ",
                        path,
                        ", ex ",
                        ex.Message())
            Return Nothing
        End Try
    End Function

    Public Function list_files(ByVal path As String, ByVal f As ref(Of String())) As event_comb
        Dim fs() As String = Nothing
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     fs = list_files(path)
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return Not fs Is Nothing AndAlso
                                         eva(f, fs) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function file_size(ByVal f As String) As Int64
        If Not IO.File.Exists(f) Then
            Return npos
        End If
        Try
            Return (New IO.FileInfo(f)).Length()
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to get size information of ",
                        f,
                        ", ex ",
                        ex.Message())
            Return npos
        End Try
    End Function

    Public Function file_size(ByVal f As String, ByVal r As ref(Of Int64)) As event_comb
        Dim o As Int64 = 0
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     o = file_size(f)
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return o >= 0 AndAlso
                                         eva(r, o) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function write_file(ByVal file As String,
                               ByVal value() As Byte,
                               ByVal result As ref(Of Boolean),
                               ByVal append As Boolean) As event_comb
        Dim fs As IO.FileStream = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  fs = New IO.FileStream(file,
                                                         If(append, IO.FileMode.Append, IO.FileMode.Create),
                                                         IO.FileAccess.Write,
                                                         IO.FileShare.Read,
                                                         buff_size,
                                                         IO.FileOptions.Asynchronous Or
                                                         IO.FileOptions.WriteThrough)
                                  ec = fs.send(value, 0, array_size(value))
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  fs.Flush()
                                  fs.Close()
                                  fs.Dispose()
                                  Return ec.end_result() AndAlso
                                         eva(result, True) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function read_file(ByVal key As String,
                              ByVal result As ref(Of Byte())) As event_comb
        Dim fs As IO.FileStream = Nothing
        Dim ec As event_comb = Nothing
        Dim r() As Byte = Nothing
        Dim rc As ref(Of UInt32) = Nothing
        Dim exp As UInt32 = 0
        Return New event_comb(Function() As Boolean
                                  fs = New IO.FileStream(key,
                                                         IO.FileMode.Open,
                                                         IO.FileAccess.Read,
                                                         IO.FileShare.Read,
                                                         buff_size,
                                                         IO.FileOptions.Asynchronous Or
                                                         IO.FileOptions.SequentialScan)
                                  exp = CUInt(fs.Length())
                                  If exp = 0 Then
                                      fs.Close()
                                      fs.Dispose()
                                      Return eva(result, r) AndAlso
                                             goto_end()
                                  Else
                                      ReDim r(CInt(exp) - 1)
                                      rc = New ref(Of UInt32)()
                                      ec = fs.receive(r, uint32_0, exp, rc)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  fs.Close()
                                  fs.Dispose()
                                  Return ec.end_result() AndAlso
                                         (+rc) = exp AndAlso
                                         eva(result, r) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function create_directory(ByVal dr As data_dir) As event_comb
        Dim r As Boolean = False
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     r = dr.create_directory()
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return r AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function clear_directory(ByVal dr As data_dir) As event_comb
        Dim r As Boolean = False
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     r = dr.clear()
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return r AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function file_exists(ByVal f As String, ByRef r As Boolean) As Boolean
        Try
            r = IO.File.Exists(f)
            Return True
        Catch ex As Exception
            raise_error(error_type.warning,
                        "failed to get existing information of ",
                        f,
                        ", ex ",
                        ex.Message())
            Return False
        End Try
    End Function

    Public Function file_exists(ByVal f As String, ByVal r As ref(Of Boolean)) As event_comb
        Dim suc As Boolean = False
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     Dim t As Boolean = False
                                                     suc = file_exists(f, t) AndAlso eva(r, t)
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return suc AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function directory_size(ByVal path As String, ByVal result As ref(Of Int64)) As event_comb
        Dim r As Int64 = 0
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     Dim fs() As String = Nothing
                                                     fs = list_files(path)
                                                     For i As Int32 = 0 To array_size_i(fs) - 1
                                                         Dim s As Int64 = 0
                                                         s = file_size(fs(i))
                                                         If s > 0 Then
                                                             r += s
                                                         End If
                                                     Next
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return eva(result, r) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module
