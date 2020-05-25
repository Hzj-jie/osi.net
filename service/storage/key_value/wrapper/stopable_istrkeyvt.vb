
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.lock
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.server_services)>
Public Class stopable_istrkeyvt
    Implements istrkeyvt

    Private ReadOnly impl As istrkeyvt
    Private ReadOnly stopping As pointer(Of singleentry)
    Private ReadOnly working As atomic_int

    Shared Sub New()
        assert(DirectCast(Nothing, Boolean) = False)
    End Sub

    Public Sub New(ByVal impl As istrkeyvt)
        assert(Not impl Is Nothing)
        Me.impl = impl
        Me.stopping = New pointer(Of singleentry)()
        Me.working = New atomic_int()
    End Sub

    Public Function stopped() As Boolean
        Return stopping.in_use()
    End Function

    Private Function work(ByVal d As Func(Of event_comb)) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If stopped() Then
                                      Return False
                                  Else
                                      Me.working.increment()
                                      If stopped() Then
                                          Me.working.decrement()
                                          Return False
                                      Else
                                          ec = d()
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert(Me.working.decrement() >= 0)
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return work(Function() impl.append(key, value, ts, result))
    End Function

    Public Function capacity(ByVal result As pointer(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return work(Function() impl.capacity(result))
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return work(Function() impl.delete(key, result))
    End Function

    Public Function empty(ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.empty
        Return work(Function() impl.empty(result))
    End Function

    Public Function full(ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.full
        Return work(Function() impl.full(result))
    End Function

    Public Function heartbeat() As event_comb Implements istrkeyvt.heartbeat
        Return work(Function() impl.heartbeat())
    End Function

    Public Function keycount(ByVal result As pointer(Of Int64)) As event_comb Implements istrkeyvt.keycount
        Return work(Function() impl.keycount(result))
    End Function

    Public Function list(ByVal result As pointer(Of vector(Of String))) As event_comb Implements istrkeyvt.list
        Return work(Function() impl.list(result))
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return work(Function() impl.modify(key, value, ts, result))
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As pointer(Of Byte()),
                         ByVal ts As pointer(Of Int64)) As event_comb Implements istrkeyvt.read
        Return work(Function() impl.read(key, result, ts))
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return work(Function() impl.retire())
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return work(Function() impl.seek(key, result))
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As pointer(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return work(Function() impl.sizeof(key, result))
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If stopping.mark_in_use() Then
                                      Return waitfor(Function() (+working) = 0) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  ec = impl.stop()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Return goto_end()
                                  Else
                                      stopping.release()
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As pointer(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return work(Function() impl.unique_write(key, value, ts, result))
    End Function

    Public Function valuesize(ByVal result As pointer(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return work(Function() impl.valuesize(result))
    End Function

    Private Shared Sub init()
        assert(wrapper.register("stopable",
                                Function(v As var,
                                         i As istrkeyvt,
                                         ByRef o As istrkeyvt) As Boolean
                                    o = New stopable_istrkeyvt(i)
                                    Return True
                                End Function))
    End Sub
End Class
