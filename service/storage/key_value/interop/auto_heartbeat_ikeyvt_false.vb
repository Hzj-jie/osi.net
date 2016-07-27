
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector

Friend Class auto_heartbeat_ikeyvt_false
    Implements ikeyvt(Of _false)

    Private ReadOnly impl As ikeyvt(Of _false)

    Public Sub New(ByVal impl As ikeyvt(Of _false))
        assert(Not impl Is Nothing)
        Me.impl = impl
    End Sub

    Private Function retry(ByVal d As Func(Of event_comb)) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Dim retried As Boolean = False
        Return New event_comb(Function() As Boolean
                                  If ec Is Nothing OrElse ec.end_result() Then
                                      ec = d()
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Return goto_end()
                                  ElseIf Not retried Then
                                      retried = True
                                      ec = impl.heartbeat()
                                      Return waitfor(ec) AndAlso
                                             goto_begin()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Private Function not_full_retry(ByVal d As Func(Of event_comb)) As event_comb
        assert(Not d Is Nothing)
        Return retry(Function() As event_comb
                         Dim ec As event_comb = Nothing
                         Dim b As pointer(Of Boolean) = Nothing
                         Dim i As pointer(Of Int64) = Nothing
                         Return New event_comb(Function() As Boolean
                                                   ec = d()
                                                   Return waitfor(ec) AndAlso
                                                          goto_next()
                                               End Function,
                                               Function() As Boolean
                                                   If ec.end_result() Then
                                                       Return goto_end()
                                                   Else
                                                       i = New pointer(Of Int64)()
                                                       ec = impl.capacity(i)
                                                       Return waitfor(ec) AndAlso
                                                              goto_next()
                                                   End If
                                               End Function,
                                               Function() As Boolean
                                                   If ec.end_result() Then
                                                       If (+i) <= 0 Then
                                                           Return goto_end()
                                                       Else
                                                           b = New pointer(Of Boolean)()
                                                           ec = impl.full(b)
                                                           Return waitfor(ec) AndAlso
                                                                  goto_next()
                                                       End If
                                                   Else
                                                       Return False
                                                   End If
                                               End Function,
                                               Function() As Boolean
                                                   Return ec.end_result() AndAlso
                                                          (+b) AndAlso
                                                          goto_end()
                                               End Function)
                     End Function)
    End Function

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).append
        Return not_full_retry(Function() impl.append(key, value, ts, result))
    End Function

    Public Function capacity(ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).capacity
        Return retry(Function() impl.capacity(result))
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).delete
        Return retry(Function() impl.delete(key, result))
    End Function

    Public Function empty(ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).empty
        Return retry(Function() impl.empty(result))
    End Function

    Public Function full(ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).full
        Return retry(Function() impl.full(result))
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvt(Of _false).heartbeat
        Return event_comb.succeeded()
    End Function

    Public Function keycount(ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).keycount
        Return retry(Function() impl.keycount(result))
    End Function

    Public Function list(ByVal result As pointer(Of vector(Of Byte()))) As event_comb Implements ikeyvt(Of _false).list
        Return retry(Function() impl.list(result))
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).modify
        Return not_full_retry(Function() impl.modify(key, value, ts, result))
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal result As pointer(Of Byte()),
                         ByVal ts As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).read
        Return retry(Function() impl.read(key, result, ts))
    End Function

    Public Function retire() As event_comb Implements ikeyvt(Of _false).retire
        Return retry(Function() impl.retire())
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As pointer(Of Boolean)) As event_comb Implements ikeyvt(Of _false).seek
        Return retry(Function() impl.seek(key, result))
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).sizeof
        Return retry(Function() impl.sizeof(key, result))
    End Function

    Public Function [stop]() As event_comb Implements ikeyvt(Of _false).stop
        Return retry(Function() impl.stop())
    End Function

    Public Function unique_write(ByVal key() As Byte,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As pointer(Of Boolean)) As event_comb _
                                Implements ikeyvt(Of _false).unique_write
        Return not_full_retry(Function() impl.unique_write(key, value, ts, result))
    End Function

    Public Function valuesize(ByVal result As pointer(Of Int64)) As event_comb Implements ikeyvt(Of _false).valuesize
        Return retry(Function() impl.valuesize(result))
    End Function
End Class
