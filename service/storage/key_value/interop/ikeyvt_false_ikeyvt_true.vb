
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.template

Friend Class ikeyvt_false_ikeyvt_true
    Implements ikeyvt(Of _true)

    Private ReadOnly impl As ikeyvt(Of _false)
    Private ReadOnly l As ref(Of event_comb_lock)

    Public Sub New(ByVal impl As ikeyvt(Of _false))
        assert(Not impl Is Nothing)
        Me.impl = impl
        Me.l = New ref(Of event_comb_lock)()
    End Sub

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _true).append
        Return l.locked(Function() impl.append(key, value, ts, result))
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt(Of _true).capacity
        Return l.locked(Function() impl.capacity(result))
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _true).delete
        Return l.locked(Function() impl.delete(key, result))
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _true).empty
        Return l.locked(Function() impl.empty(result))
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _true).full
        Return l.locked(Function() impl.full(result))
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvt(Of _true).heartbeat
        Return l.locked(AddressOf impl.heartbeat)
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt(Of _true).keycount
        Return l.locked(Function() impl.keycount(result))
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb Implements ikeyvt(Of _true).list
        Return l.locked(Function() impl.list(result))
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _true).modify
        Return l.locked(Function() impl.modify(key, value, ts, result))
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements ikeyvt(Of _true).read
        Return l.locked(Function() impl.read(key, result, ts))
    End Function

    Public Function retire() As event_comb Implements ikeyvt(Of _true).retire
        Return l.locked(AddressOf impl.retire)
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of _true).seek
        Return l.locked(Function() impl.seek(key, result))
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt(Of _true).sizeof
        Return l.locked(Function() impl.sizeof(key, result))
    End Function

    Public Function [stop]() As event_comb Implements ikeyvt(Of _true).stop
        Return l.locked(AddressOf impl.stop)
    End Function

    Public Function unique_write(ByVal key() As Byte,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb _
                                Implements ikeyvt(Of _true).unique_write
        Return l.locked(Function() impl.unique_write(key, value, ts, result))
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt(Of _true).valuesize
        Return l.locked(Function() impl.valuesize(result))
    End Function
End Class
