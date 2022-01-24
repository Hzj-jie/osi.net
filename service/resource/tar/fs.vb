
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class tar
    Public Interface fs
        Function read(ByVal file As String, ByVal o As MemoryStream) As Boolean
        Function write(ByVal file As String, ByVal i As MemoryStream) As Boolean
        Function exists(ByVal file As String) As Boolean
    End Interface

    Public Class file_fs
        Implements fs

        Private ReadOnly r As Func(Of MemoryStream, String, Boolean)
        Private ReadOnly w As Func(Of MemoryStream, String, Boolean)

        Protected Sub New(ByVal read As Func(Of MemoryStream, String, Boolean),
                          ByVal write As Func(Of MemoryStream, String, Boolean))
            assert(Not read Is Nothing)
            assert(Not write Is Nothing)
            Me.r = read
            Me.w = write
        End Sub

        Public Function exists(ByVal file As String) As Boolean Implements fs.exists
            Return IO.File.Exists(file)
        End Function

        Public Function read(ByVal file As String, ByVal o As MemoryStream) As Boolean Implements fs.read
            If String.IsNullOrEmpty(file) Then
                Return False
            End If
            If Not exists(file) Then
                Return False
            End If
            Return r(o, file)
        End Function

        Public Function write(ByVal file As String, ByVal i As MemoryStream) As Boolean Implements fs.write
            If String.IsNullOrEmpty(file) Then
                Return False
            End If
            Return w(i, file)
        End Function
    End Class

    Public NotInheritable Class default_fs
        Inherits file_fs

        Public Shared ReadOnly instance As default_fs = New default_fs()

        Private Sub New()
            MyBase.New(AddressOf read_from_file, AddressOf dump_to_file)
        End Sub
    End Class

    Public NotInheritable Class zip_writer_fs
        Inherits file_fs

        Public Shared ReadOnly instance As zip_writer_fs = New zip_writer_fs()

        Private Sub New()
            MyBase.New(AddressOf read_from_file, AddressOf zip_to_file)
        End Sub
    End Class

    Public NotInheritable Class zip_reader_fs
        Inherits file_fs

        Public Shared ReadOnly instance As zip_reader_fs = New zip_reader_fs()

        Private Sub New()
            MyBase.New(AddressOf unzip_from_file, AddressOf dump_to_file)
        End Sub
    End Class

    Public NotInheritable Class testing_fs
        Implements fs

        Private Const mem_stream_prefix As String = "mem_stream_"
        ' Use map to keep the order.
        Private ReadOnly m As map(Of String, MemoryStream) = New map(Of String, MemoryStream)()

        Public Function stream_of(ByVal s As String) As MemoryStream
            assert(Not String.IsNullOrEmpty(s))
            Return memory_stream.of(s)
        End Function

        Public Function [with](ByVal v As vector(Of MemoryStream)) As testing_fs
            assert(Not v Is Nothing)
            Dim i As UInt32 = 0
            While i < v.size()
                assert(m.emplace(strcat(mem_stream_prefix, i), v(i)).second())
                i += uint32_1
            End While
            Return Me
        End Function

        Public Function [with](ByVal v As vector(Of String)) As testing_fs
            assert(Not v Is Nothing)
            Dim i As UInt32 = 0
            While i < v.size()
                assert(m.emplace(v(i), memory_stream.of(v(i))).second())
                i += uint32_1
            End While
            Return Me
        End Function

        Public Function [with](ByVal v As vector(Of String),
                               ByVal ParamArray others() As vector(Of String)) As testing_fs
            assert(others.array_size() > 0)
            [with](v)
            For Each other As vector(Of String) In others
                [with](other)
            Next
            Return Me
        End Function

        Public Function [erase](ByVal v As vector(Of MemoryStream)) As testing_fs
            assert(Not v Is Nothing)
            Return [erase](streams.range(0, v.size()).
                                   map(Function(ByVal i As Int32) As String
                                           Return strcat(mem_stream_prefix, i)
                                       End Function).
                                   collect(Of vector(Of String))())
        End Function

        Public Function [erase](ByVal v As vector(Of String)) As testing_fs
            assert(Not v Is Nothing)
            Dim i As UInt32 = 0
            While i < v.size()
                assert(m.erase(v(i)))
                i += uint32_1
            End While
            Return Me
        End Function

        Public Function [erase](ByVal v As vector(Of String),
                                ByVal ParamArray others() As vector(Of String)) As testing_fs
            assert(others.array_size() > 0)
            [erase](v)
            For Each other As vector(Of String) In others
                [erase](other)
            Next
            Return Me
        End Function

        Public Function list_files() As vector(Of String)
            Return m.stream().
                     map(m.first_selector).
                     collect(Of vector(Of String))()
        End Function

        Public Function exists(ByVal file As String) As Boolean Implements fs.exists
            Return m.find(file) <> m.end()
        End Function

        Public Function read(ByVal file As String, ByVal o As MemoryStream) As Boolean Implements fs.read
            assert(Not o Is Nothing)
            Dim it As map(Of String, MemoryStream).iterator = m.find(file)
            If it = m.end() Then
                Return False
            End If
            o.write((+it).second.ToArray())
            Return True
        End Function

        Public Function write(ByVal file As String, ByVal i As MemoryStream) As Boolean Implements fs.write
            ' Do not allow overwriting in tests.
            assert(m.emplace(file, i.clone()).second())
            Return True
        End Function

        Public Function contains(ByVal file As String, ByVal i As MemoryStream) As Boolean
            Dim it As map(Of String, MemoryStream).iterator = m.find(file)
            If it = m.end() Then
                Return False
            End If
            Return (+it).second.content_compare_to(i) = 0
        End Function

        Public Function contains(ByVal file As String) As Boolean
            Return contains(file, memory_stream.of(file))
        End Function
    End Class

    Public NotInheritable Class in_mem_fs
        Implements fs

        Public Const file As String = "in_mem_fs_file"
        Private Const exp_file As String = file + "0"
        Private ReadOnly m As MemoryStream

        Public Sub New(ByVal m As MemoryStream)
            assert(Not m Is Nothing)
            Me.m = m
        End Sub

        Public Function exists(ByVal file As String) As Boolean Implements fs.exists
            assert(Not file Is Nothing)
            Return exp_file.Equals(file)
        End Function

        Public Function read(ByVal file As String, ByVal o As MemoryStream) As Boolean Implements fs.read
            assert(Not file Is Nothing)
            assert(Not o Is Nothing)
            If Not exp_file.Equals(file) Then
                Return False
            End If
            m.WriteTo(o)
            Return True
        End Function

        Public Function write(ByVal file As String, ByVal i As MemoryStream) As Boolean Implements fs.write
            assert(Not file Is Nothing)
            assert(Not i Is Nothing)
            ' Allow writing only once.
            assert(m.Length() = 0)
            If Not exp_file.Equals(file) Then
                Return False
            End If
            i.WriteTo(m)
            Return True
        End Function
    End Class

    Public NotInheritable Class fs_wrapper
        Implements fs

        Private ReadOnly read_fs As fs
        Private ReadOnly write_fs As fs

        Public Sub New(ByVal read_fs As fs, ByVal write_fs As fs)
            assert(Not read_fs Is Nothing)
            assert(Not write_fs Is Nothing)
            Me.read_fs = read_fs
            Me.write_fs = write_fs
        End Sub

        Public Function exists(ByVal file As String) As Boolean Implements fs.exists
            Return read_fs.exists(file)
        End Function

        Public Function read(ByVal file As String, ByVal o As MemoryStream) As Boolean Implements fs.read
            Return read_fs.read(file, o)
        End Function

        Public Function write(ByVal file As String, ByVal i As MemoryStream) As Boolean Implements fs.write
            Return write_fs.write(file, i)
        End Function
    End Class

    Private Sub New()
    End Sub
End Class
