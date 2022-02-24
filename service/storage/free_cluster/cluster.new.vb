
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Partial Public Class cluster
    Shared Sub New()
        verify_constants()
    End Sub

    Private Sub New(ByVal id As Int64,
                    ByVal offset As UInt64,
                    ByVal length As UInt64,
                    ByVal vd As virtdisk,
                    ByVal used As Int64,
                    ByVal prev_id As Int64,
                    ByVal next_id As Int64)
        _id = id
        _offset = offset
        _length = length
        _vd = vd
        _used = used
        _prev_id = prev_id
        _next_id = next_id

        assert_valid_status_ctor()
    End Sub

    'create a new cluster in the virtdisk
    'the real cluster length is not guaranteed to be same as length input
    Public Shared Function ctor(ByVal vd As virtdisk,
                                ByVal id As Int64,
                                ByVal offset As UInt64,
                                ByVal length As UInt64,
                                ByVal r As ref(Of cluster)) As event_comb
        Dim ec As event_comb = Nothing
        Dim c As cluster = Nothing
        Return New event_comb(Function() As Boolean
                                  If Not vd Is Nothing AndAlso
                                     valid_id(id) AndAlso
                                     valid_offset(offset) AndAlso
                                     valid_length(length) Then
                                      If length < MIN_CLUSTER_LENGTH Then
                                          length = MIN_CLUSTER_LENGTH
                                      ElseIf ((length - MIN_CLUSTER_LENGTH) Mod DISK_CLUSTER_SIZE) <> 0 Then
                                          length = MIN_CLUSTER_LENGTH +
                                                   ((((length - MIN_CLUSTER_LENGTH) Mod DISK_CLUSTER_SIZE) + uint32_1) *
                                                    DISK_CLUSTER_SIZE)
                                      End If
                                      assert(((length + STRUCTURE_SIZE) Mod DISK_CLUSTER_SIZE) = 0)
                                      c = New cluster(id, offset, length, vd, FREE_USED, INVALID_ID, INVALID_ID)
                                      ec = c.write_structure()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      ec = c.enlarge_virtdisk()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return c.virtdisk_is_fitting() AndAlso
                                         eva(r, c) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Shared Function ctor(ByVal vd As virtdisk,
                                ByVal id As Int64,
                                ByVal length As UInt64,
                                ByVal r As ref(Of cluster)) As event_comb
        Return ctor(vd, id, If(vd Is Nothing, uint64_0, vd.size()), length, r)
    End Function

    'create an existing cluster from the virtdisk
    Public Shared Function ctor(ByVal vd As virtdisk,
                                ByVal offset As UInt64,
                                ByVal r As ref(Of cluster)) As event_comb
        Dim id As ref(Of Int64) = Nothing
        Dim used As ref(Of Int64) = Nothing
        Dim length As ref(Of UInt64) = Nothing
        Dim next_id As ref(Of Int64) = Nothing
        Dim prev_id As ref(Of Int64) = Nothing
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If vd Is Nothing Then
                                      Return False
                                  Else
                                      id = New ref(Of Int64)()
                                      used = New ref(Of Int64)()
                                      length = New ref(Of UInt64)()
                                      next_id = New ref(Of Int64)()
                                      prev_id = New ref(Of Int64)()
                                      ec = read_structure(vd, offset, id, used, length, next_id, prev_id)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         eva(r, New cluster(+id,
                                                            +offset,
                                                            +length,
                                                            vd,
                                                            +used,
                                                            +prev_id,
                                                            +next_id)) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
