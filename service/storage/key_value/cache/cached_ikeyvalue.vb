
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.constants
Imports osi.service.cache

Partial Friend Class cached_ikeyvalue
    Implements ikeyvalue

    Private ReadOnly impl As ikeyvalue
    Private ReadOnly cache As caches

    Public Sub New(ByVal impl As ikeyvalue,
                   ByVal cached_count As UInt64,
                   ByVal max_value_size As UInt64)
        assert(impl IsNot Nothing)
        Me.impl = impl
        Me.cache = New caches(cached_count, max_value_size)
    End Sub

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.append
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If result Is Nothing Then
                                      result = New ref(Of Boolean)()
                                  End If
                                  ec = impl.append(key, value, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cache.append_set(key, value, +result)
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.capacity
        Return impl.capacity(result)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.delete
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If cache.havenot(key) Then
                                      Return eva(result, False) AndAlso
                                             goto_end()
                                  Else
                                      If result Is Nothing Then
                                          result = New ref(Of Boolean)()
                                      End If
                                      ec = impl.delete(key, result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cache.delete_set(key, +result)
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.empty
        Return impl.empty(result)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.full
        Return impl.full(result)
    End Function

    Public Function heartbeat() As event_comb Implements ikeyvalue.heartbeat
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = impl.heartbeat()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cache.heartbeat_set()
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.keycount
        Return impl.keycount(result)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As event_comb Implements ikeyvalue.list
        Return impl.list(result)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.modify
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If result Is Nothing Then
                                      result = New ref(Of Boolean)()
                                  End If
                                  ec = impl.modify(key, value, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cache.modify_set(key, value, +result)
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal value As ref(Of Byte())) As event_comb Implements ikeyvalue.read
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim t() As Byte = Nothing
                                  If cache.read_get(key, t) Then
                                      Return eva(value, t) AndAlso
                                             goto_end()
                                  Else
                                      If value Is Nothing Then
                                          value = New ref(Of Byte())()
                                      End If
                                      ec = impl.read(key, value)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cache.read_set(key, +value)
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function retire() As event_comb Implements ikeyvalue.retire
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = impl.retire()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cache.retire_set()
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As ref(Of Boolean)) As event_comb Implements ikeyvalue.seek
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim t As Boolean = False
                                  If cache.seek_get(key, t) Then
                                      Return eva(result, t) AndAlso
                                             goto_end()
                                  Else
                                      If result Is Nothing Then
                                          result = New ref(Of Boolean)()
                                      End If
                                      ec = impl.seek(key, result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cache.seek_set(key, +result)
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.sizeof
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim t As Int64 = 0
                                  If cache.sizeof_get(key, t) Then
                                      Return eva(result, t) AndAlso
                                             goto_end()
                                  Else
                                      If result Is Nothing Then
                                          result = New ref(Of Int64)()
                                      End If
                                      ec = impl.sizeof(key, result)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      cache.sizeof_set(key, +result)
                                      Return goto_end()
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function

    Public Function [stop]() As event_comb Implements ikeyvalue.stop
        Return impl.stop()
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements ikeyvalue.valuesize
        Return impl.valuesize(result)
    End Function
End Class
