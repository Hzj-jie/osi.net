
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.constants.filesystem
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.convertor
Imports osi.service.selector
Imports constructor = osi.service.device.constructor

<global_init(global_init_level.server_services)>
Public Class file_index
    Private Const ts_prefix As Byte = 0
    Private Const fn_prefix As Byte = 1
    Private Const buff_size As Int32 = 16 * 1024
    Private Const index_file As String = "$.fii"
    Private Const content_file As String = "$.fic"
    Private ReadOnly index As ikeyvalue
    Private ReadOnly dr As data_dir
    Private ReadOnly ci As capinfo

    Shared Sub New()
        assert(ts_prefix <> fn_prefix)
    End Sub

    Public Sub New(ByVal index As ikeyvalue, ByVal data_dir As String)
        assert(Not index Is Nothing)
        Me.index = index
        Me.dr = New data_dir(data_dir)
        Me.ci = New capinfo(data_dir)
    End Sub

    Public Shared Function gnew(Of T)(ByVal index As ikeyvalue2(Of T),
                                      ByVal data_dir As String) As file_index
        Return New file_index(adapter.ikeyvalue2_ikeyvalue(index), data_dir)
    End Function

    Public Shared Function ctor(ByVal index As ikeyvalue, ByVal data_dir As String) As istrkeyvt
        Return adapt(New file_index(index, data_dir))
    End Function

    Public Shared Function ctor(Of T)(ByVal index As ikeyvalue2(Of T),
                                      ByVal data_dir As String) As istrkeyvt
        Return adapt(gnew(index, data_dir))
    End Function

    Public Shared Function ctor(ByVal r As pointer(Of file_index),
                                ByVal data_dir As String,
                                Optional ByVal buff_size As Int32 = npos,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Dim ec As event_comb = Nothing
        Dim f As pointer(Of fces) = Nothing
        Return New event_comb(Function() As Boolean
                                  f = New pointer(Of fces)()
                                  ec = fces.ctor(f,
                                                 IO.Path.Combine(data_dir, index_file),
                                                 IO.Path.Combine(data_dir, content_file),
                                                 buff_size,
                                                 max_key_count)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(r, gnew(+f, data_dir)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function ctor(ByVal r As pointer(Of istrkeyvt),
                                ByVal data_dir As String,
                                Optional ByVal buff_size As Int32 = npos,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of file_index) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of file_index)()
                                  ec = ctor(p, data_dir, buff_size, max_key_count)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(r, adapt(+p)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Function filename_key(ByVal key() As Byte) As Byte()
        Return merge_key(key, fn_prefix)
    End Function

    Private Shared Function timestamp_key(ByVal key() As Byte) As Byte()
        Return merge_key(key, ts_prefix)
    End Function

    Private Shared Function is_timestamp_key(ByVal key() As Byte, ByRef original() As Byte) As Boolean
        Return is_merged_key(key, ts_prefix, original)
    End Function

    Private Function new_filename(ByVal r As pointer(Of String)) As event_comb
        Dim ec As event_comb = Nothing
        Dim s As String = Nothing
        Dim ex As pointer(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  ex = New pointer(Of Boolean)()
                                  Return goto_next()
                              End Function,
                              Function() As Boolean
                                  s = strcat(guid_str(), extension_prefix, nowadays.milliseconds())
                                  ec = file_exists(dr.fullpath(s), ex)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If Not (+ex) Then
                                          Return eva(r, s) AndAlso
                                                 goto_end()
                                      Else
                                          Return goto_prev()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Class atom
        Private ReadOnly filename As String
        Public ReadOnly ts_key() As Byte
        Public ReadOnly fn_key() As Byte
        Public ReadOnly timestamp As Int64
        Private ReadOnly fi As file_index

        Private Sub New(ByVal fi As file_index,
                        ByVal filename As String,
                        ByVal ts_key() As Byte,
                        ByVal fn_key() As Byte,
                        ByVal ts As Int64)
            assert(Not fi Is Nothing)
            Me.fi = fi
            Me.filename = filename
            Me.ts_key = ts_key
            Me.fn_key = fn_key
            Me.timestamp = ts
        End Sub

        Public Function full_file_path() As String
            Return fi.dr.fullpath(filename)
        End Function

        Public Shared Function ctor(ByVal fi As file_index,
                                    ByVal key() As Byte,
                                    ByVal o As pointer(Of atom)) As event_comb
            assert(Not fi Is Nothing)
            Dim ec As event_comb = Nothing
            Dim b As pointer(Of Byte()) = Nothing
            Dim ts_key() As Byte = Nothing
            Dim fn_key() As Byte = Nothing
            Dim ts As Int64 = 0
            Dim fn As String = Nothing
            Return New event_comb(Function() As Boolean
                                      b = New pointer(Of Byte())()
                                      ts_key = timestamp_key(key)
                                      ec = fi.index.read(ts_key, b)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() AndAlso
                                         to_timestamp(+b, ts) Then
                                          b.clear()
                                          fn_key = filename_key(key)
                                          ec = fi.index.read(fn_key, b)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return False
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() Then
                                          fn = bytes_str(+b)
                                          Return eva(o, New atom(fi, fn, ts_key, fn_key, ts)) AndAlso
                                                 goto_end()
                                      Else
                                          Return False
                                      End If
                                  End Function)
        End Function
    End Class

    Public Shared Function create(ByVal v As var, ByVal o As pointer(Of file_index)) As event_comb
        Const buff_size As String = "buff-size"
        Const max_key_count As String = "max-key-count"
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If v Is Nothing OrElse v.other_values().empty() Then
                                      Return False
                                  Else
                                      v.bind(buff_size,
                                             max_key_count)
                                      Dim bs As Int32 = 0
                                      Dim mkc As Int64 = 0
                                      bs = v.value(buff_size).to_int32(npos)
                                      mkc = v.value(max_key_count).to_int64(npos)
                                      ec = ctor(o,
                                                v.other_values()(0),
                                                bs,
                                                mkc)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function create(ByVal v As var, ByVal o As pointer(Of istrkeyvt)) As event_comb
        Dim ec As event_comb = Nothing
        Dim p As pointer(Of file_index) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New pointer(Of file_index)()
                                  ec = create(v, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         assert(Not +p Is Nothing) AndAlso
                                         eva(o, adapt(+p)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function create_as_file_index(ByVal v As var) As async_getter(Of file_index)
        Return async_preparer.[New](Function(p As pointer(Of file_index)) As event_comb
                                        Return create(v, p)
                                    End Function)
    End Function

    Public Shared Function create_as_istrkeyvt(ByVal v As var) As async_getter(Of istrkeyvt)
        Return async_preparer.[New](Function(p As pointer(Of istrkeyvt)) As event_comb
                                        Return create(v, p)
                                    End Function)
    End Function

    Private Shared Sub init()
        assert(constructor.register(AddressOf create_as_file_index))
        assert(constructor.register("file-index", AddressOf create_as_istrkeyvt))
    End Sub
End Class
