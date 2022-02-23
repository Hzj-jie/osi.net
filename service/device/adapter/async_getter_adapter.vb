
Option Strict On

Imports System.Threading
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.selector

' Consumers should only use this class to create a new interface, say, async_getter_herald.
Public Class async_getter_adapter
    Public Shared Function [New](Of IT, OT)(ByVal i As async_getter(Of IT),
                                            ByVal c As Func(Of IT, OT)) As async_getter_adapter(Of OT)
        Return async_getter_adapter(Of OT).[New](i, c)
    End Function

    Public Shared Function new_async_getter(Of IT, OT)(ByVal i As async_getter(Of IT),
                                                       ByVal c As Func(Of IT, OT)) As async_getter(Of OT)
        Return [New](i, c)
    End Function

    Private Sub New()
    End Sub
End Class

Public Class async_getter_adapter(Of T)
    Implements type_attribute.signal, async_getter(Of T)

    Private ReadOnly dev As async_getter(Of T)
    Private ReadOnly attr As type_attribute

    Protected Sub New(ByVal p As pair(Of async_getter(Of T), type_attribute))
        assert(p IsNot Nothing)
        assert(p.first IsNot Nothing)
        assert(p.second IsNot Nothing)
        Me.dev = p.first
        Me.attr = p.second
    End Sub

    Public Shared Function [New](Of IT)(ByVal i As async_getter(Of IT),
                                        ByVal c As Func(Of IT, T)) As async_getter_adapter(Of T)
        Return New async_getter_adapter(Of T)(convert(i, c))
    End Function

    Private Shared Function find_type_attribute(Of IT, OT As T)(ByVal i As async_getter(Of IT),
                                                                ByVal c As Func(Of IT, OT)) As type_attribute
        If Not type_info(Of OT, type_info_operators.equal, T).v AndAlso type_attribute.has(Of OT)() Then
            Return type_attribute.of(Of OT)()
        ElseIf type_attribute.has(Of IT)() Then
            Return type_attribute.of(Of IT)()
        ElseIf type_attribute.has(i) Then
            Return type_attribute.of(i)
        ElseIf type_attribute.has(c) Then
            Return type_attribute.of(c)
        Else
            assert(False)
            Return Nothing
        End If
    End Function

    Protected Shared Function convert(Of IT, OT As T) _
                                     (ByVal i As async_getter(Of IT),
                                      ByVal c As Func(Of IT, OT)) _
                                     As pair(Of async_getter(Of T), type_attribute)
        assert(i IsNot Nothing)
        assert(c IsNot Nothing)
        Dim p As ref(Of IT) = Nothing
        Dim ec As event_comb = Nothing
        Return pair.emplace_of(Of async_getter(Of T), type_attribute)(
                   New async_thread_unsafe_lazier(Of T)(
                       Function(r As ref(Of T)) As event_comb
                           Return New event_comb(Function() As Boolean
                                                     p = New ref(Of IT)()
                                                     ec = i.get(p)
                                                     Return waitfor(ec) AndAlso
                                                            goto_next()
                                                 End Function,
                                                 Function() As Boolean
                                                     If ec.end_result() AndAlso Not p.empty() Then
                                                         Return eva(r, c(+p)) AndAlso
                                                                goto_end()
                                                     Else
                                                         Return False
                                                     End If
                                                 End Function)
                       End Function),
                   find_type_attribute(Of IT, OT)(i, c))
    End Function

    Private Shared Function find_type_attribute(Of OT As T)(ByVal i As async_getter(Of OT)) As type_attribute
        If Not type_info(Of T, type_info_operators.equal, OT).v AndAlso type_attribute.has(Of OT)() Then
            Return type_attribute.of(Of OT)()
        ElseIf type_attribute.has(i) Then
            Return type_attribute.of(i)
        Else
            assert(False)
            Return Nothing
        End If
    End Function

    Protected Shared Function convert(Of OT As T)(ByVal i As async_getter(Of OT)) _
                                                 As pair(Of async_getter(Of T), type_attribute)
        Return pair.emplace_of(Of async_getter(Of T), type_attribute)(
                   New async_getter_downgrader(Of T, OT)(i), find_type_attribute(Of OT)(i))
    End Function

    Protected Shared Function convert(ByVal i As async_getter(Of T)) As pair(Of async_getter(Of T), type_attribute)
        Return pair.emplace_of(Of async_getter(Of T), type_attribute)(i, type_attribute.of(i))
    End Function

    Public Function _do(ByVal d As Func(Of T, event_comb)) As event_comb
        assert(d IsNot Nothing)
        Dim ec As event_comb = Nothing
        Dim p As ref(Of T) = Nothing
        Return New event_comb(Function() As Boolean
                                  p = New ref(Of T)()
                                  ec = dev.get(p)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() AndAlso Not p.empty() Then
                                      ec = d(+p)
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

    Public Function attribute() As type_attribute Implements type_attribute.signal.attribute
        Return attr
    End Function

    Public Function alive() As ternary Implements async_getter(Of T).alive
        Return dev.alive()
    End Function

    Public Function [get](ByVal r As ref(Of T)) As event_comb Implements async_getter(Of T).get
        Return dev.get(r)
    End Function

    Public Function [get](ByRef r As T) As Boolean Implements async_getter(Of T).get
        Return dev.get(r)
    End Function

    Public Function initialized_wait_handle() As WaitHandle Implements async_getter(Of T).initialized_wait_handle
        Return dev.initialized_wait_handle()
    End Function
End Class
