
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class tar
    Public NotInheritable Class writer
        Private ReadOnly fs As fs
        Private ReadOnly max_size As UInt32
        Private ReadOnly file_namer As Func(Of UInt32, String)
        Private ReadOnly v As vector(Of String)

        Public Sub New(ByVal max_size As UInt32, ByVal output_base As String, ByVal files As vector(Of String))
            Me.New(default_fs.instance, max_size, output_base, files)
        End Sub

        Public Sub New(ByVal max_size As UInt32, ByVal output_base As String, ByVal files As selector)
            Me.New(max_size, output_base, assert_which.of(files).is_not_null().relative())
        End Sub

        Private Sub New(ByVal fs As fs,
                        ByVal max_size As UInt32,
                        ByVal file_namer As Func(Of UInt32, String),
                        ByVal files As vector(Of String))
            assert(Not fs Is Nothing)
            assert(max_size > 0)
            assert(Not file_namer Is Nothing)
            assert(Not files Is Nothing)
            Me.fs = fs
            Me.max_size = max_size
            Me.file_namer = file_namer
            Me.v = files
        End Sub

        Private Sub New(ByVal fs As fs,
                        ByVal max_size As UInt32,
                        ByVal output_base As String,
                        ByVal files As vector(Of String))
            Me.New(fs,
                   max_size,
                   Function(ByVal i As UInt32) As String
                       assert(Not String.IsNullOrWhiteSpace(output_base))
                       Return strcat(output_base, i)
                   End Function,
                   files)
        End Sub

        Public Shared Function of_testing(ByVal fs As testing_fs, ByVal max_size As UInt32) As writer
            Return of_testing(fs, max_size, fs.list_files())
        End Function

        Public Shared Function of_testing(ByVal fs As fs,
                                          ByVal max_size As UInt32,
                                          ByVal files As vector(Of String)) As writer
            Return New writer(fs, max_size, "testing_output_", files)
        End Function

        Public Shared Function of_testing(ByVal fs As fs,
                                          ByVal max_size As UInt32,
                                          ByVal files As selector) As writer
            Return of_testing(fs, max_size, assert_which.of(files).is_not_null().relative())
        End Function

        Public Shared Function zip(ByVal max_size As UInt32,
                                   ByVal output_base As String,
                                   ByVal files As vector(Of String)) As writer
            Return New writer(zip_writer_fs.instance, max_size, output_base, files)
        End Function

        Public Shared Function zip(ByVal max_size As UInt32,
                                   ByVal output_base As String,
                                   ByVal files As selector) As writer
            Return zip(max_size, output_base, assert_which.of(files).is_not_null().relative())
        End Function

        Public Shared Function in_mem(ByVal max_size As UInt32,
                                      ByVal files As vector(Of String),
                                      ByVal output As MemoryStream) As writer
            Return New writer(New fs_wrapper(default_fs.instance, New in_mem_fs(output)),
                              max_size,
                              in_mem_fs.file,
                              files)
        End Function

        Public Shared Function in_mem(ByVal max_size As UInt32,
                                      ByVal files As selector,
                                      ByVal output As MemoryStream) As writer
            Return in_mem(max_size, assert_which.of(files).is_not_null().relative(), output)
        End Function

        Private Function dump(ByVal write_index As UInt32) As Boolean
            Dim m As New MemoryStream()
            Dim it As vector(Of String).iterator = v.begin()
            While it <> v.end()
                Dim c As New MemoryStream()
                If Not fs.read(+it, c) Then
                    Return False
                End If
                c.Position() = 0
                If Not bytes_serializer.append_to(+it, m) OrElse
                   Not bytes_serializer.append_to(c, m) Then
                    Return False
                End If
                If m.Length() >= max_size Then
                    If Not fs.write(file_namer(write_index), m) Then
                        Return False
                    End If
                    write_index += uint32_1
                    m.clear()
                End If
                it += 1
            End While
            If Not m.empty() AndAlso Not fs.write(file_namer(write_index), m) Then
                Return False
            End If
            Return True
        End Function

        Public Function dump() As Boolean
            Return dump(0)
        End Function

        Public Function append() As Boolean
            Dim i As UInt32 = 0
            While fs.exists(file_namer(i))
                i += uint32_1
            End While
            Return dump(i)
        End Function
    End Class
End Class
