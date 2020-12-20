
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector

Friend Class ikeyvalue_input_validation_wrapper
    Implements ikeyvalue

    Private ReadOnly impl As ikeyvalue

    Public Sub New(ByVal impl As ikeyvalue)
        assert(Not impl Is Nothing)
        Me.impl = impl
    End Sub

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.append
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.append(key, value, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.append(key, value, result)
                                       End Function)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.capacity
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.capacity(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.capacity(result)
                                       End Function)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.delete
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.delete(key, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.delete(key, result)
                                       End Function)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.empty
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.empty(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.empty(result)
                                       End Function)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.full
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.full(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.full(result)
                                       End Function)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvalue.heartbeat
        Return impl.heartbeat()
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.keycount
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.keycount(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.keycount(result)
                                       End Function)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb Implements ikeyvalue.list
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.list(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.list(result)
                                       End Function)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.modify
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.modify(key, value, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.modify(key, value, result)
                                       End Function)
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal value As ref(Of Byte())) As event_comb Implements ikeyvalue.read
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.read(key, value)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.read(key, value)
                                       End Function)
    End Function

    Public Function retire() As event_comb Implements ikeyvalue.retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.seek
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.seek(key, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.seek(key, result)
                                       End Function)
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.sizeof
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.sizeof(key, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.sizeof(key, result)
                                       End Function)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvalue.stop
        Return impl.stop()
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.valuesize
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.valuesize(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.valuesize(result)
                                       End Function)
    End Function
End Class
