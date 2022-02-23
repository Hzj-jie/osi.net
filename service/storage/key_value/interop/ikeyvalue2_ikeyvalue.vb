
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.constants
Imports osi.root.procedure
Imports osi.root.utils

Public Class ikeyvalue2_ikeyvalue(Of SEEK_RESULT)
    Implements ikeyvalue

    Private ReadOnly impl As ikeyvalue2(Of SEEK_RESULT)

    Public Sub New(ByVal impl As ikeyvalue2(Of SEEK_RESULT))
        assert(impl IsNot Nothing)
        Me.impl = impl
    End Sub

    Private Function if_existing(ByVal key() As Byte,
                                 ByVal existing_do As Func(Of SEEK_RESULT, event_comb),
                                 ByVal not_existing_do As Func(Of event_comb)) As event_comb
        assert(existing_do IsNot Nothing)
        assert(not_existing_do IsNot Nothing)
        Dim ec As event_comb = Nothing
        Dim sr As ref(Of SEEK_RESULT) = Nothing
        Dim r As ref(Of Boolean) = Nothing
        Return New event_comb(Function() As Boolean
                                  sr = New ref(Of SEEK_RESULT)()
                                  r = New ref(Of Boolean)()
                                  ec = impl.seek(key, sr, r)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+r) Then
                                          ec = existing_do(+sr)
                                      Else
                                          ec = not_existing_do()
                                      End If
                                      Return If(ec Is Nothing,
                                                goto_end(),
                                                waitfor(ec) AndAlso goto_next())
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.append
        Return if_existing(key,
                           Function(sr As SEEK_RESULT) As event_comb
                               Return impl.append_existing(sr, value, result)
                           End Function,
                           Function() As event_comb
                               Return impl.write_new(key, value, result)
                           End Function)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.capacity
        Return impl.capacity(result)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.delete
        Return if_existing(key,
                           Function(sr As SEEK_RESULT) As event_comb
                               Return impl.delete_existing(sr, result)
                           End Function,
                           Function() As event_comb
                               assert(eva(result, False))
                               Return Nothing
                           End Function)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.empty
        Return impl.empty(result)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.full
        Return impl.full(result)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvalue.heartbeat
        Return impl.heartbeat()
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.keycount
        Return impl.keycount(result)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb Implements ikeyvalue.list
        Return impl.list(result)
    End Function

    Private Function delete_then_write(ByVal sr As SEEK_RESULT,
                                       ByVal key() As Byte,
                                       ByVal value() As Byte,
                                       ByVal result As ref(Of Boolean)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If result Is Nothing Then
                                      result = New ref(Of Boolean)()
                                  End If
                                  ec = impl.delete_existing(sr, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso (+result) Then
                                      ec = impl.write_new(key, value, result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.modify
        Return if_existing(key,
                           Function(sr As SEEK_RESULT) As event_comb
                               Return delete_then_write(sr, key, value, result)
                           End Function,
                           Function() As event_comb
                               Return impl.write_new(key, value, result)
                           End Function)
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal value As ref(Of Byte())) As event_comb Implements ikeyvalue.read
        Return if_existing(key,
                           Function(sr As SEEK_RESULT) As event_comb
                               Return impl.read_existing(sr, value)
                           End Function,
                           Function() As event_comb
                               assert(eva(value, DirectCast(Nothing, Byte())))
                               Return Nothing
                           End Function)
    End Function

    Public Function retire() As event_comb Implements ikeyvalue.retire
        Return impl.retire()
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.seek
        Return impl.seek(key, New ref(Of SEEK_RESULT)(), result)
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.sizeof
        Return if_existing(key,
                           Function(sr As SEEK_RESULT) As event_comb
                               Return impl.sizeof_existing(sr, result)
                           End Function,
                           Function() As event_comb
                               assert(eva(result, npos))
                               Return Nothing
                           End Function)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvalue.stop
        Return impl.stop()
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.valuesize
        Return impl.valuesize(result)
    End Function
End Class
