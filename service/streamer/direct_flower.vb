
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.transmitter

Public Class direct_flower(Of T)
    Inherits flower(Of T)

    Private ReadOnly input As T_receiver(Of T)
    Private ReadOnly output As T_sender(Of T)
    Private ReadOnly sense_timeout_ms As Int64
    Private ReadOnly broken_pipe As pointer(Of singleentry)
    Private ReadOnly idle_timeout_ms As Int64
    Private ReadOnly result As atomic_int64
    Private ReadOnly treat_no_flow_as_failure As Boolean

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal sense_timeout_ms As Int64,
                   ByVal broken_pipe As pointer(Of singleentry),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        MyBase.New()
        assert(Not input Is Nothing)
        assert(Not output Is Nothing)
        Me.input = input
        Me.output = output
        If sense_timeout_ms < 0 Then
            Me.sense_timeout_ms = constants.default_sense_timeout_ms
        Else
            Me.sense_timeout_ms = sense_timeout_ms
        End If
        If broken_pipe Is Nothing Then
            Me.broken_pipe = New pointer(Of singleentry)()
        Else
            Me.broken_pipe = broken_pipe
        End If
        If idle_timeout_ms < 0 Then
            Me.idle_timeout_ms = max_int64
        ElseIf idle_timeout_ms < (sense_timeout_ms << 1) Then
            Me.idle_timeout_ms = (sense_timeout_ms << 1)
        Else
            Me.idle_timeout_ms = idle_timeout_ms
        End If
        Me.result = result
        Me.treat_no_flow_as_failure = treat_no_flow_as_failure
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal sense_timeout_ms As Int64,
                   ByVal broken_pipe As pointer(Of singleentry),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, sense_timeout_ms, broken_pipe, npos, result, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal broken_pipe As pointer(Of singleentry),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input,
               output,
               constants.default_sense_timeout_ms,
               broken_pipe,
               idle_timeout_ms,
               result,
               treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal broken_pipe As pointer(Of singleentry),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, broken_pipe, npos, result, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   ByVal idle_timeout_ms As Int64,
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, Nothing, idle_timeout_ms, result, treat_no_flow_as_failure)
    End Sub

    Public Sub New(ByVal input As T_receiver(Of T),
                   ByVal output As T_sender(Of T),
                   Optional ByVal result As atomic_int64 = Nothing,
                   Optional ByVal treat_no_flow_as_failure As Boolean = True)
        Me.New(input, output, npos, result, treat_no_flow_as_failure)
    End Sub

    Public Overrides Function [stop]() As Boolean
        Return broken_pipe.mark_in_use()
    End Function

    Protected Overridable Shadows Function is_eos(ByVal i As T) As Boolean
        Return flower(Of T).is_eos(i)
    End Function

    Private Function flow_once(ByVal sense_timeout_ms As Int64,
                               ByVal transfered As pointer(Of Boolean),
                               ByVal eos As pointer(Of Boolean)) As event_comb
        Dim ec As event_comb = Nothing
        Dim r As pointer(Of Boolean) = Nothing
        Dim p As pointer(Of T) = Nothing
        Return New event_comb(Function() As Boolean
                                  eva(transfered, False)
                                  eva(eos, False)
                                  r = New pointer(Of Boolean)()
                                  ec = input.sense(r, sense_timeout_ms)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+r) Then
                                          p = New pointer(Of T)()
                                          ec = input.receive(p)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If is_eos(+p) Then
                                          Return eva(eos, True) AndAlso
                                                 goto_end()
                                      Else
                                          ec = output.send(+p)
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  If Not result Is Nothing Then
                                      result.add(1)
                                  End If
                                  Return ec.end_result() AndAlso
                                         eva(transfered, True) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Function flow_once(ByVal transfered As pointer(Of Boolean),
                               ByVal eos As pointer(Of Boolean)) As event_comb
        Return flow_once(sense_timeout_ms, transfered, eos)
    End Function

    Protected Overrides Function flow() As event_comb
        Dim ec As event_comb = Nothing
        Dim transfered As pointer(Of Boolean) = Nothing
        Dim transfered_times As UInt32 = 0
        Dim eos As pointer(Of Boolean) = Nothing
        Dim last_active_ms As Int64 = 0
        Return New event_comb(Function() As Boolean
                                  transfered = New pointer(Of Boolean)()
                                  eos = New pointer(Of Boolean)()
                                  last_active_ms = nowadays.milliseconds()
                                  Return goto_next()
                              End Function,
                              Function() As Boolean
                                  ec = flow_once(transfered, eos)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If broken_pipe.in_use() Then
                                          Return goto_next()
                                      ElseIf (+eos) OrElse
                                             idle_timeout_ms <= nowadays.milliseconds() - last_active_ms Then
                                          broken_pipe.mark_in_use()
                                          Return (transfered_times > 0 OrElse
                                                  Not treat_no_flow_as_failure) AndAlso
                                                 goto_end()
                                      ElseIf (+transfered) Then
                                          transfered_times += uint32_1
                                          last_active_ms = nowadays.milliseconds()
                                      End If
                                      Return goto_prev()
                                  Else
                                      broken_pipe.mark_in_use()
                                      Return False
                                  End If
                              End Function,
                              Function() As Boolean
                                  transfered.renew()
                                  ec = flow_once(0, transfered, Nothing)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If ec.end_result() Then
                                      If (+transfered) Then
                                          transfered_times += uint32_1
                                          Return goto_prev()
                                      Else
                                          Return (transfered_times > 0 OrElse
                                                  Not treat_no_flow_as_failure) AndAlso
                                                 goto_end()
                                      End If
                                  Else
                                      Return False
                                  End If
                              End Function)
    End Function
End Class
