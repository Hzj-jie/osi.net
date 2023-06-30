
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.template

Friend Class ikeyvt_input_validation_wrapper(Of THREADSAFE As _boolean)
    Implements ikeyvt(Of THREADSAFE)

    Private ReadOnly impl As ikeyvt(Of THREADSAFE)

    Public Sub New(ByVal impl As ikeyvt(Of THREADSAFE))
        assert(Not impl Is Nothing)
        Me.impl = impl
    End Sub

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of THREADSAFE).append
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.append(key, value, ts, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.append(key, value, ts, result)
                                       End Function)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb _
                            Implements ikeyvt(Of THREADSAFE).capacity
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.capacity(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.capacity(result)
                                       End Function)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of THREADSAFE).delete
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.delete(key, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.delete(key, result)
                                       End Function)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of THREADSAFE).empty
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.empty(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.empty(result)
                                       End Function)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of THREADSAFE).full
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.full(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.full(result)
                                       End Function)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvt(Of THREADSAFE).heartbeat
        Return impl.heartbeat()
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb _
                            Implements ikeyvt(Of THREADSAFE).keycount
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.keycount(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.keycount(result)
                                       End Function)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb _
                        Implements ikeyvt(Of THREADSAFE).list
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.list(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.list(result)
                                       End Function)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of THREADSAFE).modify
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.modify(key, value, ts, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.modify(key, value, ts, result)
                                       End Function)
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements ikeyvt(Of THREADSAFE).read
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.read(key, result, ts)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.read(key, result, ts)
                                       End Function)
    End Function

    Public Function retire() As event_comb Implements ikeyvt(Of THREADSAFE).retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvt(Of THREADSAFE).seek
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.seek(key, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.seek(key, result)
                                       End Function)
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As ref(Of Int64)) As event_comb Implements ikeyvt(Of THREADSAFE).sizeof
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.sizeof(key, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.sizeof(key, result)
                                       End Function)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvt(Of THREADSAFE).stop
        Return impl.stop()
    End Function

    Public Function unique_write(ByVal key() As Byte,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb _
                                Implements ikeyvt(Of THREADSAFE).unique_write
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.unique_write(key, value, ts, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.unique_write(key, value, ts, result)
                                       End Function)
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb _
                             Implements ikeyvt(Of THREADSAFE).valuesize
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.valuesize(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.valuesize(result)
                                       End Function)
    End Function
End Class
