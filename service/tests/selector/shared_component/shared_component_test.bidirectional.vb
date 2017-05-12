﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.selector

Partial Public Class shared_component_test
    Private Class bidirectional_test
        Inherits [case]

        Private Const iteration_count As Int32 = 100
        Private ReadOnly ic As collection
        Private ReadOnly oc As collection
        Private ReadOnly f As functor
        Private ReadOnly ins As vector(Of shared_component(Of Byte, Byte, component, Int32, parameter))
        Private ReadOnly outs As vector(Of shared_component(Of Byte, Byte, component, Int32, parameter))
        Private ReadOnly inp As parameter
        Private ReadOnly outp As parameter

        Public Sub New()
            MyBase.New()
            ic = New collection()
            oc = New collection()
            f = New functor()
            ins = _new(ins)
            outs = _new(outs)
            While inp Is Nothing OrElse
                  Not component.is_valid_port(inp.local_port)
                inp = New parameter(rnd_uint8(), True)
            End While
            While outp Is Nothing OrElse
                  Not component.is_valid_port(outp.local_port) OrElse
                  outp.local_port = inp.local_port
                outp = New parameter(rnd_uint8(), False)
            End While
        End Sub

        Private Shared Function execute_receive_then_send(
                ByVal sc As shared_component(Of Byte, Byte, component, Int32, parameter),
                ByVal first As Int32,
                ByVal self_increment As Boolean) As event_comb
            assert(Not sc Is Nothing)
            Dim ec As event_comb = Nothing
            Dim p As pointer(Of Int32) = Nothing
            Dim c As Int32 = 0
            Return New event_comb(Function() As Boolean
                                      p = New pointer(Of Int32)()
                                      ec = sc.receiver.receive(p)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If assert_true(ec.end_result()) Then
                                          assert_equal((+p), first + c)
                                          If self_increment Then
                                              If c = iteration_count - 1 Then
                                                  Return goto_end()
                                              End If
                                              ec = sc.sender.send((+p) + 1)
                                          Else
                                              ec = sc.sender.send(+p)
                                          End If
                                          Return waitfor(ec) AndAlso
                                                 goto_next()
                                      Else
                                          Return assert_true(False)
                                      End If
                                  End Function,
                                  Function() As Boolean
                                      If assert_true(ec.end_result()) Then
                                          c += 1
                                          If c = iteration_count Then
                                              Return goto_end()
                                          Else
                                              Return goto_begin()
                                          End If
                                      Else
                                          Return assert_true(False)
                                      End If
                                  End Function)
        End Function

        Private Shared Function execute_receive_then_send_next(
                ByVal sc As shared_component(Of Byte, Byte, component, Int32, parameter),
                ByVal first As Int32) As event_comb
            Return execute_receive_then_send(sc, first, True)
        End Function

        Private Shared Function execute_receive_then_send(
                ByVal sc As shared_component(Of Byte, Byte, component, Int32, parameter),
                ByVal first As Int32) As event_comb
            Return execute_receive_then_send(sc, first, False)
        End Function

        Private Shared Function execute_send_receive(
            ByVal sender As shared_component(Of Byte, Byte, component, Int32, parameter),
            ByVal receiver As shared_component(Of Byte, Byte, component, Int32, parameter),
            ByVal first As Int32) As event_comb
            Dim ec1 As event_comb = Nothing
            Dim ec2 As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      ec1 = execute_receive_then_send(receiver, first)
                                      ec2 = execute_receive_then_send_next(sender, first)
                                      Return waitfor(ec1, ec2) AndAlso
                                             goto_next()
                                  End Function,
                                  Function() As Boolean
                                      Return assert_true(ec1.end_result()) AndAlso
                                             assert_true(ec2.end_result()) AndAlso
                                             goto_end()
                                  End Function)
        End Function

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                ins.clear()
                outs.clear()
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            Dim inc As ref_instance(Of component) = Nothing
            Dim ind As dispenser(Of Int32, const_pair(Of Byte, Byte)) = Nothing
            AddHandler ic.new_shared_component_exported,
                       Sub(ByVal sc As shared_component(Of Byte, Byte, component, Int32, parameter))
                           assert_not_nothing(sc)
                           assert_true(sc.is_valid())
                           ins.emplace_back(sc)
                       End Sub
            If Not assert_true(ic.[New](inp, inp.local_port, inc)) OrElse
               Not assert_true(ic.[New](inp, inp.local_port, inc, ind)) Then
                Return False
            End If
            If Not assert_happening(AddressOf inc.referred) Then
                Return False
            End If
            If Not assert_equal((+inc).local_port, inp.local_port) Then
                Return False
            End If
            functor.address += uint8_1
            Const connection_count As Int32 = max_uint8 - min_uint8 - uint8_1 - uint8_2
            Dim ecs() As event_comb = Nothing
            ReDim ecs(connection_count - uint8_1)
            ' Remove two invalid ports
            For i As Int32 = 0 To connection_count - 1
                Dim j As Int32 = 0
                j = i
                Dim first As Int32 = 0
                first = iteration_count * i
                Dim sc As shared_component(Of Byte, Byte, component, Int32, parameter) = Nothing
                sc = shared_component(Of Byte, Byte, component, Int32, parameter).creator.
                         [New]().
                         with_parameter(outp).
                         with_remote(make_const_pair((+inc).address, inp.local_port)).
                         with_collection(oc).
                         with_functor(f).
                         create()
                If Not assert_true(sc.is_valid()) Then
                    Return False
                End If
                outs.emplace_back(sc)
                If Not assert_true(async_sync(sc.sender.send(first))) Then
                    Return False
                End If
                If Not assert_happening(Function() ins.size() = CUInt(j - min_uint8 + 1)) Then
                    Return False
                End If
                ecs(i) = execute_send_receive(sc, ins(CUInt(i)), first)
            Next
            If Not assert_equal(ins.size(), outs.size()) Then
                Return False
            End If
            assert_true(async_sync(New event_comb(Function() As Boolean
                                                      Return waitfor(ecs) AndAlso
                                                             goto_end()
                                                  End Function,
                                                  Function() As Boolean
                                                      Return ecs.end_result() AndAlso
                                                             goto_end()
                                                  End Function)))
            For i As UInt32 = 0 To ins.size() - uint32_1
                ins(i).dispose()
                outs(i).dispose()
            Next
            assert_equal(ind.binding_count(), uint32_1)
            assert_true(ind.release())
            assert_false(ind.binding())
            ' Ensure all dispensers are stopped.
            sleep(constants.default_sense_timeout_ms * 4)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            ins.clear()
            outs.clear()
            Return MyBase.finish()
        End Function
    End Class
End Class
