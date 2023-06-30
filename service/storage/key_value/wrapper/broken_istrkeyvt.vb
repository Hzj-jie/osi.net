
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.device

'test purpose only
<global_init(global_init_level.server_services)>
Public Class broken_istrkeyvt
    Implements istrkeyvt

    Private ReadOnly impl As istrkeyvt

    Public Sub New(ByVal impl As istrkeyvt)
        assert(Not impl Is Nothing)
        Me.impl = impl
    End Sub

    Private Function random_broken(ByVal d As Func(Of event_comb)) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If rnd_bool() Then
                                      Return rnd_bool() AndAlso
                                             goto_end()
                                  Else
                                      ec = d()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return random_broken(Function() impl.append(key, value, ts, result))
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return random_broken(Function() impl.capacity(result))
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return random_broken(Function() impl.delete(key, result))
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.empty
        Return random_broken(Function() impl.empty(result))
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.full
        Return random_broken(Function() impl.full(result))
    End Function

    Public Function heartbeat() As event_comb Implements istrkeyvt.heartbeat
        Return impl.heartbeat()
        'Return random_broken(Function() impl.heartbeat())
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.keycount
        Return random_broken(Function() impl.keycount(result))
    End Function

    Public Function list(ByVal result As ref(Of vector(Of String))) As event_comb Implements istrkeyvt.list
        Return random_broken(Function() impl.list(result))
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return random_broken(Function() impl.modify(key, value, ts, result))
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements istrkeyvt.read
        Return random_broken(Function() impl.read(key, result, ts))
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return impl.retire()
        'Return random_broken(Function() impl.retire())
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return random_broken(Function() impl.seek(key, result))
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return random_broken(Function() impl.sizeof(key, result))
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return random_broken(Function() impl.stop())
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return random_broken(Function() impl.unique_write(key, value, ts, result))
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return random_broken(Function() impl.valuesize(result))
    End Function

    Private Shared Sub init()
        assert(wrapper.register("broken",
                                Function(v As var,
                                         i As istrkeyvt,
                                         ByRef o As istrkeyvt) As Boolean
                                    o = New broken_istrkeyvt(i)
                                    Return True
                                End Function))
    End Sub
End Class
