
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports clusters_t = osi.root.formation.unordered_map(Of System.Int64, osi.service.storage.cluster)

Partial Public NotInheritable Class free_cluster
    'all the clusters
    Private ReadOnly cs As clusters_t
    'a subset of cs, only head clusters
    Private ReadOnly hcs As clusters_t
    'a subset of cs, free clusters
    Private ReadOnly fcs As queue(Of cluster)
    Private ReadOnly vd As virtdisk
    Private max_cluster_id As Int64

    Shared Sub New()
        assert(npos < 0)
    End Sub

    Private Sub New(ByVal i As virtdisk)
        assert(Not i Is Nothing)
        cs = New clusters_t()
        hcs = New clusters_t()
        fcs = New queue(Of cluster)()
        max_cluster_id = 0
        vd = i
    End Sub

    Public Shared Function ctor(ByVal r As ref(Of free_cluster),
                                ByVal i As virtdisk) As event_comb
        Dim ec As event_comb = Nothing
        Dim f As free_cluster = Nothing
        Return New event_comb(Function() As Boolean
                                  If i Is Nothing Then
                                      Return False
                                  Else
                                      f = New free_cluster(i)
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

    Public Shared Function ctor(ByVal r As ref(Of free_cluster),
                                ByVal file As String,
                                Optional ByVal buff_size As Int32 = npos) As event_comb
        Return ctor(r, New virtdisk(file, buff_size))
    End Function

    Public Function file_name() As String
        Return vd.file_name()
    End Function

    Public Function capacity() As UInt64
        Return vd.capacity()
    End Function

    'only return false if cluster.ctor failed and drop data failed, which means harddisk error may happen
    Private Function open() As event_comb
        Dim offset As UInt64 = 0
        Dim ec As event_comb = Nothing
        Dim r As ref(Of cluster) = Nothing
        Return New event_comb(Function() As Boolean
                                  If offset < vd.size() Then
                                      If r Is Nothing Then
                                          r = New ref(Of cluster)()
                                      Else
                                          r.clear()
                                      End If
                                      ec = storage.cluster.ctor(vd, offset, r)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return goto_end()
                                  End If
                              End Function,
                                         Function() As Boolean
                                             If ec.end_result() AndAlso Not (+r) Is Nothing Then
                                                 If cs.find((+r).id()) <> cs.end() Then
                                                     raise_error(error_type.warning,
                                                                 "duplicate cluster id ",
                                                                 (+r).id(),
                                                                 " found in file ",
                                                                 file_name(),
                                                                 ", offset ",
                                                                 offset,
                                                                 ", over-written")
                                                 End If
                                                 cs((+r).id()) = (+r)
                                                 If head_cluster(+r) Then
                                                     hcs((+r).id()) = (+r)
                                                 ElseIf (+r).free() Then
                                                     assert(fcs.push(+r))
                                                 End If
                                                 If (+r).id() > max_cluster_id Then
                                                     max_cluster_id = (+r).id()
                                                 End If

                                                 offset = (+r).end_disk_offset()
                                                 assert(offset <= vd.size())
                                                 Return goto_prev()
                                             Else
                                                 raise_error(error_type.warning,
                                                             "failed to read cluster from file ",
                                                             file_name(),
                                                             ", offset ",
                                                             offset,
                                                             ", ignore following data")
                                                 ec = vd.drop(offset)
                                                 Return waitfor(ec) AndAlso
                                                        goto_next()
                                             End If
                                         End Function,
                                         Function() As Boolean
                                             Return ec.end_result() AndAlso
                                                    goto_end()
                                         End Function)
    End Function

    Public Sub close()
        vd.close()
    End Sub
End Class
