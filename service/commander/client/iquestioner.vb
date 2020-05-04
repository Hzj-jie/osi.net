
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.procedure
Imports osi.root.template
Imports osi.service.commander.constants

Public MustInherit Class iquestioner(Of _ENABLE_AUTO_PING As _boolean)
    Implements iquestioner
    Private Shared ReadOnly enable_auto_ping As Boolean

    Shared Sub New()
        enable_auto_ping = +(alloc(Of _ENABLE_AUTO_PING)())
    End Sub

    Private ReadOnly last_error As atomic_int

    Protected Sub New()
        If enable_auto_ping Then
            last_error = New atomic_int()
        End If
    End Sub

    Protected MustOverride Function communicate(ByVal request As command,
                                                ByVal response As pointer(Of command)) As event_comb

    Default ReadOnly Property question(ByVal request As command,
                                       ByVal response As pointer(Of command)) As event_comb _
                                      Implements iquestioner.question
        Get
            Dim ec As event_comb = Nothing
            Return New event_comb(Function() As Boolean
                                      If enable_auto_ping AndAlso (+last_error) >= last_error_count Then
                                          Return False
                                      End If
                                      ec = communicate(request, response)
                                      Return waitfor(ec) AndAlso
                                                 goto_next()
                                  End Function,
                                  Function() As Boolean
                                      If ec.end_result() Then
                                          Return goto_end()
                                      Else
                                          If enable_auto_ping AndAlso last_error.increment() = last_error_count Then
                                              start_ping(weak_pointer.of(Me))
                                          End If
                                          Return False
                                      End If
                                  End Function)
        End Get
    End Property

    Default Public ReadOnly Property question(ByVal request() As Byte,
                                              ByVal response As pointer(Of Byte())) As event_comb _
                                             Implements iquestioner.question
        Get
            Return question_redirect(Me, request, response)
        End Get
    End Property

    Default Public ReadOnly Property question(ByVal request As command,
                                              ByVal response As Func(Of command, Boolean)) As event_comb _
                                             Implements iquestioner.question
        Get
            Return question_redirect(Me, request, response)
        End Get
    End Property

    Default Public ReadOnly Property question(ByVal request As command,
                                              ByVal response As Func(Of Boolean)) As event_comb _
                                             Implements iquestioner.question
        Get
            Return question_redirect(Me, request, response)
        End Get
    End Property

    Default Public ReadOnly Property question(ByVal request As command) As event_comb Implements iquestioner.question
        Get
            Return question_redirect(Me, request)
        End Get
    End Property

    Private Function ping_once() As event_comb
        Dim ec As event_comb = Nothing
        Dim o As pointer(Of command) = Nothing
        Return New event_comb(Function() As Boolean
                                  o = New pointer(Of command)()
                                  ec = communicate(command.[New](action.ping), o)
                                  Return waitfor(ec) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return ec.end_result() AndAlso
                                         Not (+o) Is Nothing AndAlso
                                         (+o).action_is(response.success) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Private Shared Sub start_ping(ByVal p As weak_pointer(Of iquestioner(Of _ENABLE_AUTO_PING)))
        assert(enable_auto_ping)
        Dim ec As event_comb = Nothing
        assert_begin(New event_comb(Function() As Boolean
                                        Dim q As iquestioner(Of _ENABLE_AUTO_PING) = Nothing
                                        q = (+p)
                                        If q Is Nothing Then
                                            Return goto_end()
                                        Else
                                            ec = q.ping_once()
                                            Return waitfor(ec) AndAlso
                                                   goto_next()
                                        End If
                                    End Function,
                                    Function() As Boolean
                                        Dim q As iquestioner(Of _ENABLE_AUTO_PING) = Nothing
                                        q = (+p)
                                        If q Is Nothing Then
                                            Return goto_end()
                                        Else
                                            If ec.end_result() Then
                                                q.last_error.set(0)
                                                Return goto_end()
                                            Else
                                                Return waitfor(ping_interval_ms) AndAlso
                                                       goto_begin()
                                            End If
                                        End If
                                    End Function))
    End Sub
End Class
