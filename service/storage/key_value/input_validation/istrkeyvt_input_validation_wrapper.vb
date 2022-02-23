
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector

Friend Class istrkeyvt_input_validation_wrapper
    Implements istrkeyvt

    Private ReadOnly impl As istrkeyvt

    Public Sub New(ByVal impl As istrkeyvt)
        assert(impl IsNot Nothing)
        Me.impl = impl
    End Sub

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.append(key, value, ts, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.append(key, value, ts, result)
                                       End Function)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.capacity(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.capacity(result)
                                       End Function)
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.delete(key, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.delete(key, result)
                                       End Function)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.empty
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.empty(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.empty(result)
                                       End Function)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.full
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.full(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.full(result)
                                       End Function)
    End Function

    Public Function heartbeat() As event_comb Implements istrkeyvt.heartbeat
        Return impl.heartbeat()
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.keycount
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.keycount(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.keycount(result)
                                       End Function)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of String))) As event_comb Implements istrkeyvt.list
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.list(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.list(result)
                                       End Function)
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.modify(key, value, ts, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.modify(key, value, ts, result)
                                       End Function)
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements istrkeyvt.read
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.read(key, result, ts)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.read(key, result, ts)
                                       End Function)
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.seek(key, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.seek(key, result)
                                       End Function)
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.sizeof(key, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.sizeof(key, result)
                                       End Function)
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return impl.stop()
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.unique_write(key, value, ts, result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.unique_write(key, value, ts, result)
                                       End Function)
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return event_comb.chain_before(Function() As Boolean
                                           Return input_validation.valuesize(result)
                                       End Function,
                                       Function() As event_comb
                                           Return impl.valuesize(result)
                                       End Function)
    End Function
End Class
