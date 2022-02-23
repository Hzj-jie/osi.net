
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.connector
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.server_services)>
Public Class timestamp_override_istrkeyvt
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
        Return impl.append(key, value, now_as_timestamp(), result)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return impl.capacity(result)
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return impl.delete(key, result)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.empty
        Return impl.empty(result)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.full
        Return impl.full(result)
    End Function

    Public Function heartbeat() As event_comb Implements istrkeyvt.heartbeat
        Return impl.heartbeat()
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.keycount
        Return impl.keycount(result)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of String))) As event_comb Implements istrkeyvt.list
        Return impl.list(result)
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return impl.modify(key, value, now_as_timestamp(), result)
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements istrkeyvt.read
        Return impl.read(key, result, ts)
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return impl.seek(key, result)
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return impl.sizeof(key, result)
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return impl.stop()
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return impl.unique_write(key, value, now_as_timestamp(), result)
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return impl.valuesize(result)
    End Function

    Private Shared Sub init()
        assert(wrapper.register("timestamp-override",
                                Function(v As var,
                                         i As istrkeyvt,
                                         ByRef o As istrkeyvt) As Boolean
                                    o = New timestamp_override_istrkeyvt(i)
                                    Return True
                                End Function))
    End Sub
End Class
