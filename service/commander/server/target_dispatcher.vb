
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.service.device

Public NotInheritable Class target_dispatcher(Of T)
    Private ReadOnly dispatcher As dispatcher

    Public Sub New(ByVal dispatcher As dispatcher)
        assert(Not dispatcher Is Nothing)
        Me.dispatcher = dispatcher
    End Sub

    Public Sub New()
        Me.New(dispatcher.default)
    End Sub

    Private Shared Function handle(ByVal i As command,
                                   ByVal o As command,
                                   ByVal act As Func(Of T, command, command, event_comb)) As event_comb
        assert(Not i Is Nothing)
        assert(Not o Is Nothing)
        assert(Not act Is Nothing)
        Dim ec As event_comb = Nothing
        Dim c As container(Of T) = Nothing
        Return New event_comb(Function() As Boolean
                                  Dim b As String = Nothing
                                  If i.parameter(constants.parameter.name, b) AndAlso
                                     container(Of T).create(b, c) Then
                                      assert(Not c Is Nothing)
                                      ec = act(+c, i, o)
                                      Return waitfor(ec) AndAlso
                                             goto_next()
                                  Else
                                      o.attach(constants.response.invalid_request)
                                      Return goto_end()
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert(Not c Is Nothing)
                                  c.release()
                                  Return ec.end_result() AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Function handle(ByVal act As Func(Of T, command, command, event_comb)) _
                                  As Func(Of command, command, event_comb)
        If act Is Nothing Then
            Return Nothing
        Else
            Return Function(i As command, o As command) As event_comb
                       Return handle(i, o, act)
                   End Function
        End If
    End Function

    Public Shared Function register(ByVal dispatcher As dispatcher,
                                    ByVal action() As Byte,
                                    ByVal act As Func(Of T, command, command, event_comb),
                                    Optional ByVal replace As Boolean = False) As Boolean
        Return Not dispatcher Is Nothing AndAlso
               dispatcher.register(action, handle(act), replace)
    End Function

    Public Function register(ByVal action() As Byte,
                             ByVal act As Func(Of T, command, command, event_comb),
                             Optional ByVal replace As Boolean = False) As Boolean
        Return register(dispatcher, action, act, replace)
    End Function

    Public Shared Function register(Of AT)(ByVal dispatcher As dispatcher,
                                           ByVal action As AT,
                                           ByVal act As Func(Of T, command, command, event_comb),
                                           Optional ByVal replace As Boolean = False) _
                                          As Boolean
        Return Not dispatcher Is Nothing AndAlso
               dispatcher.register(action, handle(act), replace)
    End Function

    Public Function register(Of AT)(ByVal action As AT,
                                    ByVal act As Func(Of T, command, command, event_comb),
                                    Optional ByVal replace As Boolean = False) _
                                   As Boolean
        Return register(dispatcher, action, act, replace)
    End Function

    Public Function [erase](ByVal action() As Byte) As Boolean
        Return dispatcher.erase(action)
    End Function

    Public Function [erase](Of AT)(ByVal action As AT) As Boolean
        Return dispatcher.erase(action)
    End Function

    Public Shared Operator +(ByVal this As target_dispatcher(Of T)) As dispatcher
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.dispatcher
    End Operator
End Class
