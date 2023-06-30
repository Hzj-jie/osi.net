
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils

Public Class isynckeyvalue2_ikeyvalue2(Of SEEK_RESULT)
    Implements ikeyvalue2(Of SEEK_RESULT)

    Private ReadOnly impl As isynckeyvalue2(Of SEEK_RESULT)

    Public Sub New(ByVal impl As isynckeyvalue2(Of SEEK_RESULT))
        assert(Not impl Is Nothing)
        Me.impl = impl
    End Sub

    Public Function append_existing(ByVal sr As SEEK_RESULT,
                                    ByVal value() As Byte,
                                    ByVal result As ref(Of Boolean)) As event_comb _
                                   Implements ikeyvalue2(Of SEEK_RESULT).append_existing
        Return sync_async(Function(ByRef x) impl.append_existing(sr, value, x), result, +result)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb _
                            Implements ikeyvalue2(Of SEEK_RESULT).capacity
        Return sync_async(Function(ByRef x) impl.capacity(x), result, +result)
    End Function

    Public Function delete_existing(ByVal sr As SEEK_RESULT,
                                    ByVal result As ref(Of Boolean)) As event_comb _
                                   Implements ikeyvalue2(Of SEEK_RESULT).delete_existing
        Return sync_async(Function(ByRef x) impl.delete_existing(sr, x), result, +result)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb _
                         Implements ikeyvalue2(Of SEEK_RESULT).empty
        Return sync_async(Function(ByRef x) impl.empty(x), result, +result)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue2(Of SEEK_RESULT).full
        Return sync_async(Function(ByRef x) impl.full(x), result, +result)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvalue2(Of SEEK_RESULT).heartbeat
        Return sync_async(Function() impl.heartbeat())
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb _
                            Implements ikeyvalue2(Of SEEK_RESULT).keycount
        Return sync_async(Function(ByRef x) impl.keycount(x), result, +result)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb _
                        Implements ikeyvalue2(Of SEEK_RESULT).list
        Return sync_async(Function(ByRef x) impl.list(x), result, +result)
    End Function

    Public Function read_existing(ByVal r As SEEK_RESULT,
                                  ByVal value As ref(Of Byte())) As event_comb _
                                 Implements ikeyvalue2(Of SEEK_RESULT).read_existing
        Return sync_async(Function(ByRef x) impl.read_existing(r, x), value, +value)
    End Function

    Public Function retire() As event_comb Implements ikeyvalue2(Of SEEK_RESULT).retire
        Return sync_async(Function() impl.retire())
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal r As ref(Of SEEK_RESULT),
                         ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue2(Of SEEK_RESULT).seek
        Return New event_comb(Function() As Boolean
                                  Dim x As SEEK_RESULT = Nothing
                                  Dim y As Boolean = False
                                  Return impl.seek(key, x, y) AndAlso
                                         eva(r, x) AndAlso
                                         eva(result, y) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function sizeof_existing(ByVal r As SEEK_RESULT,
                                    ByVal result As ref(Of Int64)) As event_comb _
                                   Implements ikeyvalue2(Of SEEK_RESULT).sizeof_existing
        Return sync_async(Function(ByRef x) impl.sizeof_existing(r, x), result, +result)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvalue2(Of SEEK_RESULT).stop
        Return sync_async(Function() impl.stop())
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb _
                             Implements ikeyvalue2(Of SEEK_RESULT).valuesize
        Return sync_async(Function(ByRef x) impl.valuesize(x), result, +result)
    End Function

    Public Function write_new(ByVal key() As Byte,
                              ByVal value() As Byte,
                              ByVal result As ref(Of Boolean)) As event_comb _
                             Implements ikeyvalue2(Of SEEK_RESULT).write_new
        Return sync_async(Function(ByRef x) impl.write_new(key, value, x), result, +result)
    End Function
End Class
