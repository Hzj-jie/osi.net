
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.argument
Imports osi.service.device

<global_init(global_init_level.server_services)>
Public Class balance_distributor
    Implements istrkeyvt

    Private Const default_update_frequency As Int64 = 30 * second_milli
    Private ReadOnly expired As ref(Of singleentry)
    Private ReadOnly container As istrkeyvt_container
    Private ReadOnly capacities As vector(Of Int64)
    Private ReadOnly update_frequency As Int32

    Shared Sub New()
        assert(npos < 0)
    End Sub

    Public Sub New(ByVal c As istrkeyvt_container,
                   Optional ByVal update_frequency As Int32 = npos)
        assert(Not c Is Nothing)
        Me.container = c
        Me.capacities = New vector(Of Int64)()
        Me.capacities.resize(Me.container.size())
        Me.expired = New ref(Of singleentry)()
        If update_frequency <= 0 Then
            raise_error(error_type.user,
                        "input update frequency ",
                        update_frequency,
                        " is not valid, use default value as ",
                        default_update_frequency)
            Me.update_frequency = default_update_frequency
        Else
            Me.update_frequency = update_frequency
        End If
        start()
    End Sub

    Public Sub New(ByVal names() As String,
                   ByVal update_frequency As Int32)
        Me.New(istrkeyvt_container.ctor(names), update_frequency)
    End Sub

    Public Sub New(ByVal ParamArray names() As String)
        Me.New(names, npos)
    End Sub

    Private Sub update_capacity(ByVal i As UInt32)
        Dim ec As event_comb = Nothing
        Dim p As ref(Of Int64) = Nothing
        begin_lifetime_event_comb(expired,
                                  Function() As Boolean
                                      p = New ref(Of Int64)()
                                      Return goto_next()
                                  End Function,
                                  Function() As Boolean
                                      ec = container(CInt(i)).capacity(p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() AndAlso (+p) >= 0 Then
                                          assert(capacities.size() = container.size())
                                          capacities(i) = (+p)
                                      Else
                                          raise_error(error_type.warning,
                                                      "failed to get capacity information from ",
                                                      container.name(i),
                                                      ":",
                                                      i,
                                                      ", wait for next round")
                                      End If
                                      Return waitfor(update_frequency) AndAlso
                                             goto_prev()
                                  End Function)
    End Sub

    Private Sub start()
        Dim i As UInt32 = 0
        While i < container.size()
            update_capacity(i)
            i += uint32_1
        End While
    End Sub

    Private Function select_by_key(ByVal key As String,
                                   ByVal o As ref(Of vector(Of istrkeyvt))) As event_comb
        Return distribute(container,
                          Function(keyvt As istrkeyvt, existing As ref(Of Boolean)) As event_comb
                              Return keyvt.seek(key, existing)
                          End Function,
                          o,
                          Function(existings() As ref(Of Boolean),
                                   ByRef result As vector(Of istrkeyvt)) As Boolean
                              assert(array_size(existings) = container.size())
                              result = New vector(Of istrkeyvt)()
                              For i As Int32 = 0 To array_size_i(existings) - 1
                                  If +(existings(i)) Then
                                      result.push_back(container(i))
                                  End If
                              Next
                              Return Not result.empty()
                          End Function)
    End Function

    Private Function select_by_key(ByVal key As String,
                                   ByVal o As ref(Of istrkeyvt)) As event_comb
        'if more than one istrkeyvts contain the key, just select the first one.
        'this should not happen
        Dim ec As event_comb = Nothing
        Dim p As ref(Of vector(Of istrkeyvt)) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of vector(Of istrkeyvt))()
                                  ec = select_by_key(key, p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         Not (+p) Is Nothing AndAlso
                                         Not (+p).empty() AndAlso
                                         eva(o, (+p)(0)) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function select_by_key(ByVal key As String,
                                   ByVal d As Func(Of istrkeyvt, event_comb),
                                   Optional ByVal ud As Action = Nothing) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Dim i As ref(Of istrkeyvt) = Nothing
        Return New event_comb(Function() As Boolean
                                  i = New ref(Of istrkeyvt)()
                                  ec = select_by_key(key, i)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      assert(Not (+i) Is Nothing)
                                      ec = d(+i)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      If Not ud Is Nothing Then
                                          ud()
                                      End If
                                      Return goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function select_by_capacity(ByRef o As istrkeyvt) As Boolean
        assert(capacities.size() = container.size())
        Dim max As Int64 = 0
        Dim max_id As Int32 = 0
        max_id = npos
        Dim i As UInt32 = 0
        While i < capacities.size()
            If capacities(CUInt(i)) > max Then
                max = capacities(CUInt(i))
                max_id = CInt(i)
            End If
            i += uint32_1
        End While
        If max_id >= 0 Then
            o = container(max_id)
            Return True
        End If
        Return False
    End Function

    Private Function [select](ByVal key As String,
                              ByVal o As ref(Of istrkeyvt)) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = select_by_key(key, o)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      Return goto_end()
                                  Else
                                      Dim i As istrkeyvt = Nothing
                                      Return select_by_capacity(i) AndAlso
                                             eva(o, i) AndAlso
                                             goto_end()
                                  End If
                              End Function)
    End Function

    Private Function [select](ByVal key As String,
                              ByVal d As Func(Of istrkeyvt, event_comb)) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Dim i As ref(Of istrkeyvt) = Nothing
        Return New event_comb(Function() As Boolean
                                  i = New ref(Of istrkeyvt)()
                                  ec = [select](key, i)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      assert(Not (+i) Is Nothing)
                                      ec = d(+i)
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

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.append
        Return [select](key, Function(i) i.append(key, value, ts, result))
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.capacity
        Return sum_capacity(container, result)
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.delete
        Return all_delete(container, key, result)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.empty
        Return and_empty(container, result)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.full
        Return or_full(container, result)
    End Function

    Public Function heartbeat() As event_comb Implements istrkeyvt.heartbeat
        Return all_heartbeat(container)
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.keycount
        Return sum_keycount(container, result)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of String))) As event_comb Implements istrkeyvt.list
        Return merge_list(container, result)
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.modify
        Return [select](key, Function(i) i.modify(key, value, ts, result))
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As event_comb Implements istrkeyvt.read
        Return select_by_key(key, Function(x) x.read(key, result, ts))
    End Function

    Public Function retire() As event_comb Implements istrkeyvt.retire
        Return all_retire(container)
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.seek
        Return select_by_key(key,
                             Function(x As istrkeyvt) As event_comb
                                 eva(result, True)
                                 Return event_comb.succeeded()
                             End Function,
                             Sub()
                                 eva(result, False)
                             End Sub)
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.sizeof
        Return select_by_key(key, Function(x) x.sizeof(key, result))
    End Function

    Public Function [stop]() As event_comb Implements istrkeyvt.stop
        Return sync_async(Function() As Boolean
                              Return expired.mark_in_use()
                          End Function)
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As event_comb Implements istrkeyvt.unique_write
        Return [select](key, Function(x) x.unique_write(key, value, ts, result))
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As event_comb Implements istrkeyvt.valuesize
        Return sum_valuesize(container, result)
    End Function

    Public Shared Function create(ByVal v As var, ByRef o As balance_distributor) As Boolean
        Const update_frequence As String = "update-frequence"
        If v Is Nothing Then
            Return False
        End If
        Dim c As istrkeyvt_container = Nothing
        If Not istrkeyvt_container.create(v, c) Then
            Return False
        End If
        v.bind(update_frequence)
        Dim uf As Int32 = 0
        If v.value(update_frequence, uf) Then
            o = New balance_distributor(c, uf)
        Else
            o = New balance_distributor(c)
        End If
        Return True
    End Function

    Private Shared Function create(ByVal v As var, ByRef o As istrkeyvt) As Boolean
        Dim r As balance_distributor = Nothing
        If Not create(v, r) Then
            Return False
        End If
        o = r
        Return True
    End Function

    Private Shared Sub init()
        assert(constructor.register(Of istrkeyvt)("balance-distributor", AddressOf create))
    End Sub
End Class
