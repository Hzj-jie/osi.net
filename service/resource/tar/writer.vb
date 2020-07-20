
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class tar
    Public NotInheritable Class writer
        Private ReadOnly max_size As UInt32
        Private ReadOnly read As Func(Of String, MemoryStream, Boolean)
        Private ReadOnly write As Func(Of UInt32, MemoryStream, Boolean)
        Private ReadOnly v As vector(Of String)

        Public Sub New(ByVal max_size As UInt32, ByVal output_base As String, ByVal files As vector(Of String))
            Me.New(max_size, output_base, files, AddressOf _memory_stream.dump_to_file)
        End Sub

        Private Sub New(ByVal max_size As UInt32,
                        ByVal read As Func(Of String, MemoryStream, Boolean),
                        ByVal write As Func(Of UInt32, MemoryStream, Boolean),
                        ByVal files As vector(Of String))
            assert(max_size > 0)
            assert(Not read Is Nothing)
            assert(Not write Is Nothing)
            Me.max_size = max_size
            Me.read = read
            Me.write = write
            Me.v = files
        End Sub

        Private Sub New(ByVal max_size As UInt32,
                        ByVal output_base As String,
                        ByVal files As vector(Of String),
                        ByVal dump As Func(Of MemoryStream, String, Boolean))
            Me.New(max_size,
                   Function(ByVal file As String, ByVal o As MemoryStream) As Boolean
                       assert(Not o Is Nothing)
                       Return o.read_from_file(file)
                   End Function,
                   Function(ByVal index As UInt32, ByVal i As MemoryStream) As Boolean
                       assert(Not i Is Nothing)
                       Return dump(i, strcat(output_base, index))
                   End Function,
                   files)
        End Sub

        Public Shared Function of_testing(ByVal max_size As UInt32,
                                          ByVal files As vector(Of String),
                                          ByVal output As vector(Of MemoryStream)) As writer
            Return New writer(max_size, AddressOf memory_stream_from_string, dump_to_memory_streams(output), files)
        End Function

        Public Shared Function zip(ByVal max_size As UInt32,
                                   ByVal output_base As String,
                                   ByVal files As vector(Of String)) As writer
            Return New writer(max_size, output_base, files, AddressOf _memory_stream.zip_to_file)
        End Function

        Public Function dump() As Boolean
            Dim write_index As UInt32 = 0
            Dim m As MemoryStream = Nothing
            m = New MemoryStream()
            Dim it As vector(Of String).iterator = Nothing
            it = v.begin()
            While it <> v.end()
                Dim c As MemoryStream = Nothing
                c = New MemoryStream()
                If Not read(+it, c) Then
                    Return False
                End If
                c.Position() = 0
                If Not bytes_serializer.append_to(+it, m) OrElse
                   Not bytes_serializer.append_to(c, m) Then
                    Return False
                End If
                If m.Length() >= max_size Then
                    m.Position() = 0
                    If Not write(write_index, m) Then
                        Return False
                    End If
                    write_index += uint32_1
                    m.clear()
                End If
                it += 1
            End While
            Return True
        End Function
    End Class
End Class
