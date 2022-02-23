
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure

Friend Class isynckeyvalue_ikeyvalue
    Implements ikeyvalue

    Private ReadOnly i As isynckeyvalue

    Public Sub New(ByVal i As isynckeyvalue)
        assert(i IsNot Nothing)
        Me.i = i
    End Sub

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.append
        Return sync_async(Function(ByRef x) i.append(key, value, x), result, +result)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.capacity
        Return sync_async(Function(ByRef x) i.capacity(x), result, +result)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.delete
        Return sync_async(Function(ByRef x) i.delete(key, x), result, +result)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.empty
        Return sync_async(Function(ByRef x) i.empty(x), result, +result)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.full
        Return sync_async(Function(ByRef x) i.full(x), result, +result)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvalue.heartbeat
        Return sync_async(AddressOf i.heartbeat)
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.keycount
        Return sync_async(Function(ByRef x) i.keycount(x), result, +result)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb Implements ikeyvalue.list
        Return sync_async(Function(ByRef x) i.list(x), result, +result)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.modify
        Return sync_async(Function(ByRef x) i.modify(key, value, x), result, +result)
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal value As ref(Of Byte())) As event_comb Implements ikeyvalue.read
        Return sync_async(Function(ByRef x) i.read(key, x), value, +value)
    End Function

    Public Function retire() As event_comb Implements ikeyvalue.retire
        Return sync_async(AddressOf i.retire)
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.seek
        Return sync_async(Function(ByRef x) i.seek(key, x), result, +result)
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.sizeof
        Return sync_async(Function(ByRef x) i.sizeof(key, x), result, +result)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvalue.stop
        Return sync_async(AddressOf i.stop)
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.valuesize
        Return sync_async(Function(ByRef x) i.valuesize(x), result, +result)
    End Function
End Class
