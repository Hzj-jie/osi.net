
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.server_services)>
Public Class key_locked_istrkeyvt
    Implements istrkeyvt

    Private ReadOnly impl As istrkeyvt
    Private ReadOnly locks As multilock(Of event_comb_lock)

    Public Sub New(ByVal impl As istrkeyvt)
        assert(Not impl Is Nothing)
        Me.impl = impl
        Me.locks = New multilock(Of event_comb_lock)()
    End Sub

    Public Function lock(ByVal key As String) As Boolean
        Return waitfor(locks, key)
    End Function

    Public Sub release(ByVal key As String)
        locks.release(key)
    End Sub

    Private Function locked(ByVal key As String,
                            ByVal d As Func(Of event_comb)) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Return lock(key) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  ec = d()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  release(key)
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return locked(key, Function() impl.append(key, value, ts, result))
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return impl.capacity(result)
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return locked(key, Function() impl.delete(key, result))
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
        Return locked(key, Function() impl.modify(key, value, ts, result))
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements istrkeyvt.read
        Return locked(key, Function() impl.read(key, result, ts))
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return locked(key, Function() impl.seek(key, result))
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return locked(key, Function() impl.sizeof(key, result))
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return impl.stop()
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return locked(key, Function() impl.unique_write(key, value, ts, result))
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return impl.valuesize(result)
    End Function

    Private Shared Sub init()
        assert(wrapper.register("key-locked",
                                Function(v As var,
                                         i As istrkeyvt,
                                         ByRef o As istrkeyvt) As Boolean
                                    o = New key_locked_istrkeyvt(i)
                                    Return True
                                End Function))
    End Sub
End Class
