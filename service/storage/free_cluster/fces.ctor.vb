
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.selector
Imports constructor = osi.service.device.constructor
Imports store_t = osi.root.formation.unordered_map(Of osi.root.connector.array_ref(Of Byte),
                                                      osi.root.formation.pair(Of System.Int64, System.Int64))

<global_init(global_init_level.server_services)>
Partial Public NotInheritable Class fces
    Private Sub New(ByVal index As free_cluster, ByVal content As free_cluster, ByVal max_key_count As Int64)
        assert(index IsNot Nothing)
        assert(content IsNot Nothing)
        Me.index = index
        Me.content = content
        Me.max_key_count = If(max_key_count <= 0, max_int64, max_key_count)
        Me.m = New store_t()
    End Sub

    Private Shared Function ctor(ByVal r As ref(Of fces),
                                 ByVal d1 As Func(Of ref(Of free_cluster), event_comb),
                                 ByVal d2 As Func(Of ref(Of free_cluster), event_comb),
                                 Optional ByVal max_key_count As Int64 = npos) As event_comb
        assert(d1 IsNot Nothing)
        assert(d2 IsNot Nothing)
        Dim ec1 As event_comb = Nothing
        Dim ec2 As event_comb = Nothing
        Dim f1 As ref(Of free_cluster) = Nothing
        Dim f2 As ref(Of free_cluster) = Nothing
        Return New event_comb(Function() As Boolean
                                  f1 = New ref(Of free_cluster)()
                                  f2 = New ref(Of free_cluster)()
                                  ec1 = d1(f1)
                                  ec2 = d2(f2)
                                  Return waitfor(ec1, ec2) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec1.end_result() AndAlso
                                     ec2.end_result() Then
                                      ec1 = ctor(r, +f1, +f2, max_key_count)
                                      Return waitfor(ec1) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec1.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Function ctor(ByVal r As ref(Of istrkeyvt),
                                 ByVal d As Func(Of ref(Of fces), event_comb)) As event_comb
        assert(d IsNot Nothing)
        Dim f As ref(Of fces) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  f = New ref(Of fces)()
                                  ec = d(f)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(r, adapt(+f)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function ctor(ByVal r As ref(Of fces),
                                ByVal index As free_cluster,
                                ByVal content As free_cluster,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Dim ec As event_comb = Nothing
        Dim f As fces = Nothing
        Return New event_comb(Function() As Boolean
                                  If index Is Nothing OrElse content Is Nothing Then
                                      Return False
                                  Else
                                      f = New fces(index, content, max_key_count)
                                      ec = f.open()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(r, f) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function ctor(ByVal r As ref(Of istrkeyvt),
                                ByVal index As free_cluster,
                                ByVal content As free_cluster,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Return ctor(r, Function(x) ctor(x, index, content, max_key_count))
    End Function

    Public Shared Function ctor(ByVal r As ref(Of fces),
                                ByVal index_vd As virtdisk,
                                ByVal content_vd As virtdisk,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Return ctor(r,
                    Function(x) free_cluster.ctor(x, index_vd),
                    Function(x) free_cluster.ctor(x, content_vd),
                    max_key_count)
    End Function

    Public Shared Function ctor(ByVal r As ref(Of istrkeyvt),
                                ByVal index_vd As virtdisk,
                                ByVal content_vd As virtdisk,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Return ctor(r, Function(x) ctor(x, index_vd, content_vd, max_key_count))
    End Function

    Public Shared Function ctor(ByVal r As ref(Of fces),
                                ByVal index_file As String,
                                ByVal content_file As String,
                                Optional ByVal buff_size As Int32 = npos,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Return ctor(r,
                    Function(x) free_cluster.ctor(x, index_file, buff_size),
                    Function(x) free_cluster.ctor(x, content_file, buff_size),
                    max_key_count)
    End Function

    Public Shared Function ctor(ByVal r As ref(Of istrkeyvt),
                                ByVal index_file As String,
                                ByVal content_file As String,
                                Optional ByVal buff_size As Int32 = npos,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Return ctor(r, Function(x) ctor(x, index_file, content_file, buff_size, max_key_count))
    End Function

    Public Shared Function ctor(ByVal r As ref(Of fces),
                                ByVal file As String,
                                Optional ByVal buff_size As Int32 = npos,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Return ctor(r,
                    strcat(file, index_suffix),
                    strcat(file, content_suffix),
                    buff_size,
                    max_key_count)
    End Function

    Public Shared Function ctor(ByVal r As ref(Of istrkeyvt),
                                ByVal file As String,
                                Optional ByVal buff_size As Int32 = npos,
                                Optional ByVal max_key_count As Int64 = npos) As event_comb
        Return ctor(r, Function(x) ctor(x, file, buff_size, max_key_count))
    End Function

    Public Shared Function memory_fces(ByVal r As ref(Of fces),
                                       Optional ByVal max_key_count As Int64 = npos) As event_comb
        Return ctor(r,
                    virtdisk.memory_virtdisk(),
                    virtdisk.memory_virtdisk(),
                    max_key_count)
    End Function

    Public Shared Function memory_fces(ByVal r As ref(Of istrkeyvt),
                                       Optional ByVal max_key_count As Int64 = npos) As event_comb
        Return ctor(r,
                    virtdisk.memory_virtdisk(),
                    virtdisk.memory_virtdisk(),
                    max_key_count)
    End Function

    Public Shared Function create(ByVal p As var, ByVal o As ref(Of fces)) As event_comb
        Const in_mem As String = "in-mem"
        Const buff_size As String = "buff-size"
        Const max_key_count As String = "max-key-count"
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If p Is Nothing Then
                                      Return False
                                  Else
                                      p.bind(in_mem,
                                             buff_size,
                                             max_key_count)
                                      Dim bs As Int32 = 0
                                      Dim mkc As Int64 = 0
                                      bs = p.value(buff_size).to(Of Int32)(npos)
                                      mkc = p.value(max_key_count).to(Of Int64)(npos)
                                      If p.value(in_mem).to(Of Boolean)() Then
                                          ec = memory_fces(o, mkc)
                                      ElseIf p.other_values().empty() Then
                                          Return False
                                      ElseIf p.other_values().size() = 1 Then
                                          ec = ctor(o,
                                                    p.other_values()(0),
                                                    bs,
                                                    mkc)
                                      Else
                                          ec = ctor(o,
                                                    p.other_values()(0),
                                                    p.other_values()(1),
                                                    bs,
                                                    mkc)
                                      End If
                                      assert(ec IsNot Nothing)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function create(ByVal v As var, ByVal o As ref(Of istrkeyvt)) As event_comb
        Return ctor(o, Function(x) create(v, x))
    End Function

    Public Shared Function create_as_istrkeyvt(ByVal v As var) As async_getter(Of istrkeyvt)
        Return async_preparer.[New](Function(p As ref(Of istrkeyvt)) As event_comb
                                        Return create(v, p)
                                    End Function)
    End Function

    Public Shared Function create_as_fces(ByVal v As var) As async_getter(Of fces)
        Return async_preparer.[New](Function(p As ref(Of fces)) As event_comb
                                        Return create(v, p)
                                    End Function)
    End Function

    Private Shared Sub init()
        assert(constructor.register(AddressOf create_as_fces))
        assert(constructor.register("fces", AddressOf create_as_istrkeyvt))
    End Sub
End Class
