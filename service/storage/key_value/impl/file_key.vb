
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.server_services)>
Partial Public Class file_key
    Implements ikeyvt2(Of String)

    Private ReadOnly dr As data_dir
    Private ReadOnly ci As capinfo
    'do not have enough time to go through all the files and see if the key_count exceeded
    'Private ReadOnly max_key_count As Int64

    Public Sub New(ByVal data_dir As String)
        Me.dr = New data_dir(data_dir)
        Me.ci = New capinfo(data_dir)
    End Sub

    Public Shared Function ctor(ByVal data_dir As String) As istrkeyvt
        Return adapt(New file_key(data_dir))
    End Function

    Private Shared Function file_to_key(ByVal filename As String, ByRef key() As Byte) As Boolean
        key = hex_bytes_buff(filename)
        Return hex_bytes(filename, key)
    End Function

    Private Shared Function key_to_file(ByVal key() As Byte, ByRef filename As String) As Boolean
        assert(Not isemptyarray(key))
        filename = hex_str(key)
        Return True
    End Function

    Private Function file_full_path(ByVal key() As Byte, ByRef filename As String) As Boolean
        If key_to_file(key, filename) Then
            filename = dr.fullpath(filename)
            Return True
        Else
            Return False
        End If
    End Function

    Private Function key(ByVal file_full_path As String, ByRef k() As Byte) As Boolean
        Return file_to_key(dr.relative_path(file_full_path), k)
    End Function

    Private Function list_files(ByVal f As ref(Of String())) As event_comb
        Return _localfile.list_files(dr.base_directory(), f)
    End Function

    Public Shared Function create(ByVal p As var, ByRef o As file_key) As Boolean
        If p Is Nothing OrElse p.other_values().empty() Then
            Return False
        Else
            o = New file_key(p.other_values()(0))
            Return True
        End If
    End Function

    Public Shared Function create(ByVal p As var, ByRef o As istrkeyvt) As Boolean
        Dim f As file_key = Nothing
        If create(p, f) Then
            o = adapt(f)
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared Sub init()
        assert(constructor.register(Of file_key)(AddressOf create))
        assert(constructor.register(Of istrkeyvt)("file-key", AddressOf create))
    End Sub
End Class
