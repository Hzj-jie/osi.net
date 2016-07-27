
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.service.commander
Imports osi.service.commander.constants
Imports osi.service.storage.constants.remote
Imports parameter = osi.service.storage.constants.remote.parameter

Partial Public Class istrkeyvt_dispatcher
    Private Shared Function a_strkeyvt(ByVal strkeyvt As istrkeyvt) As istrkeyvt
        Return assert_return(Not strkeyvt Is Nothing, strkeyvt)
    End Function

    Private Shared Function handle(ByVal i As command,
                                   ByVal o As command,
                                   ByVal d As Func(Of event_comb)) As event_comb
        assert(Not d Is Nothing)
        Dim ec As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ec = d()
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  If Not ec.end_result() Then
                                      o.attach(response.failure)
                                  End If
                                  Return goto_end()
                              End Function)
    End Function

    Private Shared Function handle(ByVal i As command,
                                   ByVal o As command,
                                   ByVal p As Func(Of Boolean),
                                   ByVal d As Func(Of event_comb),
                                   ByVal r As Func(Of Boolean)) As event_comb
        Return handle(i,
                      o,
                      Function() As event_comb
                          assert(Not d Is Nothing)
                          Dim ec As event_comb = Nothing
                          Return New event_comb(Function() As Boolean
                                                    If p Is Nothing OrElse p() Then
                                                        ec = d()
                                                        Return waitfor(ec) AndAlso
                                                               goto_next()
                                                    Else
                                                        o.attach(response.invalid_request)
                                                        Return goto_end()
                                                    End If
                                                End Function,
                                                Function() As Boolean
                                                    If ec.end_result() AndAlso (r Is Nothing OrElse r()) Then
                                                        o.attach(response.success)
                                                        Return goto_end()
                                                    Else
                                                        Return False
                                                    End If
                                                End Function)
                      End Function)
    End Function

    Private Shared Function amuw(ByVal i As command,
                                 ByVal o As command,
                                 ByVal d As Func(Of String,
                                                    Byte(), 
                                                    Int64, 
                                                    pointer(Of Boolean), 
                                                    event_comb)) As event_comb
        assert(Not d Is Nothing)
        Dim key As String = Nothing
        Dim buff() As Byte = Nothing
        Dim ts As Int64 = 0
        Dim result As pointer(Of Boolean) = Nothing
        Return handle(i,
                      o,
                      Function() As Boolean
                          Return parse_key_buff_timestamp(i, key, buff, ts)
                      End Function,
                      Function() As event_comb
                          result = New pointer(Of Boolean)()
                          Return d(key, buff, ts, result)
                      End Function,
                      Function() As Boolean
                          o.attach(parameter.result, +result)
                          Return True
                      End Function)
    End Function

    Private Shared Function key_handle(ByVal i As command,
                                       ByVal o As command,
                                       ByVal d As Func(Of String, 
                                                          event_comb),
                                       ByVal r As Func(Of Boolean)) As event_comb
        assert(Not d Is Nothing)
        Dim key As String = Nothing
        Return handle(i,
                      o,
                      Function() As Boolean
                          Return i.parameter(parameter.key, key)
                      End Function,
                      Function() As event_comb
                          Return d(key)
                      End Function,
                      r)
    End Function

    Private Shared Function result_handle(ByVal i As command,
                                          ByVal o As command,
                                          ByVal p As Func(Of Boolean),
                                          ByVal d As Func(Of pointer(Of Boolean), 
                                                             event_comb)) As event_comb
        assert(Not d Is Nothing)
        assert(Not d Is Nothing)
        Dim r As pointer(Of Boolean) = Nothing
        Return handle(i,
                      o,
                      p,
                      Function() As event_comb
                          r = New pointer(Of Boolean)()
                          Return d(r)
                      End Function,
                      Function() As Boolean
                          o.attach(parameter.result, +r)
                          Return True
                      End Function)
    End Function

    Private Shared Function size_handle(ByVal i As command,
                                        ByVal o As command,
                                        ByVal p As Func(Of Boolean),
                                        ByVal d As Func(Of pointer(Of Int64), 
                                                           event_comb)) As event_comb
        assert(Not d Is Nothing)
        assert(Not d Is Nothing)
        Dim r As pointer(Of Int64) = Nothing
        Return handle(i,
                      o,
                      p,
                      Function() As event_comb
                          r = New pointer(Of Int64)()
                          Return d(r)
                      End Function,
                      Function() As Boolean
                          o.attach(parameter.size, +r)
                          Return True
                      End Function)
    End Function

    Private Shared Function key_result_handle(ByVal i As command,
                                              ByVal o As command,
                                              ByVal d As Func(Of String, 
                                                                 pointer(Of Boolean), 
                                                                 event_comb)) As event_comb
        assert(Not d Is Nothing)
        Dim key As String = Nothing
        Dim r As pointer(Of Boolean) = Nothing
        Return handle(i,
                      o,
                      Function() As Boolean
                          Return i.parameter(parameter.key, key)
                      End Function,
                      Function() As event_comb
                          r = New pointer(Of Boolean)()
                          Return d(key, r)
                      End Function,
                      Function() As Boolean
                          o.attach(parameter.result, +r)
                          Return True
                      End Function)
    End Function

    Private Shared Function parse_key_buff_timestamp(ByVal i As command,
                                                     ByRef key As String,
                                                     ByRef buff() As Byte,
                                                     ByRef ts As Int64) As Boolean
        If i Is Nothing Then
            Return False
        Else
            Return i.parameter(parameter.key, key) AndAlso
                   Not String.IsNullOrEmpty(key) AndAlso
                   i.parameter(parameter.buff, buff) AndAlso
                   Not isemptyarray(buff) AndAlso
                   If(i.parameter(parameter.timestamp, ts), ts >= 0, eva(ts, now_as_timestamp()))
        End If
    End Function
End Class
