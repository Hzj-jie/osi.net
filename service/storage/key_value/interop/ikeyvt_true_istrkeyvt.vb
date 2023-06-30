
Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.utils
Imports osi.root.template

Friend Class ikeyvt_true_istrkeyvt
    Implements istrkeyvt

    Private ReadOnly impl As ikeyvt(Of _true)

    Public Sub New(ByVal impl As ikeyvt(Of _true))
        assert(Not impl Is Nothing)
        Me.impl = impl
    End Sub

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return impl.append(str_bytes(key), value, ts, result)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return impl.capacity(result)
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return impl.delete(str_bytes(key), result)
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
        Dim ec As event_comb = Nothing
        Dim r As ref(Of vector(Of Byte())) = Nothing
        Return New event_comb(Function() As Boolean
                                  r = New ref(Of vector(Of Byte()))()
                                  ec = impl.list(r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Dim rs As vector(Of String) = Nothing
                                      rs = New vector(Of String)()
                                      For i As Int64 = 0 To (+r).size() - 1
                                          rs.emplace_back(bytes_str((+r)(i)))
                                      Next
                                      Return eva(result, rs) AndAlso
                                             goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return impl.modify(str_bytes(key), value, ts, result)
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements istrkeyvt.read
        Return impl.read(str_bytes(key), result, ts)
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return impl.seek(str_bytes(key), result)
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return impl.sizeof(str_bytes(key), result)
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return impl.stop()
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return impl.unique_write(str_bytes(key), value, ts, result)
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return impl.valuesize(result)
    End Function
End Class
