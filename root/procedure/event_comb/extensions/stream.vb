
Imports System.Runtime.CompilerServices
Imports System.IO
Imports System.Net.Sockets
Imports osi.root.template
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utils
Imports osi.root.formation
Imports osi.root.envs

Public Module _stream
    <Extension()> Public Function send(ByVal s As Stream,
                                       ByVal buff() As Byte,
                                       ByVal offset As UInt32,
                                       ByVal count As UInt32,
                                       Optional ByVal rate_sec As UInt32 = 0,
                                       Optional ByVal close_when_finish As Boolean = False,
                                       Optional ByVal close_when_fail As Boolean = True) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If s Is Nothing OrElse
                                     Not s.can_write() Then
                                      Return False
                                  ElseIf array_size(buff) < offset + count Then
                                      Return False
                                  Else
                                      ec = New event_comb_async_operation(
                                               Function(ac As AsyncCallback) As IAsyncResult
                                                   Return s.BeginWrite(buff,
                                                                       CInt(offset),
                                                                       CInt(count),
                                                                       ac,
                                                                       Nothing)
                                               End Function,
                                               Sub(ar As IAsyncResult)
                                                   s.EndWrite(ar)
                                               End Sub)
                                      Return waitfor(ec, rate_to_ms(rate_sec, count)) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If close_when_finish Then
                                      s.Close()
                                      s.Dispose()
                                  ElseIf close_when_fail AndAlso
                                         Not ec.end_result() Then
                                      'last failure shows error
                                      Try
                                          s.Close()
                                          s.Dispose()
                                      Catch
                                      End Try
                                  End If
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    <Extension()> Public Function receive(ByVal s As Stream,
                                          ByVal buff() As Byte,
                                          ByVal offset As UInt32,
                                          ByVal count As UInt32,
                                          ByVal result As ref(Of UInt32),
                                          Optional ByVal rate_sec As UInt32 = 0,
                                          Optional ByVal close_when_finish As Boolean = False,
                                          Optional ByVal close_when_fail As Boolean = True) As event_comb
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  If s Is Nothing OrElse
                                     Not s.can_read() Then
                                      Return False
                                  ElseIf array_size(buff) < offset + count Then
                                      Return False
                                  Else
                                      ec = event_comb_async_operation.ctor(
                                               Function(ac As AsyncCallback) As IAsyncResult
                                                   Return s.BeginRead(buff,
                                                                      offset,
                                                                      count,
                                                                      ac,
                                                                      Nothing)
                                               End Function,
                                               Function(ar As IAsyncResult) As UInt32
                                                   Return CUInt(s.EndRead(ar))
                                               End Function,
                                               result)
                                      Return waitfor(ec, rate_to_ms(rate_sec, count)) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  If close_when_finish OrElse
                                     (close_when_fail AndAlso
                                      Not ec.end_result()) Then
                                      s.Close()
                                      s.Dispose()
                                  End If
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Module
