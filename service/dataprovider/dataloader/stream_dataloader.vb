
Imports System.IO
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.constants

Public MustInherit Class stream_dataloader(Of T)
    Implements idataloader(Of T)

    Private ReadOnly l As istreamdataloader(Of T)

    Protected Sub New(Optional ByVal l As istreamdataloader(Of T) = Nothing)
        Me.l = l
    End Sub

    Protected MustOverride Function create(ByVal localfile As String) As Stream

    Protected Overridable Function load(ByVal s As Stream, ByVal result As pointer(Of T)) As event_comb
        assert(Not l Is Nothing)
        Return l.load(s, result)
    End Function

    Public Function load(ByVal localfile As String,
                         ByVal result As pointer(Of T)) As event_comb Implements idataloader(Of T).load
        Dim s As Stream = Nothing
        Dim ec As event_comb = Nothing
        Dim create_retry As Int32 = 0
        create_retry = constants.stream_dataloader.create_retry
        Return New event_comb(Function() As Boolean
                                  Try
                                      s = create(localfile)
                                  Catch ex As Exception
                                      If create_retry > 0 Then
                                          create_retry -= 1
                                          Return waitfor(constants.stream_dataloader.retry_interval_ms)
                                      Else
                                          raise_error(error_type.exclamation,
                                                      "failed to create stream from ",
                                                      localfile,
                                                      ", ex ",
                                                      ex.Message())
                                          Return False
                                      End If
                                  End Try
                                  ec = load(s, result)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  s.Close()
                                  s.Dispose()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
